namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Detail = c.String(),
                        Time = c.DateTime(nullable: false),
                        IsOpened = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "User_Id" });
            DropTable("dbo.Notifications");
        }
    }
}
