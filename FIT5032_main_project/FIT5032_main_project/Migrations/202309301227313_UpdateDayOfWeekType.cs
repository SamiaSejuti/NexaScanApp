namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDayOfWeekType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schedules", "DayOfWeek", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "DayOfWeek", c => c.String());
        }
    }
}
