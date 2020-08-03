using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace server_side.DataBase
{
    class AppContext : DbContext
    {
        public DbSet<PlayerModel> Accounts { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"server={DBConfig.Host};UserId={DBConfig.User};Password={DBConfig.Password};database={DBConfig.DBName};");
        }
    }
}
