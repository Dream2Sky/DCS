namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlertColumn_Parent_Member : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Parent", c => c.String(nullable: false, maxLength: 12, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Parent", c => c.String(nullable: false, unicode: false));
        }
    }
}
