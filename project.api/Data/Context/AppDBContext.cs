﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using  project.api.Data.Models;

namespace project.api.Data.Context
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=db.database.windows.net,1433;Database=project.api_db;User=user;Password=password;Trusted_Connection=False;MultipleActiveResultSets=true");
            /*@"Server=(localdb)\mssqllocaldb;Database=JobsDB;Trusted_Connection=True"*/
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Application>()
            //    .HasOne(p => p.Job)
            //    .WithMany(b => b.Applications);

            //modelBuilder.Entity<Skill>()
            //    .HasOne(p => p.Application)
            //    .WithMany(b => b.Skills);
        }


    }
}