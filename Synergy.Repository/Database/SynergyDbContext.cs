using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using Synergy.Repository.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Synergy.Repository.Database
{
    public  class SynergyDbContext : DbContext, IDbContext
    {
        public static Logger DbSqllog = LogManager.GetLogger("databaseSqlLogger");
        public SynergyDbContext(DbContextOptions<SynergyDbContext> options) : base(options)
        {
            //Database.Migrate();
        }

        public SynergyDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var configuration = builder.Build();
               optionsBuilder.UseSqlServer(configuration.GetConnectionString("SynergyDbConnection"));


            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typeRigister = Assembly.Load("Synergy.Repository").GetTypes()
                .Where(t => t.GetInterfaces()
                .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))).ToList();


            foreach (var type in typeRigister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
