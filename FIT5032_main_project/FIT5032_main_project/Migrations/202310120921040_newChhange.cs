namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newChhange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "BookingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "ScheduleDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bookings", "DayOfWeek");
            DropColumn("dbo.Schedules", "DayOfWeek");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "DayOfWeek", c => c.Int(nullable: false));
            AddColumn("dbo.Bookings", "DayOfWeek", c => c.Int(nullable: false));
            DropColumn("dbo.Schedules", "ScheduleDate");
            DropColumn("dbo.Bookings", "BookingDate");
        }
    }
}
