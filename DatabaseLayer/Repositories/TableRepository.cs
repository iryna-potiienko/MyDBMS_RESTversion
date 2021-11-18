using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class TableRepository: ITableRepository
    {
        //private readonly MyDBMSContext _context;
        private readonly IRepository _context;
        private readonly DatabaseRepository _databaseRepository;

        public TableRepository(Models.IRepository myDbmsContext, DatabaseRepository databaseRepository)
        {
            _context = myDbmsContext;
            _databaseRepository = databaseRepository;
        }
        /*Tables actions*/
        public Table FindTableByName(string databaseName, string tableName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            var table = database.Tables.Find(t => t.Name == tableName);
            return table;
        }

        public List<string> GetAllDatabaseTablesNames(string databaseName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            var tablesNames = database?.Tables.Select(table => table.Name).ToList();
            
            return tablesNames;
        }
        public bool AddTable(string databaseName, string tableName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            if (database==null)
            {
                return false;
            }
            
            var table = new Table {Name = tableName, DatabaseId = database.Id};
            _context.Tables.Add(table);
            _context.Save();
            return true;
        }

        public void RemoveTable(string databaseName, string tableName)
        {
            var table = FindTableByName(databaseName, tableName);
            _context.Tables.Remove(table);
            _context.Save();
        }

        public bool TableExistsInDatabase(string databaseName, string tableName)
        {
            if (!_databaseRepository.DatabaseExists(databaseName)) return false;
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            return _context.Tables
                .Where(t => t.Name == tableName)
                .Any(t => t.DatabaseId == database.Id);
        }
    }
}