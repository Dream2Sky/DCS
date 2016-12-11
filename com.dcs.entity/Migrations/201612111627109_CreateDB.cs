namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        CompanyCode = c.String(nullable: false, maxLength: 10),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ItemName = c.String(nullable: false, maxLength: 50),
                        ItemContent = c.String(nullable: false, maxLength: 255),
                        InsertMember = c.String(nullable: false, maxLength: 12),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomItemValues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomItemId = c.Guid(nullable: false),
                        ItemName = c.String(nullable: false, maxLength: 50),
                        ItemValue = c.String(maxLength: 255),
                        InforId = c.Guid(nullable: false),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Information",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DataCode = c.String(nullable: false, maxLength: 25),
                        InsertMember = c.String(nullable: false, maxLength: 12),
                        CompanyCode = c.String(nullable: false, maxLength: 10),
                        CustomerName = c.String(maxLength: 20),
                        Sex = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        IsMarry = c.Int(nullable: false),
                        Children = c.Int(nullable: false),
                        Phone = c.String(maxLength: 20),
                        QQ = c.String(maxLength: 20),
                        WebCat = c.String(maxLength: 30),
                        Email = c.String(maxLength: 50),
                        Address = c.String(maxLength: 255),
                        Industry = c.String(maxLength: 30),
                        Occupation = c.String(maxLength: 30),
                        Income = c.String(maxLength: 20),
                        Hobby = c.String(maxLength: 30),
                        HasCar = c.Int(nullable: false),
                        HasHouse = c.Int(nullable: false),
                        InvestProj = c.String(maxLength: 50),
                        InvestConc = c.String(maxLength: 50),
                        InvestLife = c.String(maxLength: 30),
                        Note1 = c.String(maxLength: 255),
                        Note2 = c.String(maxLength: 255),
                        Note3 = c.String(maxLength: 255),
                        State = c.Int(nullable: false),
                        UsageMember = c.String(maxLength: 12),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Role = c.Int(nullable: false),
                        Parent = c.String(nullable: false, maxLength: 12),
                        CompanyCode = c.String(nullable: false, maxLength: 10),
                        Cocount = c.Int(nullable: false),
                        Apcount = c.Int(nullable: false),
                        Ascount = c.Int(nullable: false),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperAdmins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 50),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
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
