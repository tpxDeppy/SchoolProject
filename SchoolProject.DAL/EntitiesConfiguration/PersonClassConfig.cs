using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Models.Entities;

namespace SchoolProject.Data.EntitiesConfiguration
{
    public class PersonClassConfig : IEntityTypeConfiguration<PersonClass>
    {
        public void Configure(EntityTypeBuilder<PersonClass> builder)
        {
            //configuring Composite key
            builder.HasKey(cp => new { cp.Class_ID, cp.User_ID });

            //configuring one-to-many relationship between Class and ClassConnection
            builder
                .HasData(
                new
                {
                    Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                    User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
                },
                new
                {
                    Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                    User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
                },
                new
                {
                    Class_ID = Guid.Parse("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"),
                    User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                },
                new
                {
                    Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                    User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")

                },
                new
                {
                    Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                    User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                }
            );
        }
    }
}
