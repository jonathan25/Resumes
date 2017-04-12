namespace Resumes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tweak : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
