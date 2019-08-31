namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotationUpdatedInUserTasksTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserTasks", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.UserTasks", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserTasks", "Description", c => c.String());
            AlterColumn("dbo.UserTasks", "Name", c => c.String());
        }
    }
}
