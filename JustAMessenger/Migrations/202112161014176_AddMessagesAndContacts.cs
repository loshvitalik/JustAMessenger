namespace JustAMessenger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagesAndContacts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactRelations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InitiatorUserId = c.String(),
                        ReceiverUserId = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        SenderId = c.String(),
                        ReceiverId = c.String(),
                        SenderName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
            DropTable("dbo.ContactRelations");
        }
    }
}
