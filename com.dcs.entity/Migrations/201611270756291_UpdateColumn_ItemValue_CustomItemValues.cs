namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumn_ItemValue_CustomItemValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomItemValues", "ItemValue", c => c.String(maxLength: 255, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomItemValues", "ItemValue", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
        }
    }
}
