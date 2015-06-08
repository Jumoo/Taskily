namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyAdvanced : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Survey", "CssFile", c => c.String());
            AddColumn("dbo.Survey", "HeaderHtml", c => c.String());
            AddColumn("dbo.Survey", "FooterHtml", c => c.String());
            AddColumn("dbo.Survey", "TasksHeading", c => c.String());
            AddColumn("dbo.Survey", "TasksSubHeading", c => c.String());
            AddColumn("dbo.Survey", "OrderHeading", c => c.String());
            AddColumn("dbo.Survey", "OrderSubHeading", c => c.String());
            AddColumn("dbo.Survey", "OrderText", c => c.String());
            AddColumn("dbo.Survey", "QuestionHeading", c => c.String());
            AddColumn("dbo.Survey", "QuestionSubHeading", c => c.String());
            AddColumn("dbo.Survey", "QuestionText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Survey", "QuestionText");
            DropColumn("dbo.Survey", "QuestionSubHeading");
            DropColumn("dbo.Survey", "QuestionHeading");
            DropColumn("dbo.Survey", "OrderText");
            DropColumn("dbo.Survey", "OrderSubHeading");
            DropColumn("dbo.Survey", "OrderHeading");
            DropColumn("dbo.Survey", "TasksSubHeading");
            DropColumn("dbo.Survey", "TasksHeading");
            DropColumn("dbo.Survey", "FooterHtml");
            DropColumn("dbo.Survey", "HeaderHtml");
            DropColumn("dbo.Survey", "CssFile");
        }
    }
}
