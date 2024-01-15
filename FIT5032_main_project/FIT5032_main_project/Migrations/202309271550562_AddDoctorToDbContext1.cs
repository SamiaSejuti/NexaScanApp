namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoctorToDbContext1 : DbMigration
    {
        public override void Up()
        {
           }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Patients", "User_Id", "dbo.Users");
            DropIndex("dbo.Patients", new[] { "User_Id" });
            DropIndex("dbo.Bookings", new[] { "PatientId" });
            DropColumn("dbo.Bookings", "PatientId");
            DropTable("dbo.Users");
            DropTable("dbo.Patients");
        }
    }
}
