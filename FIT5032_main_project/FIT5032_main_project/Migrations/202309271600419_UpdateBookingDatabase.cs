﻿namespace FIT5032_main_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookingDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "NewColumn", c => c.String());
        }


        public override void Down()
        {
        }
    }
}
