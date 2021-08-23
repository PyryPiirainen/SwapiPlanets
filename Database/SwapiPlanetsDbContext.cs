using DomainModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class SwapiPlanetsDbContext : DbContext
    {
        public SwapiPlanetsDbContext(DbContextOptions<SwapiPlanetsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Planet> Planets { get; set; }
        public DbSet<PlanetSpecies> PlanetSpecies { get; set; }
        public DbSet<Species> Species { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Planet>()
                .HasMany(p => p.PlanetSpecies)
                .WithOne(ps => ps.Planet)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Planet>()
                .Ignore(p => p.Species);

            builder.Entity<Species>()
                .Property(s => s.SpeciesId)
                .ValueGeneratedNever();

            builder.Entity<PlanetSpecies>()
                .HasKey(ps => new { ps.PlanetId, ps.SpeciesId });
            builder.Entity<PlanetSpecies>()
                .HasOne(ps => ps.Planet)
                .WithMany(p => p.PlanetSpecies)
                .HasForeignKey(ps => ps.PlanetId);
            builder.Entity<PlanetSpecies>()
                .HasOne(ps => ps.Species)
                .WithMany(s => s.PlanetSpecies)
                .HasForeignKey(ps => ps.SpeciesId);
        }
    }
}
