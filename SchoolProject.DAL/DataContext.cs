using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.SqlServer;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;

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

        //configuring relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuring Primary Keys
            modelBuilder.Entity<Person>()
                .HasKey(p => p.User_ID);

            modelBuilder.Entity<School>()
                .HasKey(s => s.School_ID);

            modelBuilder.Entity<Class>()
                .HasKey(c => c.Class_ID);

            //configuring one-to-many relationship
            modelBuilder.Entity<Person>()
                .HasOne(p => p.School)
                .WithMany(s => s.Persons)
                .HasForeignKey(p => p.School_ID);

            //configuring many-to-many relationship
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Persons)
                .WithMany(p => p.Classes)
                .UsingEntity(join => join.ToTable("ClassConnection"));

            //specifying data types for Person
            modelBuilder.Entity<Person>()
                .Property(p => p.User_ID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Person>()
                .Property(p => p.First_name)
                .HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Person>()
                .Property(p => p.Last_name)
                .HasColumnType("nvarchar(80)");

            modelBuilder.Entity<Person>()
                .Property(p => p.User_type)
                .HasConversion(
                    //converting it to string to store it in the DB
                    userType => userType.ToString(),
                    //converting it to UserType Enum 
                    userType => (UserType)Enum.Parse(typeof(UserType), userType));

            modelBuilder.Entity<Person>()
                .Property(p => p.Date_of_birth)
                .HasColumnType("date");

            //specifying data types for School
            modelBuilder.Entity<School>()
                .Property(s => s.School_ID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<School>()
                .Property(s => s.School_name)
                .HasColumnType("nvarchar(50)");

            //specifying data types for Class
            modelBuilder.Entity<Class>()
                .Property(c => c.Class_ID)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Class>()
                .Property(c => c.Class_name)
                .HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Class>()
                .Property(c => c.Class_description)
                .HasColumnType("nvarchar(120)");
        }
    }
}
