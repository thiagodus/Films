using Films.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Films.Contexts
{
    public class SQLiteDbContext : DbContext
    {
        public DbSet<MovieList> MovieList { get; set; }

        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : base(options)
        { }

        public SQLiteDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<MovieList>()
              //.HasData()

        }
    }
}
