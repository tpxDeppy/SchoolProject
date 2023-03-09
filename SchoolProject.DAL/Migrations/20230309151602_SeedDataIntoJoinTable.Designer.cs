﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolProject.DAL;

#nullable disable

namespace SchoolProject.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230309151602_SeedDataIntoJoinTable")]
    partial class SeedDataIntoJoinTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SchoolProject.BL.Models.Class", b =>
                {
                    b.Property<Guid>("Class_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Class_description")
                        .IsRequired()
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Class_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Class_ID");

                    b.ToTable("Class");

                    b.HasData(
                        new
                        {
                            Class_ID = new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            Class_description = "How to act",
                            Class_name = "Acting"
                        },
                        new
                        {
                            Class_ID = new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                            Class_description = "How to write movie scripts",
                            Class_name = "Scripting"
                        },
                        new
                        {
                            Class_ID = new Guid("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"),
                            Class_description = "How to sing",
                            Class_name = "Singing"
                        },
                        new
                        {
                            Class_ID = new Guid("9f082281-2925-4261-a339-be2f4db65271"),
                            Class_description = "How to dance",
                            Class_name = "Dancing"
                        });
                });

            modelBuilder.Entity("SchoolProject.BL.Models.Person", b =>
                {
                    b.Property<Guid>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime?>("Date_of_birth")
                        .HasColumnType("date");

                    b.Property<string>("First_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Last_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)");

                    b.Property<Guid>("School_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("User_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year_group")
                        .HasColumnType("int");

                    b.HasKey("User_ID");

                    b.HasIndex("School_ID");

                    b.ToTable("Person");

                    b.HasData(
                        new
                        {
                            User_ID = new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"),
                            First_name = "Johnny",
                            Last_name = "Depp",
                            School_ID = new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                            User_type = "Teacher"
                        },
                        new
                        {
                            User_ID = new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0"),
                            Date_of_birth = new DateTime(2005, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            First_name = "Angelina",
                            Last_name = "Jolie",
                            School_ID = new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                            User_type = "Pupil",
                            Year_group = 13
                        });
                });

            modelBuilder.Entity("SchoolProject.BL.Models.PersonClass", b =>
                {
                    b.Property<Guid>("Class_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("User_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Class_ID", "User_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("PersonClass");

                    b.HasData(
                        new
                        {
                            Class_ID = new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            User_ID = new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
                        },
                        new
                        {
                            Class_ID = new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                            User_ID = new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
                        },
                        new
                        {
                            Class_ID = new Guid("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"),
                            User_ID = new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                        },
                        new
                        {
                            Class_ID = new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            User_ID = new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                        },
                        new
                        {
                            Class_ID = new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                            User_ID = new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                        });
                });

            modelBuilder.Entity("SchoolProject.BL.Models.School", b =>
                {
                    b.Property<Guid>("School_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("School_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("School_ID");

                    b.ToTable("School");

                    b.HasData(
                        new
                        {
                            School_ID = new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                            School_name = "Hollywood School"
                        });
                });

            modelBuilder.Entity("SchoolProject.BL.Models.Person", b =>
                {
                    b.HasOne("SchoolProject.BL.Models.School", "School")
                        .WithMany("Persons")
                        .HasForeignKey("School_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolProject.BL.Models.PersonClass", b =>
                {
                    b.HasOne("SchoolProject.BL.Models.Class", "Class")
                        .WithMany("PersonClasses")
                        .HasForeignKey("Class_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolProject.BL.Models.Person", "Person")
                        .WithMany("PersonClasses")
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("SchoolProject.BL.Models.Class", b =>
                {
                    b.Navigation("PersonClasses");
                });

            modelBuilder.Entity("SchoolProject.BL.Models.Person", b =>
                {
                    b.Navigation("PersonClasses");
                });

            modelBuilder.Entity("SchoolProject.BL.Models.School", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
