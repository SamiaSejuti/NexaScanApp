namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoctorToDbContextNewwww : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "DayOfWeek", c => c.Int(nullable: false));
            AddColumn("dbo.Bookings", "StartTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Bookings", "EndTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Bookings", "BookingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "BookingTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bookings", "EndTime");
            DropColumn("dbo.Bookings", "StartTime");
            DropColumn("dbo.Bookings", "DayOfWeek");
        }
    }
}
