// <auto-generated />
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(SwapiPlanetsDbContext))]
    [Migration("20210822224016_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Database.Models.Planet", b =>
                {
                    b.Property<long>("PlanetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlanetId");

                    b.ToTable("Planets");
                });

            modelBuilder.Entity("Database.Models.PlanetSpecies", b =>
                {
                    b.Property<long>("PlanetId")
                        .HasColumnType("bigint");

                    b.Property<long>("SpeciesId")
                        .HasColumnType("bigint");

                    b.HasKey("PlanetId", "SpeciesId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("PlanetSpecies");
                });

            modelBuilder.Entity("Database.Models.Species", b =>
                {
                    b.Property<long>("SpeciesId")
                        .HasColumnType("bigint");

                    b.HasKey("SpeciesId");

                    b.ToTable("Species");
                });

            modelBuilder.Entity("Database.Models.PlanetSpecies", b =>
                {
                    b.HasOne("Database.Models.Planet", "Planet")
                        .WithMany("PlanetSpecies")
                        .HasForeignKey("PlanetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.Species", "Species")
                        .WithMany("PlanetSpecies")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
