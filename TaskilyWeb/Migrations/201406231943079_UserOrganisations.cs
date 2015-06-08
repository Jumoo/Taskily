namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserOrganisations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "OrganisationID", "dbo.Organisation");
            DropIndex("dbo.AspNetUsers", new[] { "OrganisationID" });
            AlterColumn("dbo.AspNetUsers", "OrganisationID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "OrganisationID");
            AddForeignKey("dbo.AspNetUsers", "OrganisationID", "dbo.Organisation", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "OrganisationID", "dbo.Organisation");
            DropIndex("dbo.AspNetUsers", new[] { "OrganisationID" });
            AlterColumn("dbo.AspNetUsers", "OrganisationID", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "OrganisationID");
            AddForeignKey("dbo.AspNetUsers", "OrganisationID", "dbo.Organisation", "ID", cascadeDelete: true);
        }
    }
}
