namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumns_Count_Member : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Cocount", c => c.Int(nullable: false));
            AddColumn("dbo.Members", "Apcount", c => c.Int(nullable: false));
            AddColumn("dbo.Members", "Ascount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Ascount");
            DropColumn("dbo.Members", "Apcount");
            DropColumn("dbo.Members", "Cocount");
        }
    }
}
