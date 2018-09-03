using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Models;

namespace TaskPlanner.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            builder.Entity<ApplicationUser>().HasIndex(m => m.Email).
                 IsUnique(true);

            builder.Entity<ApplicationUser>().HasIndex(m => m.UserName).
                IsUnique(true);

            builder.Entity<TaskModel>().HasIndex(m => m.TaskName).
                IsUnique(true);

            builder.Entity<TaskModel>().
                HasOne(m => m.ApplicationUser).
                WithMany(m => m.Tasks).
                HasForeignKey(m => m.UserId);
        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
