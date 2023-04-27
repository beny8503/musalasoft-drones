using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Drones.Models;

namespace Drones.Context
{
    public class DronesContext: DbContext
    {
        public DronesContext(DbContextOptions options): base(options)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "drones.db");
        }

        public string DbPath { get; }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<Drone> Drones { get; set; }
        public DbSet<Medication> Medications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drone>(entity =>
            {
                entity.HasIndex(e => e.SN).IsUnique();
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
            });
        }
    }
}
