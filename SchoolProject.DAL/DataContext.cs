using Microsoft.EntityFrameworkCore;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;
using SchoolProject.DAL.EntitiesConfiguration;

namespace SchoolProject.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //the name of the DbSet will be the name of the corresponding database table
        public DbSet<Person> Person { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<PersonClass> PersonClass { get; set; }

        //configuring relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //apply configuration for Person
            modelBuilder.ApplyConfiguration(new PersonConfig());

            //apply configuration for School
            modelBuilder.ApplyConfiguration(new SchoolConfig());

            //apply configuration for Class
            modelBuilder.ApplyConfiguration(new ClassConfig());

            //apply configuration for PersonClass
            modelBuilder.ApplyConfiguration(new PersonClassConfig());
        }
    }
}
