namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        PatientId = c.String(),
                        DoctorId = c.Int(nullable: false),
                        Patient_PatientId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.Patient_PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.Patient_PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Patient_PatientId", "dbo.Patients");
            DropForeignKey("dbo.Ratings", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Ratings", new[] { "Patient_PatientId" });
            DropIndex("dbo.Ratings", new[] { "DoctorId" });
            DropTable("dbo.Ratings");
        }
    }
}
