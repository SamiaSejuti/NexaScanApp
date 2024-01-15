namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPatientToBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "PatientId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Bookings", "PatientId", "dbo.Patients", "Id", cascadeDelete: true);
            CreateIndex("dbo.Bookings", "PatientId");
        }



        public override void Down()
        {
            DropIndex("dbo.Bookings", new[] { "PatientId" });
            DropForeignKey("dbo.Bookings", "PatientId", "dbo.Patients");
            DropColumn("dbo.Bookings", "PatientId");
        }
    }
}
