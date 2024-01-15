namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class admin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Bookings", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            DropIndex("dbo.Bookings", new[] { "ScheduleId" });
            DropIndex("dbo.Bookings", new[] { "PatientId" });
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.AdminId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            DropTable("dbo.Doctors");
            DropTable("dbo.Schedules");
            DropTable("dbo.Bookings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        BookingTime = c.DateTime(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Specialization = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            DropForeignKey("dbo.Admins", "User_Id", "dbo.Users");
            DropIndex("dbo.Admins", new[] { "User_Id" });
            DropTable("dbo.Admins");
            CreateIndex("dbo.Bookings", "PatientId");
            CreateIndex("dbo.Bookings", "ScheduleId");
            CreateIndex("dbo.Schedules", "DoctorId");
            AddForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
            AddForeignKey("dbo.Bookings", "ScheduleId", "dbo.Schedules", "ScheduleId", cascadeDelete: true);
            AddForeignKey("dbo.Bookings", "PatientId", "dbo.Patients", "PatientId", cascadeDelete: true);
        }
    }
}
