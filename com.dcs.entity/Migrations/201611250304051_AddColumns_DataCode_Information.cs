namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumns_DataCode_Information : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Information", "DataCode", c => c.String(nullable: false, maxLength: 25, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Information", "DataCode");
        }
    }
}
