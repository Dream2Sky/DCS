namespace com.dcs.entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumn_Information : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Information", "Sex", c => c.Int(nullable: false));
            AlterColumn("dbo.Information", "IsMarry", c => c.Int(nullable: false));
            AlterColumn("dbo.Information", "Children", c => c.Int(nullable: false));
            AlterColumn("dbo.Information", "HasCar", c => c.Int(nullable: false));
            AlterColumn("dbo.Information", "HasHouse", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Information", "HasHouse", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Information", "HasCar", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Information", "Children", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Information", "IsMarry", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Information", "Sex", c => c.Boolean(nullable: false));
        }
    }
}
