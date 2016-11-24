namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumn_Information1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Information", "CompanyCode", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Information", "CompanyCode");
        }
    }
}
