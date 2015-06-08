namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyUID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Survey", "UID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Survey", "UID");
        }
    }
}
