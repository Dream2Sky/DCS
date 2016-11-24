namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumn_Age_Information : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Information", "Age", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Information", "Age", c => c.String(maxLength: 3, storeType: "nvarchar"));
        }
    }
}
