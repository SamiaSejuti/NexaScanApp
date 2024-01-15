namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePatient : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Patients", "Id", "PatientId");
            DropPrimaryKey("dbo.Patients");
            AddPrimaryKey("dbo.Patients", "PatientId");
        }

        public override void Down()
        {
            // to revert the changes
            DropPrimaryKey("dbo.Patients");
            RenameColumn("dbo.Patients", "PatientId", "Id");
            AddPrimaryKey("dbo.Patients", "Id");
        }

    }
}
