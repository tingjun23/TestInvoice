using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using TestInvoice.Core.Dto;
using TestInvoice.DataAccess.Entity;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.DataAccess.Repositories.Interfaces;
using TestInvoice.Service.Services.Interfaces;

namespace TestInvoice.Service.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IGoodAndServiceRepository _goodAndServiceRepository;

        public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository, IClientRepository clientRepository, IGoodAndServiceRepository goodAndServiceRepository)
        {
            _mapper = mapper;
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _goodAndServiceRepository = goodAndServiceRepository;
        }

        public async Task<List<InvoiceDto>> GetInvoiceByIdAsync(int invoiceId)
        {
            if (_invoiceRepository == null) return null;
            try
            {
                var invoice = await _invoiceRepository.Get()
                    .Include(i => i.Client)
                    .Include(i => i.Orders.Select(o => o.GoodAndService))
                    .Where(i => i.Id == invoiceId).ToListAsync();
                return _mapper.Map<List<InvoiceDto>>(invoice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<InvoiceDto>> GetInvoicesAsync()
        {
            try
            {
                var invoices = await _invoiceRepository.Get()
                    .Include(i => i.Client)
                    .Include(i => i.Orders.Select(o => o.GoodAndService))
                    .ToListAsync();
                return _mapper.Map<List<InvoiceDto>>(invoices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<InvoiceDto>> CreateInvoiceAsync(List<InvoiceDto> invoices)
        {
            try
            {
                var clients = await _clientRepository.Get().ToListAsync();
                var goodAndServices = await _goodAndServiceRepository.Get().ToListAsync();
                var newInvoices = new List<Invoice>();

                if (clients.Count == 0)
                    throw new Exception("Client record is unavailable.");

                if (goodAndServices.Count == 0)
                    throw new Exception("Good and Service record is unavailable.");

                invoices.ForEach(i =>
                {
                    if (i.Id != 0)
                        throw new Exception("Error! This is an existing invoice.");

                    if (i.ClientId == 0 && !clients.Any(c => c.Id == i.ClientId))
                        throw new Exception("Invalid selected client.");

                    var newInvoice = new Invoice
                    {
                        ClientId = i.ClientId,
                        InvoiceDate = DateTime.Now,
                        IsDeleted = false,
                        Orders = new List<Order>()
                    };

                    var goodAndServiceIds = new List<int>();

                    i.Orders.ForEach(o =>
                    {
                        if (o.Id != 0)
                            throw new Exception("Error! This is an existing order.");

                        if (o.GoodAndServiceId == 0 && !goodAndServices.Any(g => g.Id == o.GoodAndServiceId))
                            throw new Exception("Invalid selected good and service.");

                        if (goodAndServiceIds.Contains(o.GoodAndServiceId))
                            throw new Exception("Similar Good and Service selected. Kindly verify.");

                        if (o.Quantity <= 0)
                            throw new Exception("Invalid quantity.");

                        newInvoice.Orders.Add(new Order
                        {
                            GoodAndServiceId = o.GoodAndServiceId,
                            Quantity = o.Quantity,
                            IsDeleted = false
                        });

                        goodAndServiceIds.Add(o.GoodAndServiceId);
                    });
                    newInvoices.Add(newInvoice);
                    _invoiceRepository.Add(newInvoice);
                });

                _invoiceRepository.SaveChanges();

                return _mapper.Map<List<InvoiceDto>>(newInvoices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<InvoiceDto>> UpdateInvoiceAsync(List<InvoiceDto> invoices)
        {
            var editedInvoice = new List<Invoice>();

            try
            {
                var clients = await _clientRepository.Get().ToListAsync();
                var goodAndServices = await _goodAndServiceRepository.Get().ToListAsync();
                var invoiceIds = invoices.Select(i => i.Id).ToList();
                var existingInvoices = await _invoiceRepository.Get()
                    .Include(i => i.Client)
                    .Include(i => i.Orders.Select(o => o.GoodAndService))
                    .Where(i => invoiceIds.Contains(i.Id))
                    .ToListAsync();
                Invoice existingInvoice;
                Order existingOrder;

                invoices.ForEach(i =>
                {
                    if (existingInvoices.Any(ei => ei.Id == i.Id))
                        existingInvoice = existingInvoices.FirstOrDefault(ei => ei.Id == i.Id);
                    else
                        throw new Exception("Invalid invoice edited.");

                    if (i.ClientId == 0 && !clients.Any(c => c.Id == i.ClientId))
                        throw new Exception("Invalid selected client.");

                    if (existingInvoice != null)
                    {
                        existingInvoice.ClientId = i.ClientId;
                        existingInvoice.InvoiceDate = DateTime.Now;
                        existingInvoice.Orders.ToList().ForEach(o => o.IsDeleted = true);

                        i.Orders.ForEach(o =>
                        {
                            if (o.GoodAndServiceId == 0 && !goodAndServices.Any(g => g.Id == o.GoodAndServiceId))
                                throw new Exception("Invalid selected good and service.");

                            if (o.Quantity <= 0)
                                throw new Exception("Invalid quantity.");

                            if (existingInvoice.Orders.Any(eo => eo.Id == o.Id))
                            {
                                existingOrder = existingInvoice.Orders.First(eo => eo.Id == o.Id);

                                existingOrder.GoodAndServiceId = o.GoodAndServiceId;
                                existingOrder.Quantity = o.Quantity;
                                existingOrder.IsDeleted = false;
                            }
                            else
                            {
                                existingInvoice.Orders.Add(new Order
                                {
                                    InvoiceId = i.Id,
                                    GoodAndServiceId = o.GoodAndServiceId,
                                    Quantity = o.Quantity
                                });
                            }

                        });
                        _invoiceRepository.Update(existingInvoice);
                        editedInvoice.Add(existingInvoice);
                    }
                });

                _invoiceRepository.SaveChanges();

                return _mapper.Map<List<InvoiceDto>>(editedInvoice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
