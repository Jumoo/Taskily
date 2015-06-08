namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicResults : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Survey", "PublicResults", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Survey", "PublicResults");
        }
    }
}
