using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Learner.Models;
using Microsoft.EntityFrameworkCore;

namespace Learner.Infrastruction
{
    public class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<Word> Words { get; set; }

        public DbSet<Collection> Collections { get; set; }

        //public DbSet<ManyToMany> CollectionsWords { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken); //writing new data to the DbSet<Word> Words property
            App._words = await Words.ToListAsync(); //updating words list
            App._collections = await Collections.ToListAsync();
            return result; //returning SaveChanges() result
        }
    }
}
