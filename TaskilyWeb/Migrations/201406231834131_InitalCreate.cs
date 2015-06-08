namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ResponseID = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                        AnswerNo = c.Int(nullable: false),
                        AnswerText = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Response", t => t.ResponseID, cascadeDelete: true)
                .ForeignKey("dbo.Question", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.ResponseID)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(nullable: false),
                        Name = c.String(),
                        Text = c.String(),
                        Type = c.Int(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survey", t => t.SurveyID, cascadeDelete: true)
                .Index(t => t.SurveyID);
            
            CreateTable(
                "dbo.Survey",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrganisationID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        TaskCount = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        WelcomeTitle = c.String(),
                        WelcomeMessage = c.String(),
                        CompleteTitle = c.String(),
                        CompleteMessage = c.String(),
                        EndUrl = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organisation", t => t.OrganisationID, cascadeDelete: true)
                .Index(t => t.OrganisationID);
            
            CreateTable(
                "dbo.Organisation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Website = c.String(),
                        Address = c.String(),
                        AccountType = c.Int(nullable: false),
                        TrialDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Response",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(),
                        Completed = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survey", t => t.SurveyID)
                .Index(t => t.SurveyID);
            
            CreateTable(
                "dbo.Picked",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ResponseID = c.Int(nullable: false),
                        SurveyTaskID = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Response", t => t.ResponseID, cascadeDelete: true)
                .ForeignKey("dbo.SurveyTask", t => t.SurveyTaskID)
                .Index(t => t.ResponseID)
                .Index(t => t.SurveyTaskID);
            
            CreateTable(
                "dbo.SurveyTask",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survey", t => t.SurveyID, cascadeDelete: true)
                .Index(t => t.SurveyID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrganisationID = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organisation", t => t.OrganisationID, cascadeDelete: true)
                .Index(t => t.OrganisationID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "OrganisationID", "dbo.Organisation");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Answer", "QuestionID", "dbo.Question");
            DropForeignKey("dbo.Response", "SurveyID", "dbo.Survey");
            DropForeignKey("dbo.Picked", "SurveyTaskID", "dbo.SurveyTask");
            DropForeignKey("dbo.SurveyTask", "SurveyID", "dbo.Survey");
            DropForeignKey("dbo.Picked", "ResponseID", "dbo.Response");
            DropForeignKey("dbo.Answer", "ResponseID", "dbo.Response");
            DropForeignKey("dbo.Question", "SurveyID", "dbo.Survey");
            DropForeignKey("dbo.Survey", "OrganisationID", "dbo.Organisation");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "OrganisationID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SurveyTask", new[] { "SurveyID" });
            DropIndex("dbo.Picked", new[] { "SurveyTaskID" });
            DropIndex("dbo.Picked", new[] { "ResponseID" });
            DropIndex("dbo.Response", new[] { "SurveyID" });
            DropIndex("dbo.Survey", new[] { "OrganisationID" });
            DropIndex("dbo.Question", new[] { "SurveyID" });
            DropIndex("dbo.Answer", new[] { "QuestionID" });
            DropIndex("dbo.Answer", new[] { "ResponseID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SurveyTask");
            DropTable("dbo.Picked");
            DropTable("dbo.Response");
            DropTable("dbo.Organisation");
            DropTable("dbo.Survey");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
