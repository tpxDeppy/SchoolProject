using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.EntitiesConfiguration;
using SchoolProject.Models.Entities;

namespace SchoolProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //the name of the DbSet will be the name of the corresponding database table
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<PersonClass> PersonClass { get; set; }

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
