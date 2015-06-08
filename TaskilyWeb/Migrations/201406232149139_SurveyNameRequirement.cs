namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyNameRequirement : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Survey", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Survey", "Name", c => c.String(nullable: false));
        }
    }
}
