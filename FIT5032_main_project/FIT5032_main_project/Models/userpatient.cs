using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FIT5032_main_project.Models
{
    public partial class userpatient : DbContext
    {
        public userpatient()
            : base("name=userpatient")
        {
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Patients)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
