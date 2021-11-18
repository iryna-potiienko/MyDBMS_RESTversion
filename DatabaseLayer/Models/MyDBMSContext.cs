using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayer.Models
{
    public class MyDBMSContext: DbContext, IRepository
    {
        public DbSet<Database> Databases { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<DataValue> DataValues { get; set; }
         
        public MyDBMSContext()
        {
            Database.EnsureCreated();
        }
 
        public Task Save()
        {
            return SaveChangesAsync();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1440;Database=MyDBMSDatabase;User=SA;Password=myStrong_password(!);");
            // Data Source=localhost,1440;Initial Catalog=MovieDB;Persist Security Info=True;User ID=SA;Password=vV5r9tn0M4@
            //optionsBuilder.UseSqlServer("Server=IPOTIIENKONB\\SQLEXPRESS;Database=MyDBMSDatabase;Trusted_Connection=True;");
        }
    }
}