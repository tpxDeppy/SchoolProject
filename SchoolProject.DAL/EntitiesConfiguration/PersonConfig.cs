using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Data.EntitiesConfiguration
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            //configuring Primary Key
            builder.HasKey(p => p.UserID);

            //configuring one-to-many relationship
            builder
                .HasOne(p => p.School)
                .WithMany(s => s.Persons)
                .HasForeignKey(p => p.SchoolID);

            //configuring relationship between Person and ClassConnection
            builder
                .HasMany(p => p.PersonClasses)
                .WithOne(cp => cp.Person)
                .HasForeignKey(cp => cp.UserID);

            //specifying data types
            builder
                .Property(p => p.UserID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(p => p.FirstName)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(p => p.LastName)
                .HasColumnType("nvarchar(80)");

            builder
                .Property(p => p.UserType)
                .HasConversion(
                    //converting it to string to store it in the DB
                    userType => userType.ToString(),
                    //converting it to UserType Enum 
                    userType => (UserType)Enum.Parse(typeof(UserType), userType));

            builder
                .Property(p => p.DateOfBirth)
                .HasColumnType("date");

            //seed Data
            builder
                .HasData(
                    new Person
                    {
                        UserID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"),
                        FirstName = "Johnny",
                        LastName = "Depp",
                        UserType = UserType.Teacher,
                        DateOfBirth = null,
                        YearGroup = null,
                        SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                    },
                    new Person
                    {
                        UserID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0"),
                        FirstName = "Angelina",
                        LastName = "Jolie",
                        UserType = UserType.Pupil,
                        DateOfBirth = new DateTime(2005, 1, 9),
                        YearGroup = 13,
                        SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                    }
                );
        }
    }
}
