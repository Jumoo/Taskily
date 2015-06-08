namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrizeDraw_Marketing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Survey", "MarketingOptIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Survey", "MarketingMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Survey", "MarketingMessage");
            DropColumn("dbo.Survey", "MarketingOptIn");
        }
    }
}
