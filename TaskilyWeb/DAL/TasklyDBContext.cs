using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using TaskilyWeb.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TaskilyWeb.DAL
{
    public class TasklyDbContext : IdentityDbContext<TasklyUser>
    {
        public TasklyDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static TasklyDbContext Create()
        {
            return new TasklyDbContext();
        }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyTask> Tasks { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<Response> Responses { get; set; }
        public DbSet<Picked> Picked { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DbSet<PrizeDrawNames> Draws { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // when we add users
            //modelBuilder.Entity<Organisation>()
            // .HasMany()

            modelBuilder.Entity<Picked>()
                .HasRequired(p => p.Task)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}