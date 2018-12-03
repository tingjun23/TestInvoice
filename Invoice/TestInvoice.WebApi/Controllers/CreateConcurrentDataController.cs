using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.Service.Services.Interfaces;

namespace TestInvoice.WebApi.Controllers
{
    public class CreateConcurrentDataController : ApiController
    {
        private readonly IClientService _clientService;
        private readonly IGoodAndServiceService _goodAndServiceService;
        private readonly InvoiceDbContext _context;
        private readonly object _locker = new object();

        public CreateConcurrentDataController(IClientService clientService, IGoodAndServiceService goodAndServiceService, InvoiceDbContext context)
        {
            _clientService = clientService;
            _goodAndServiceService = goodAndServiceService;
            _context = context;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateInvoice()
        {
            try
            {
                var rnd = new Random();
                var clients = await _clientService.GetClientsAsync();
                var goodAndServices = await _goodAndServiceService.GetGoodAndServicesAsync();

                var invoices = new List<Invoice>();

                Parallel.ForEach(Enumerable.Range(0, 200000), i =>
                {
                    lock (_locker)
                    {
                        var clientRandomIds = rnd.Next(clients.Min(c => c.Id), clients.Max(c => c.Id) + 1);
                        var randomOrders = rnd.Next(1, 6);

                        var newInvoice = new Invoice
                        {
                            ClientId = clientRandomIds,
                            InvoiceDate = DateTime.Now,
                            IsDeleted = false,
                            Orders = new List<Order>()
                        };

                        for (var j = 0; j < randomOrders; j++)
                        {
                            var goodAndServiceRandomIds = rnd.Next(goodAndServices.Min(g => g.Id),
                                goodAndServices.Max(g => g.Id) + 1);
                            var randomQuantity = rnd.Next(1, 6);

                            newInvoice.Orders.Add(new Order
                            {
                                GoodAndServiceId = goodAndServiceRandomIds,
                                Quantity = randomQuantity,
                                IsDeleted = false
                            });
                        }

                        invoices.Add(newInvoice);
                    }
                });

                BulkInsert(invoices);

                return Ok("Records generated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void BulkInsert(List<Invoice> invoiceTable)
        {
            var connString = _context.Database.Connection.ConnectionString;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var bulkInsertSessionId = InitializeBulkInsertSession(conn, tran);
                    BulkInsertInvoices(invoiceTable, bulkInsertSessionId, true, conn, tran);
                    BulkInsertOrders(invoiceTable, bulkInsertSessionId, conn, tran);
                    tran.Commit();
                }
            }
        }
        private static int InitializeBulkInsertSession(SqlConnection conn, SqlTransaction tran)
        {
            var cmd = new SqlCommand(
                "INSERT INTO dbo.[BulkInsertSession]([CreatedTimeStamp], IsDeleted) VALUES (CURRENT_TIMESTAMP, 0)", conn, tran);

            cmd.ExecuteNonQuery();

            cmd = new SqlCommand(
                "SELECT [Id] FROM dbo.[BulkInsertSession] " +
                "WHERE @@ROWCOUNT > 0 and [Id] = SCOPE_IDENTITY()", conn, tran);

            var bulkInsertSessionId = (int)cmd.ExecuteScalar();

            return bulkInsertSessionId;
        }

        private static void BulkInsertInvoices(List<Invoice> invoiceTable, int bulkInsertSessionId, bool fetchIdentities, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                var invoiceDt = new DataTable();
                invoiceDt.Columns.Add("ClientId", typeof(int));
                invoiceDt.Columns.Add("InvoiceDate", typeof(SqlDateTime));
                invoiceDt.Columns.Add("IsDeleted", typeof(bool));
                invoiceDt.Columns.Add("BulkInsertSessionId", typeof(int));

                invoiceTable.ForEach(i =>
                {
                    invoiceDt.Rows.Add(new object[]
                    {
                        i.ClientId,
                        i.InvoiceDate,
                        i.IsDeleted,
                        bulkInsertSessionId
                    });
                });

                using (var bulkCopy =
                    new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                {

                    bulkCopy.DestinationTableName = "dbo.Invoice";
                    bulkCopy.ColumnMappings.Add("ClientId", "ClientId");
                    bulkCopy.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                    bulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                    bulkCopy.ColumnMappings.Add("BulkInsertSessionId", "BulkInsertSessionId");
                    bulkCopy.BulkCopyTimeout = 300000;

                    bulkCopy.WriteToServer(invoiceDt);

                }

                if (fetchIdentities)
                    LoadInvoiceIdentities(invoiceTable, bulkInsertSessionId, conn, tran);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void LoadInvoiceIdentities(List<Invoice> invoiceTable, int bulkInsertSessionId, SqlConnection conn, SqlTransaction tran)
        {

            var cmd = new SqlCommand(
                "SELECT Id " +
                "FROM dbo.Invoice " +
                "WHERE [BulkInsertSessionID]=@bulkInsertSessionId " +
                "ORDER BY InvoiceDate ASC", conn, tran);
            cmd.Parameters.Add(new SqlParameter("@bulkInsertSessionId", bulkInsertSessionId));

            invoiceTable = invoiceTable.OrderBy(i => i.InvoiceDate).ToList();

            using (var reader = cmd.ExecuteReader())
            {
                var index = 0;
                try
                {
                    while (reader.Read())
                    {
                        var id = (int)reader[0];
                        foreach (var order in invoiceTable[index].Orders)
                        {
                            order.InvoiceId = id;
                        }
                        invoiceTable[index++].Id = id;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private static void BulkInsertOrders(List<Invoice> invoiceTable, int bulkInsertSessionId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                var orderDt = new DataTable();
                orderDt.Columns.Add("GoodAndServiceId", typeof(int));
                orderDt.Columns.Add("Quantity", typeof(int));
                orderDt.Columns.Add("IsDeleted", typeof(bool));
                orderDt.Columns.Add("InvoiceId", typeof(int));

                invoiceTable.ForEach(i =>
                {
                    i.Orders.ForEach(o =>
                    {
                        orderDt.Rows.Add(new object[]
                        {
                            o.GoodAndServiceId,
                            o.Quantity,
                            o.IsDeleted,
                            o.InvoiceId
                        });
                    });
                });

                using (var bulkCopy =
                    new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                {

                    bulkCopy.DestinationTableName = "dbo.[Order]";
                    bulkCopy.ColumnMappings.Add("GoodAndServiceId", "GoodAndServiceId");
                    bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                    bulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                    bulkCopy.ColumnMappings.Add("InvoiceId", "InvoiceId");
                    bulkCopy.BulkCopyTimeout = 300000;

                    bulkCopy.WriteToServer(orderDt);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}