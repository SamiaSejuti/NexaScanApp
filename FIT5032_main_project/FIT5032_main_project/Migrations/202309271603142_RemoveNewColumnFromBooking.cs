namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNewColumnFromBooking : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "NewColumn");
        }


        public override void Down()
        {
            AddColumn("dbo.Bookings", "NewColumn", c => c.String());
        }

    }
}
