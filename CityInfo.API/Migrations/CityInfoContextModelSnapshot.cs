﻿// <auto-generated />
using CityInfo.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityInfo.API.Migrations
{
    [DbContext(typeof(CityInfoContext))]
    partial class CityInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "It's a crowded place, but fun!",
                            Name = "Ho Chi Minh City"
                        },
                        new
                        {
                            Id = 2,
                            Description = "I love this city so much, it's my hometown.",
                            Name = "Quy Nhon City"
                        },
                        new
                        {
                            Id = 3,
                            Description = "This island is a bright gem!",
                            Name = "Phu Quoc City"
                        });
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointsOfInterest");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Description = "The highest building of the city.",
                            Name = "Landmark 81"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 1,
                            Description = "The most beautiful church in the city.",
                            Name = "Duc Ba Church"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 1,
                            Description = "One of the oldest zoo in the world.",
                            Name = "The Saigon Zoo"
                        },
                        new
                        {
                            Id = 4,
                            CityId = 2,
                            Description = "Fun, crowded, big.",
                            Name = "The Quy Nhon Centre Square"
                        },
                        new
                        {
                            Id = 5,
                            CityId = 2,
                            Description = "Enjoy sea food, and the sea atmosphere.",
                            Name = "Hon Kho Island"
                        },
                        new
                        {
                            Id = 6,
                            CityId = 2,
                            Description = "The beauty of the beach will amaze you.",
                            Name = "Ky Co Beach"
                        },
                        new
                        {
                            Id = 7,
                            CityId = 3,
                            Description = "Relax, enjoy, fresh air, beautiful beach.",
                            Name = "Bai Khem"
                        },
                        new
                        {
                            Id = 8,
                            CityId = 3,
                            Description = "It looks like a new place.",
                            Name = "Vinwonders"
                        },
                        new
                        {
                            Id = 9,
                            CityId = 3,
                            Description = "Fresh and delicious.",
                            Name = "Bun Quay"
                        });
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.HasOne("CityInfo.API.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Navigation("PointsOfInterest");
                });
#pragma warning restore 612, 618
        }
    }
}
