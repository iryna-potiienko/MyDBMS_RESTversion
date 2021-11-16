using System.Collections.Generic;
using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class ColumnRepository
    {
        private TableRepository _tableRepository { get; set; }
        public ColumnRepository(TableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }
        /*Columns actions*/
        public Column FindColumnByName(string databaseName, string tableName, string columnName)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var column = table.Columns.Find(c => c.Name == columnName);
            return column;
        }

        public List<Column> FindAllTableColumns(string databaseName, string tableName)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            return table.Columns;
        }
        
        public void AddColumn(string databaseName, string tableName, string columnName, Type columnType)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var column = new Column {Name = columnName, Type = columnType};
            table.Columns.Add(column);
        }

        public void RemoveColumn(string databaseName, string tableName, string columnName)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var column = FindColumnByName(databaseName, tableName, columnName);
            table.Columns.Remove(column);
        }
    }
}