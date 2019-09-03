namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTaskAndProjectAddedInNotificationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Project_Id", c => c.Int());
            AddColumn("dbo.Notifications", "Task_Id", c => c.Int());
            CreateIndex("dbo.Notifications", "Project_Id");
            CreateIndex("dbo.Notifications", "Task_Id");
            AddForeignKey("dbo.Notifications", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Notifications", "Task_Id", "dbo.UserTasks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Task_Id", "dbo.UserTasks");
            DropForeignKey("dbo.Notifications", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Notifications", new[] { "Task_Id" });
            DropIndex("dbo.Notifications", new[] { "Project_Id" });
            DropColumn("dbo.Notifications", "Task_Id");
            DropColumn("dbo.Notifications", "Project_Id");
        }
    }
}
