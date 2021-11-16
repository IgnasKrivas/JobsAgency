using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using  project.api.Data.Models;

namespace project.api.Data.Context
{
    public class AppDBContext : IdentityDbContext<User>
    {
        //public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=JobsDBv3;Trusted_Connection=True");
            /*@"Server=(localdb)\mssqllocaldb;Database=JobsDB;Trusted_Connection=True"*/
            //@"Server=projectapidbserver.database.windows.net,1433;Database=project.api_db;User=ignaskrivas;Password=Summoner1;Trusted_Connection=False;MultipleActiveResultSets=true"
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Application>()
            //    .HasOne(p => p.Job)
            //    .WithMany(b => b.Applications);

            //modelBuilder.Entity<Skill>()
            //    .HasOne(p => p.Application)
            //    .WithMany(b => b.Skills);
        }


    }
}
