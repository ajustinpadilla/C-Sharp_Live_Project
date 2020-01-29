namespace TheatreCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRentalRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalRequests", "RentalCode", c => c.Int(nullable: false));
            DropColumn("dbo.RentalRequests", "Attachments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalRequests", "Attachments", c => c.Binary());
            DropColumn("dbo.RentalRequests", "RentalCode");
        }
    }
}
