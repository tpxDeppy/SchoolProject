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
    public class SchoolConfig : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            //configuring Primary Key
            builder.HasKey(s => s.School_ID);

            //specifying data types
            builder
                .Property(s => s.School_ID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(s => s.School_name)
                .HasColumnType("nvarchar(50)");

            //seed Data
            builder
                .HasData(
                    new School
                    {
                        School_ID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                        School_name = "Hollywood School"
                    }
                );
        }
    }
}
