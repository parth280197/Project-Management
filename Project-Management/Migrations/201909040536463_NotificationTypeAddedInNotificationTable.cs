namespace Project_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationTypeAddedInNotificationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "NotificationType");
        }
    }
}
