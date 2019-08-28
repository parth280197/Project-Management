namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModalAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompletedPercentage = c.Double(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                        UserTask_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.UserTasks", t => t.UserTask_Id)
                .Index(t => t.User_Id)
                .Index(t => t.UserTask_Id);
            
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompletedPercentage = c.Double(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.UserTaskUsers",
                c => new
                    {
                        UserTask_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserTask_Id, t.User_Id })
                .ForeignKey("dbo.UserTasks", t => t.UserTask_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.UserTask_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "PersonType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTaskUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTaskUsers", "UserTask_Id", "dbo.UserTasks");
            DropForeignKey("dbo.UserTasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "UserTask_Id", "dbo.UserTasks");
            DropForeignKey("dbo.Notes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserTaskUsers", new[] { "User_Id" });
            DropIndex("dbo.UserTaskUsers", new[] { "UserTask_Id" });
            DropIndex("dbo.UserTasks", new[] { "ProjectId" });
            DropIndex("dbo.Notes", new[] { "UserTask_Id" });
            DropIndex("dbo.Notes", new[] { "User_Id" });
            DropIndex("dbo.Projects", new[] { "CreatedBy_Id" });
            DropColumn("dbo.AspNetUsers", "PersonType");
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.UserTaskUsers");
            DropTable("dbo.UserTasks");
            DropTable("dbo.Notes");
            DropTable("dbo.Projects");
        }
    }
}
