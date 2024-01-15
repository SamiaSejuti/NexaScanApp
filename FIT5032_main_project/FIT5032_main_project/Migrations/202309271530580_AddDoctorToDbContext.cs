namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoctorToDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Specialization = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        BookingTime = c.DateTime(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .Index(t => t.ScheduleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Bookings", "ScheduleId", "dbo.Schedules");
            DropIndex("dbo.Bookings", new[] { "ScheduleId" });
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            DropTable("dbo.Bookings");
            DropTable("dbo.Schedules");
            DropTable("dbo.Doctors");
        }
    }
}
