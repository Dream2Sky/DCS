namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_ItemContent_CustomItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomItems", "ItemContent", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomItems", "ItemContent");
        }
    }
}
