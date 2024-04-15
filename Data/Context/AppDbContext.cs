using AplicacaoWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AplicacaoWeb.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Filme> Filmes { get; set; }
    }
}