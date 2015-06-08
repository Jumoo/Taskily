namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrizeDraw : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrizeDrawNames",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(),
                        Name = c.String(),
                        Email = c.String(),
                        contact = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survey", t => t.SurveyID)
                .Index(t => t.SurveyID);
            
            AddColumn("dbo.Survey", "Draw", c => c.Boolean(nullable: false));
            AddColumn("dbo.Survey", "DrawTitle", c => c.String());
            AddColumn("dbo.Survey", "DrawMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrizeDrawNames", "SurveyID", "dbo.Survey");
            DropIndex("dbo.PrizeDrawNames", new[] { "SurveyID" });
            DropColumn("dbo.Survey", "DrawMessage");
            DropColumn("dbo.Survey", "DrawTitle");
            DropColumn("dbo.Survey", "Draw");
            DropTable("dbo.PrizeDrawNames");
        }
    }
}
