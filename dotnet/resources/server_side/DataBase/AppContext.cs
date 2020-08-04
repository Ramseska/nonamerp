using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_side.DataBase.Models;

namespace server_side.DataBase
{
    class AppContext : DbContext
    {
        public DbSet<PlayerModel> Accounts { get; set; }
        public DbSet<HouseModel> House { get; set; }
        public DbSet<ItemModel> Items { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"server={DBConfig.Host};UserId={DBConfig.User};Password={DBConfig.Password};database={DBConfig.DBName};");
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new MyLoggerProvider());
        });
    }
}
