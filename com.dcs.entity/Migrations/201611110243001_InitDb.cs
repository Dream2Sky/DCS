namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        CompanyCode = c.String(nullable: false, maxLength: 10, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ItemName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        InsertMember = c.String(nullable: false, maxLength: 12, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomItemValues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomItemId = c.Guid(nullable: false),
                        ItemValue = c.String(nullable: false, unicode: false),
                        InforId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Information",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InsertMember = c.String(nullable: false, unicode: false),
                        CustomerName = c.String(maxLength: 20, storeType: "nvarchar"),
                        Sex = c.Boolean(nullable: false),
                        Age = c.String(maxLength: 3, storeType: "nvarchar"),
                        IsMarry = c.Boolean(nullable: false),
                        Children = c.Boolean(nullable: false),
                        Phone = c.String(maxLength: 20, storeType: "nvarchar"),
                        QQ = c.String(maxLength: 20, storeType: "nvarchar"),
                        WebCat = c.String(maxLength: 30, storeType: "nvarchar"),
                        Email = c.String(maxLength: 50, storeType: "nvarchar"),
                        Address = c.String(maxLength: 255, storeType: "nvarchar"),
                        Industry = c.String(maxLength: 30, storeType: "nvarchar"),
                        Occupation = c.String(maxLength: 30, storeType: "nvarchar"),
                        Income = c.String(maxLength: 20, storeType: "nvarchar"),
                        Hobby = c.String(maxLength: 30, storeType: "nvarchar"),
                        HasCar = c.Boolean(nullable: false),
                        HasHouse = c.Boolean(nullable: false),
                        InvestProj = c.String(maxLength: 50, storeType: "nvarchar"),
                        InvestConc = c.String(maxLength: 50, storeType: "nvarchar"),
                        InvestLife = c.String(maxLength: 30, storeType: "nvarchar"),
                        Note1 = c.String(maxLength: 255, storeType: "nvarchar"),
                        Note2 = c.String(maxLength: 255, storeType: "nvarchar"),
                        Note3 = c.String(maxLength: 255, storeType: "nvarchar"),
                        State = c.Int(nullable: false),
                        UsageMember = c.String(unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 12, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Role = c.Int(nullable: false),
                        CompanyCode = c.String(nullable: false, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperAdmins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SuperAdmins");
            DropTable("dbo.Members");
            DropTable("dbo.Information");
            DropTable("dbo.CustomItemValues");
            DropTable("dbo.CustomItems");
            DropTable("dbo.Companies");
        }
    }
}
