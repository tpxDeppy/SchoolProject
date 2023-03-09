using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.DAL.EntitiesConfiguration
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            //configuring Primary Key
            builder.HasKey(p => p.User_ID);

            //configuring one-to-many relationship
            builder
                .HasOne(p => p.School)
                .WithMany(s => s.Persons)
                .HasForeignKey(p => p.School_ID);

            //configuring relationship between Person and ClassConnection
            builder
                .HasMany(p => p.PersonClasses)
                .WithOne(cp => cp.Person)
                .HasForeignKey(cp => cp.User_ID);

            //specifying data types
            builder
                .Property(p => p.User_ID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(p => p.First_name)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(p => p.Last_name)
                .HasColumnType("nvarchar(80)");

            builder
                .Property(p => p.User_type)
                .HasConversion(
                    //converting it to string to store it in the DB
                    userType => userType.ToString(),
                    //converting it to UserType Enum 
                    userType => (UserType)Enum.Parse(typeof(UserType), userType));

            builder
                .Property(p => p.Date_of_birth)
                .HasColumnType("date");

            //seed Data
            builder
                .HasData(
                    new Person
                    {
                        User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"),
                        First_name = "Johnny",
                        Last_name = "Depp",
                        User_type = UserType.Teacher,
                        Date_of_birth = null,
                        Year_group = null,
                        School_ID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                    },
                    new Person
                    {
                        User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0"),
                        First_name = "Angelina",
                        Last_name = "Jolie",
                        User_type = UserType.Pupil,
                        Date_of_birth = new DateTime(2005, 1, 9),
                        Year_group = 13,
                        School_ID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                    }
                );
        }
    }
}
