// <auto-generated />
using System;
using HigherOrLower.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HigherOrLower.Infrastructure.Migrations
{
    [DbContext(typeof(HigherOrLowerDbContext))]
    partial class HigherOrLowerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HigherOrLower.Infrastructure.Entities.CardEntity", b =>
                {
                    b.Property<Guid>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FaceValue")
                        .HasColumnType("int");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NextValue")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CardId");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Card");
                });

            modelBuilder.Entity("HigherOrLower.Infrastructure.Entities.GameEntity", b =>
                {
                    b.Property<Guid>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FaceValue")
                        .HasColumnType("int");

                    b.Property<bool>("IsGameOver")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("RemainingCards")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("GameId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("HigherOrLower.Infrastructure.Entities.CardEntity", b =>
                {
                    b.HasOne("HigherOrLower.Infrastructure.Entities.GameEntity", "Game")
                        .WithOne("CardEntity")
                        .HasForeignKey("HigherOrLower.Infrastructure.Entities.CardEntity", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
