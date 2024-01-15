namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDoctorsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Doctors", "ApplicationUserId");
            AddForeignKey("dbo.Doctors", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }


        public override void Down()
        {
            DropForeignKey("dbo.Doctors", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Doctors", new[] { "ApplicationUserId" });
            DropColumn("dbo.Doctors", "ApplicationUserId");
        }

    }
}
