namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BithYear", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "BithDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BithDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "BithYear");
        }
    }
}
