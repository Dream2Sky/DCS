namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_ItemName_CustomItemValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomItemValues", "ItemName", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomItemValues", "ItemName");
        }
    }
}
