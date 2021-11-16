using System.Collections.Generic;
using System.Linq;
using DataLayer.Models;
using WinFormsApp.Repositories;

namespace DataLayer.Repositories
{
    public class TableRepository
    {
        private DatabaseRepository _databaseRepository { get; set; }
        public TableRepository(DatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }
        /*Tables actions*/
        public Table FindTableByName(string databaseName,string tableName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            var table = database.Tables.Find(t => t.Name == tableName);
            return table;
        }

        public List<string> GetAllDatabaseTablesNames(string databaseName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            var tablesNames = database.Tables.Select(table => table.Name).ToList();
            return tablesNames;
        }
        public void AddTable(string databaseName, string tableName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            
            var table = new Table {Name = tableName};
            database.Tables.Add(table);
        }

        public void RemoveTable(string databaseName, string tableName)
        {
            var database = _databaseRepository.FindDatabaseByName(databaseName);
            var table = FindTableByName(databaseName, tableName);
            
            database.Tables.Remove(table);
        }
    }
}