namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddchnagesToSchedulepart : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            AddColumn("dbo.Schedules", "Doctor_DoctorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Schedules", "DoctorId", c => c.String());
            CreateIndex("dbo.Schedules", "Doctor_DoctorId");
            AddForeignKey("dbo.Schedules", "Doctor_DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "Doctor_DoctorId", "dbo.Doctors");
            DropIndex("dbo.Schedules", new[] { "Doctor_DoctorId" });
            AlterColumn("dbo.Schedules", "DoctorId", c => c.Int(nullable: false));
            DropColumn("dbo.Schedules", "Doctor_DoctorId");
            CreateIndex("dbo.Schedules", "DoctorId");
            AddForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
        }
    }
}
