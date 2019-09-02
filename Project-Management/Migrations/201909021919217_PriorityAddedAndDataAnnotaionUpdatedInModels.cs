namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriorityAddedAndDataAnnotaionUpdatedInModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Priority", c => c.Int(nullable: false));
            AddColumn("dbo.UserTasks", "priority", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Notes", "Comment", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notes", "Comment", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropColumn("dbo.UserTasks", "priority");
            DropColumn("dbo.Projects", "Priority");
        }
    }
}
