namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admins", "Name");
        }
    }
}
