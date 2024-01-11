//using BulkyWeb.Models;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;

//namespace BulkyWeb.Data
namespace Bulky.DataAccess.Data
{
    /// <summary>
    ///  DbContext
    ///  Press ctrl . while on DbContext
    ///  is a builtin class inside the entity framework core NuGet package
    ///  So our applicationDBcontext now basically immplements or inherits from the DBcontext class
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
                    
        }

        ///create table "Categories will be the table name"
        public DbSet<Category> Categories { get; set; }
        //run this command to nuget tools console "add-migration AddCategoryTableToDb" AddCategoryTableToDb is just the name of migration
        ///then run update-database again
        /// <summary>
        /// then run update-database again
 


        ////Try to seed data on database
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }
        ///then run this add-migration SeedingCategoryTable
        ///then run update-database
    }
}
