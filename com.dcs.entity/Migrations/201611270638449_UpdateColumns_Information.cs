namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumns_Information : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomItemValues", "ItemValue", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Information", "InsertMember", c => c.String(nullable: false, maxLength: 12, storeType: "nvarchar"));
            AlterColumn("dbo.Information", "CompanyCode", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
            AlterColumn("dbo.Information", "UsageMember", c => c.String(maxLength: 12, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Information", "UsageMember", c => c.String(unicode: false));
            AlterColumn("dbo.Information", "CompanyCode", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Information", "InsertMember", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.CustomItemValues", "ItemValue", c => c.String(nullable: false, unicode: false));
        }
    }
}
