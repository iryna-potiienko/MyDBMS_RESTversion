using System.Collections.Generic;
using DatabaseLayer.Models;

namespace DatabaseLayer.IRepositories
{
    public interface IColumnRepository
    {
        List<Column> FindAllTableColumns(string databaseName, string tableName);
        List<string> FindAllTableColumnsNames(string databaseName, string tableName);
        bool ColumnExistsInTable(string databaseName, string tableName, string columnName);
        Column FindColumnByName(string databaseName, string tableName, string columnName);

        bool AddColumn(string databaseName, string tableName, string columnName, Type columnType);
        void RemoveColumn(string databaseName, string tableName, string columnName);
    }
}