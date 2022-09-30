using HealthyMink.Class;
using Microsoft.EntityFrameworkCore;

namespace HealthyMink.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Shift>? Shifts { get; set; }
        public DbSet<Penalty>? Penalties { get; set; }

        #region Подключение SQL Lite
        public string DbPath { get; }
        public DataBaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "DB.db");
            Database.EnsureCreated();
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee[]
                {
                    new Employee(1, "Иванов","Иван","Иванович",Class.Enum.JobTitle.Manager,null),
                    new Employee(2, "Петров","Петр","Петрович",Class.Enum.JobTitle.Engineer,null),
                    new Employee(3, "Сидоров","Алексей","Михайлович",Class.Enum.JobTitle.Tester,null)
                });
        }
    }
}
