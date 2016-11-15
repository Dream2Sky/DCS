namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumns_InsertTime_UpdateTime_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "InsertTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Companies", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CustomItems", "InsertTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CustomItems", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CustomItemValues", "InsertTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CustomItemValues", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Information", "InsertTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Information", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Members", "InsertTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Members", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.SuperAdmins", "InsertTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.SuperAdmins", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SuperAdmins", "UpdateTime");
            DropColumn("dbo.SuperAdmins", "InsertTime");
            DropColumn("dbo.Members", "UpdateTime");
            DropColumn("dbo.Members", "InsertTime");
            DropColumn("dbo.Information", "UpdateTime");
            DropColumn("dbo.Information", "InsertTime");
            DropColumn("dbo.CustomItemValues", "UpdateTime");
            DropColumn("dbo.CustomItemValues", "InsertTime");
            DropColumn("dbo.CustomItems", "UpdateTime");
            DropColumn("dbo.CustomItems", "InsertTime");
            DropColumn("dbo.Companies", "UpdateTime");
            DropColumn("dbo.Companies", "InsertTime");
        }
    }
}
