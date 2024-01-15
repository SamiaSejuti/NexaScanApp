namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddchnagesToSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "DayOfWeek", c => c.String());
            AlterColumn("dbo.Schedules", "StartTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Schedules", "EndTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Schedules", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "DayOfWeek");
        }
    }
}
