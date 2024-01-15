namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDayOfWeekType1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "IsDefaultScheduleAdded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "IsDefaultScheduleAdded");
        }
    }
}
