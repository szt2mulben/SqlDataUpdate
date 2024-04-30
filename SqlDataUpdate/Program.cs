using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SqlDataUpdate
{

    public class FirstDatabaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Data { get; set; }
    }

    public class SecondDatabaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Data { get; set; }
    }

    public class FirstDbContext : DbContext
    {
        public DbSet<FirstDatabaseEntity> FirstDatabaseEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost;user id=root;password=;database=flyhighdb");
        }
    }

    public class SecondDbContext : DbContext
    {
        public DbSet<SecondDatabaseEntity> SecondDatabaseEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost;user id=root;password=;database=flyhighdb2");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var firstDbContext = new FirstDbContext())
            {
                var entitiesFromFirstDb = firstDbContext.FirstDatabaseEntities.ToList();

                using (var secondDbContext = new SecondDbContext())
                {
                    foreach (var entity in entitiesFromFirstDb)
                    {
                        secondDbContext.SecondDatabaseEntities.Add(new SecondDatabaseEntity { Data = entity.Data });
                    }

                    secondDbContext.SaveChanges();
                }
            }

            using (var secondDbContext = new SecondDbContext())
            {
                var entitiesFromSecondDb = secondDbContext.SecondDatabaseEntities.ToList();

                using (var firstDbContext = new FirstDbContext())
                {
                    foreach (var entity in entitiesFromSecondDb)
                    {
                        firstDbContext.FirstDatabaseEntities.Add(new FirstDatabaseEntity { Data = entity.Data });
                    }

                    firstDbContext.SaveChanges();
                }
            }

            Console.WriteLine("Adatbázisok szinkronizálva.");
        }
    }
}
