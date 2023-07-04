using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace SimpeQuickGrid.Data
{
    public class PeopleDbContext: DbContext
    {
        public DbSet<Person> People { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SimpleQuickGridData;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
