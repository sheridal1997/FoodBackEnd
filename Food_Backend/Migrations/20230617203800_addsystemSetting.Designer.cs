﻿// <auto-generated />
using System;
using Food_Backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Food_Backend.Migrations
{
    [DbContext(typeof(DbContexD))]
    [Migration("20230617203800_addsystemSetting")]
    partial class addsystemSetting
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Food_Backend.Entity.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EditUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsShuffle")
                        .HasColumnType("bit");

                    b.Property<int?>("RecipeVideoId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("RecipeVideoId");

                    b.ToTable("Recipe");
                });

            modelBuilder.Entity("Food_Backend.Entity.RecipeVideo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EditUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObjectName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("RecipeVideo");
                });

            modelBuilder.Entity("Food_Backend.Entity.SystemSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EditUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("SystemSetting");
                });

            modelBuilder.Entity("Food_Backend.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EditUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserType")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Food_Backend.Entity.WeeklyPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EditUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("RecipeId");

                    b.ToTable("WeeklyPlan");
                });

            modelBuilder.Entity("Food_Backend.Entity.Recipe", b =>
                {
                    b.HasOne("Food_Backend.Entity.RecipeVideo", "RecipeVideo")
                        .WithMany("Recipe")
                        .HasForeignKey("RecipeVideoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("RecipeVideo");
                });

            modelBuilder.Entity("Food_Backend.Entity.WeeklyPlan", b =>
                {
                    b.HasOne("Food_Backend.Entity.Recipe", "Recipe")
                        .WithMany("WeeklyPlans")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Food_Backend.Entity.Recipe", b =>
                {
                    b.Navigation("WeeklyPlans");
                });

            modelBuilder.Entity("Food_Backend.Entity.RecipeVideo", b =>
                {
                    b.Navigation("Recipe");
                });
#pragma warning restore 612, 618
        }
    }
}
