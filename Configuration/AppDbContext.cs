using AllRiskSolutions_Desafio.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AllRiskSolutions_Desafio.Configuration;

public class AppDbContext(IWebHostEnvironment env, VariableProvider variableProvider) : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<City> Cities { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.FavoriteCities)
            .WithMany();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(variableProvider.GetSqlDatabaseConnectionString());
    }
}