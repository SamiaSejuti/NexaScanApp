namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddchnagesToPatient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Patients", "UserId", "dbo.Users");
            DropIndex("dbo.Patients", new[] { "UserId" });
            AddColumn("dbo.Patients", "Name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Patients", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Patients", "ApplicationUserId");
            AddForeignKey("dbo.Patients", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Patients", "Gender");
            DropColumn("dbo.Patients", "UserId");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Patients", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Patients", "Gender", c => c.String(nullable: false));
            DropForeignKey("dbo.Patients", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Patients", new[] { "ApplicationUserId" });
            DropColumn("dbo.Patients", "ApplicationUserId");
            DropColumn("dbo.Patients", "Name");
            CreateIndex("dbo.Patients", "UserId");
            AddForeignKey("dbo.Patients", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
