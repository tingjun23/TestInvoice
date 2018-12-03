namespace TestInvoice.DataAccess.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BulkInsertSession",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedTimeStamp = c.Time(nullable: false, precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BulkInsertSession_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 1000),
                        Company = c.String(maxLength: 1000),
                        Address = c.String(maxLength: 1000),
                        City = c.String(maxLength: 1000),
                        State = c.String(maxLength: 1000),
                        Phone = c.String(maxLength: 1000),
                        Email = c.String(maxLength: 1000),
                        Website = c.String(maxLength: 1000),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Client_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoodAndService",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 1000),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GoodAndService_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        BulkInsertSessionId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Invoice_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodAndServiceId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Order_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodAndService", t => t.GoodAndServiceId)
                .ForeignKey("dbo.Invoice", t => t.InvoiceId)
                .Index(t => t.GoodAndServiceId)
                .Index(t => t.InvoiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.Order", "GoodAndServiceId", "dbo.GoodAndService");
            DropForeignKey("dbo.Invoice", "ClientId", "dbo.Client");
            DropIndex("dbo.Order", new[] { "InvoiceId" });
            DropIndex("dbo.Order", new[] { "GoodAndServiceId" });
            DropIndex("dbo.Invoice", new[] { "ClientId" });
            DropTable("dbo.Order",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Order_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Invoice",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Invoice_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GoodAndService",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GoodAndService_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Client",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Client_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.BulkInsertSession",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BulkInsertSession_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
