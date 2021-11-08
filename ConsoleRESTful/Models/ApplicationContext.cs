using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRESTful.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().Property(p => p.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Project>().Property(p => p.CreatedAt).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Project>().Property(p => p.Filepath).IsRequired();
            modelBuilder.Entity<Project>().Property(p => p.ProjectType).HasMaxLength(10);
        }
    }
}
