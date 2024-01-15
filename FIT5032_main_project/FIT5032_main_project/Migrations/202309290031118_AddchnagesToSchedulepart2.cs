namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddchnagesToSchedulepart2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Schedules", new[] { "Doctor_DoctorId" });
            DropColumn("dbo.Schedules", "DoctorId");
            RenameColumn(table: "dbo.Schedules", name: "Doctor_DoctorId", newName: "DoctorId");
            AlterColumn("dbo.Schedules", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Schedules", "DoctorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            AlterColumn("dbo.Schedules", "DoctorId", c => c.String());
            RenameColumn(table: "dbo.Schedules", name: "DoctorId", newName: "Doctor_DoctorId");
            AddColumn("dbo.Schedules", "DoctorId", c => c.String());
            CreateIndex("dbo.Schedules", "Doctor_DoctorId");
        }
    }
}
