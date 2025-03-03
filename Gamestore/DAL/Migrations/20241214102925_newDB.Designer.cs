﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(GameContext))]
    [Migration("20241214102925_newDB")]
    partial class NewDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("DAL.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("ParentGenreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentGenreId");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = new Guid("46165ae2-b7b5-4147-a6cf-bf1d19611b7f"),
                            Name = "Strategy"
                        },
                        new
                        {
                            Id = new Guid("26e0605a-260f-430c-86af-154bfee66e58"),
                            Name = "RTS",
                            ParentGenreId = new Guid("46165ae2-b7b5-4147-a6cf-bf1d19611b7f")
                        },
                        new
                        {
                            Id = new Guid("e1250942-0bfe-46fb-9059-f9cc2e79428a"),
                            Name = "TBS",
                            ParentGenreId = new Guid("46165ae2-b7b5-4147-a6cf-bf1d19611b7f")
                        },
                        new
                        {
                            Id = new Guid("def411c6-6ca6-4506-9a3a-34ed2999b9d5"),
                            Name = "RPG"
                        },
                        new
                        {
                            Id = new Guid("b33bbde2-0a2e-499e-b70f-bb6879e38795"),
                            Name = "Sports"
                        },
                        new
                        {
                            Id = new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813"),
                            Name = "Races",
                            ParentGenreId = new Guid("b33bbde2-0a2e-499e-b70f-bb6879e38795")
                        },
                        new
                        {
                            Id = new Guid("bf0dfa8f-27d5-41ad-8e07-b2f6e5e39061"),
                            Name = "Rally",
                            ParentGenreId = new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813")
                        },
                        new
                        {
                            Id = new Guid("dbbf13cd-ddd3-4b0b-9c49-5474ccae051d"),
                            Name = "Arcade",
                            ParentGenreId = new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813")
                        },
                        new
                        {
                            Id = new Guid("5598feb0-6a7c-4a04-95c6-5f126ac69b68"),
                            Name = "Formula",
                            ParentGenreId = new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813")
                        },
                        new
                        {
                            Id = new Guid("26be36b2-cceb-4222-84ce-aaa9f3ff5d2c"),
                            Name = "Off-road",
                            ParentGenreId = new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813")
                        },
                        new
                        {
                            Id = new Guid("3768cb72-a96b-4894-ad41-5ba15c8f4307"),
                            Name = "Action"
                        },
                        new
                        {
                            Id = new Guid("01d77623-f0be-47ab-9446-8bcc5c8650b9"),
                            Name = "FPS",
                            ParentGenreId = new Guid("3768cb72-a96b-4894-ad41-5ba15c8f4307")
                        },
                        new
                        {
                            Id = new Guid("5266fca4-4016-47dd-9542-174f53c43b1a"),
                            Name = "TPS",
                            ParentGenreId = new Guid("3768cb72-a96b-4894-ad41-5ba15c8f4307")
                        },
                        new
                        {
                            Id = new Guid("96a88ced-7d69-42c2-a658-6c6280ab9bb1"),
                            Name = "Adventure"
                        },
                        new
                        {
                            Id = new Guid("3dd316d1-fa85-455e-a21d-b367f12635a3"),
                            Name = "Puzzle & Skill"
                        });
                });

            modelBuilder.Entity("DAL.Entities.Platform", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.Property<Guid>("GamesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GenresId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GamesId", "GenresId");

                    b.HasIndex("GenresId");

                    b.ToTable("GameGenre");
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.Property<Guid>("GamesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlatformsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GamesId", "PlatformsId");

                    b.HasIndex("PlatformsId");

                    b.ToTable("GamePlatform");
                });

            modelBuilder.Entity("DAL.Entities.Genre", b =>
                {
                    b.HasOne("DAL.Entities.Genre", "ParentGenre")
                        .WithMany("ChildGenres")
                        .HasForeignKey("ParentGenreId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentGenre");
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.HasOne("DAL.Entities.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.HasOne("DAL.Entities.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Platform", null)
                        .WithMany()
                        .HasForeignKey("PlatformsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Entities.Genre", b =>
                {
                    b.Navigation("ChildGenres");
                });
#pragma warning restore 612, 618
        }
    }
}
