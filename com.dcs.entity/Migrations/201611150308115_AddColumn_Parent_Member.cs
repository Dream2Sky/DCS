namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_Parent_Member : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Parent", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Parent");
        }
    }
}
