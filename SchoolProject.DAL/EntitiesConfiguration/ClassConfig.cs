using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Models.Entities;

namespace SchoolProject.Data.EntitiesConfiguration
{
    public class ClassConfig : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            //configuring Primary Key
            builder.HasKey(c => c.ClassID);

            //specifying data types
            builder
                .Property(c => c.ClassID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(c => c.ClassName)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(c => c.ClassDescription)
                .HasColumnType("nvarchar(120)");

            //configuring relationship between Class and ClassConnection
            builder
                .HasMany(c => c.PersonClasses)
                .WithOne(cc => cc.Class)
                .HasForeignKey(cc => cc.ClassID);

            //seed Data
            builder
                .HasData(
                    new Class
                    {
                        ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                        ClassName = "Acting",
                        ClassDescription = "How to act",
                    },
                    new Class
                    {
                        ClassID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                        ClassName = "Scripting",
                        ClassDescription = "How to write movie scripts",
                    },
                    new Class
                    {
                        ClassID = Guid.Parse("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"),
                        ClassName = "Singing",
                        ClassDescription = "How to sing",
                    },
                    new Class
                    {
                        ClassID = Guid.Parse("9f082281-2925-4261-a339-be2f4db65271"),
                        ClassName = "Dancing",
                        ClassDescription = "How to dance",
                    }
                );
        }
    }
}
