﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolProject.Data;

#nullable disable

namespace SchoolProject.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230307200905_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClassPerson", b =>
                {
                    b.Property<Guid>("ClassesClass_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonsUser_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClassesClass_ID", "PersonsUser_ID");

                    b.HasIndex("PersonsUser_ID");

                    b.ToTable("ClassConnection", (string)null);
                });

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
                });

            modelBuilder.Entity("SchoolProject.BL.Models.Person", b =>
                {
                    b.Property<Guid>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Date_of_birth")
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

                    b.Property<int>("Year_group")
                        .HasColumnType("int");

                    b.HasKey("User_ID");

                    b.HasIndex("School_ID");

                    b.ToTable("Person");
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
                });

            modelBuilder.Entity("ClassPerson", b =>
                {
                    b.HasOne("SchoolProject.BL.Models.Class", null)
                        .WithMany()
                        .HasForeignKey("ClassesClass_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolProject.BL.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("PersonsUser_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("SchoolProject.BL.Models.School", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
