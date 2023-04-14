using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Models.Entities;

namespace SchoolProject.Data.EntitiesConfiguration
{
    public class SchoolConfig : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            //configuring Primary Key
            builder.HasKey(s => s.SchoolID);

            //specifying data types
            builder
                .Property(s => s.SchoolID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(s => s.SchoolName)
                .HasColumnType("nvarchar(50)");

            //seed Data
            builder
                .HasData(
                    new School
                    {
                        SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                        SchoolName = "Hollywood School"
                    }
                );
        }
    }
}
