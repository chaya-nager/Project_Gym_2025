﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mock;

#nullable disable

namespace Mock.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20250628193138_init10")]
    partial class init10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Repository.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HealthConditions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Repository.Entities.UserWorkoutPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserWorkoutPlans");
                });

            modelBuilder.Entity("Repository.Entities.WorkoutVideo", b =>
                {
                    b.Property<int>("VideoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VideoId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DifficultyLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("TargetAudience")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserWorkoutPlanId")
                        .HasColumnType("int");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkoutType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WorkoutVideo")
                        .HasColumnType("int");

                    b.HasKey("VideoId");

                    b.HasIndex("TrainerId");

                    b.HasIndex("UserWorkoutPlanId");

                    b.ToTable("WorkoutVideos");
                });

            modelBuilder.Entity("Repository.Entities.UserWorkoutPlan", b =>
                {
                    b.HasOne("Repository.Entities.User", "User")
                        .WithOne("UserWorkoutPlan")
                        .HasForeignKey("Repository.Entities.UserWorkoutPlan", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Repository.Entities.WorkoutVideo", b =>
                {
                    b.HasOne("Repository.Entities.User", "Trainer")
                        .WithMany("WorkoutVideos")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Entities.UserWorkoutPlan", "UserWorkoutPlan")
                        .WithMany("WorkoutPlanVideos")
                        .HasForeignKey("UserWorkoutPlanId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Trainer");

                    b.Navigation("UserWorkoutPlan");
                });

            modelBuilder.Entity("Repository.Entities.User", b =>
                {
                    b.Navigation("UserWorkoutPlan")
                        .IsRequired();

                    b.Navigation("WorkoutVideos");
                });

            modelBuilder.Entity("Repository.Entities.UserWorkoutPlan", b =>
                {
                    b.Navigation("WorkoutPlanVideos");
                });
#pragma warning restore 612, 618
        }
    }
}
