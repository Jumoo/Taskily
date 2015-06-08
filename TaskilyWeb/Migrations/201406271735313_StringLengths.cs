namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringLengths : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Survey", "UID", c => c.String(maxLength: 24));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Survey", "UID", c => c.String());
        }
    }
}
