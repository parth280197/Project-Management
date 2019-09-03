namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeadlineAddedToTaskTableAndProjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTasks", "Deadline", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "Deadline", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Deadline");
            DropColumn("dbo.UserTasks", "Deadline");
        }
    }
}
