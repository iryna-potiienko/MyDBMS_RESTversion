using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayer.Models
{
    public interface IRepository
    {
        public DbSet<Database> Databases { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<DataValue> DataValues { get; set; }
        
        Task Save();
    }
}