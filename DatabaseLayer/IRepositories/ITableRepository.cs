using System.Collections.Generic;
using DatabaseLayer.Models;

namespace DatabaseLayer.IRepositories
{
    public interface ITableRepository
    {
        List<string> GetAllDatabaseTablesNames(string databaseName);

        bool TableExistsInDatabase(string databaseName, string tableName);

        Table FindTableByName(string databaseName, string tableName);

        bool AddTable(string databaseName, string tableName);

        void RemoveTable(string databaseName, string tableName);
    }
}