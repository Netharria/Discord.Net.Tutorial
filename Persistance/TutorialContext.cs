using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistance
{
    public class TutorialContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<AutoRole> AutoRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql("server=localhost;user=root;database=tutorial;port=3306;Connect Timeout=5;");

    }
}
