using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AplicacaoWeb.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<UserScreen> UserScreens { get; set; }
        public DbSet<ImageUrlAndName> ImageUrlAndNames { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}