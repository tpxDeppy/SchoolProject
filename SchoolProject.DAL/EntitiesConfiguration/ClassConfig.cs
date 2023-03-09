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
    public class ClassConfig : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            //configuring Primary Key
            builder.HasKey(c => c.Class_ID);

            //specifying data types
            builder
                .Property(c => c.Class_ID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(c => c.Class_name)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(c => c.Class_description)
                .HasColumnType("nvarchar(120)");

            //configuring relationship between Class and ClassConnection
            builder
                .HasMany(c => c.PersonClasses)
                .WithOne(cc => cc.Class)
                .HasForeignKey(cc => cc.Class_ID);

            //builder
            //    .HasMany(c => c.Persons)
            //    .WithMany(p => p.Classes)
            //    .UsingEntity(join => join.ToTable("ClassConnection"));
            //.HasData(
            //    new
            //    {
            //        Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
            //        User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
            //    },
            //    new
            //    {
            //        Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
            //        User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
            //    },
            //    new
            //    {
            //        Class_ID = Guid.Parse("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"),
            //        User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")
            //    },
            //    new
            //    {
            //        Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
            //        User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")

            //    },
            //    new
            //    {
            //        Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
            //        User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")
            //    }
            //);

            //seed Data
            builder
                .HasData(
                    new Class
                    {
                        Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                        Class_name = "Acting",
                        Class_description = "How to act",
                    },
                    new Class
                    {
                        Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                        Class_name = "Scripting",
                        Class_description = "How to write movie scripts",
                    },
                    new Class
                    {
                        Class_ID = Guid.Parse("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"),
                        Class_name = "Singing",
                        Class_description = "How to sing",
                    },
                    new Class
                    {
                        Class_ID = Guid.Parse("9f082281-2925-4261-a339-be2f4db65271"),
                        Class_name = "Dancing",
                        Class_description = "How to dance",
                    }
                );
        }
    }
}
