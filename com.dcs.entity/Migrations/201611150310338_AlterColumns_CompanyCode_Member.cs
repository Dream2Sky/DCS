namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumns_CompanyCode_Member : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "CompanyCode", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "CompanyCode", c => c.String(nullable: false, unicode: false));
        }
    }
}
