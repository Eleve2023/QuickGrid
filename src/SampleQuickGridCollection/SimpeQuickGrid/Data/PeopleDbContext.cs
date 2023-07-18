// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Microsoft.EntityFrameworkCore;

namespace SimpeQuickGrid.Data;

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
