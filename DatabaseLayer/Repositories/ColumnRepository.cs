using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class ColumnRepository
    {
        private readonly MyDBMSContext _context;
        private TableRepository _tableRepository { get; set; }
        
        public ColumnRepository(MyDBMSContext myDbmsContext, TableRepository tableRepository)
        {
            _context = myDbmsContext;
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
            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
                return null;
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            return table.Columns;
        }
        
        public bool AddColumn(string databaseName, string tableName, string columnName, Type columnType)
        {
            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
                return false;
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            //if (table == null) return false;
            
            var column = new Column {Name = columnName, Type = columnType, TableId = table.Id};
            _context.Columns.Add(column);
            _context.SaveChanges();
            
            for (var i = 1; i <= table.RowsNumber; i++)
            {
                _context.DataValues.Add(new DataValue {ColumnId = column.Id, RowNumber = i});
            }

            _context.SaveChanges();
            return true;
        }

        public void RemoveColumn(string databaseName, string tableName, string columnName)
        {
            //var table = _tableRepository.FindTableByName(databaseName, tableName);
            var column = FindColumnByName(databaseName, tableName, columnName);
            //table.Columns.Remove(column);
            
            _context.Columns.Remove(column);
            _context.SaveChanges();
        }

        public bool ColumnExistsInTable(string databaseName, string tableName, string columnName)
        {
            var tableExists = _tableRepository.TableExistsInDatabase(databaseName, tableName);
            if (!tableExists) return false;

            var table = _tableRepository.FindTableByName(databaseName, tableName);
            return _context.Columns
                .Where(t => t.Name == columnName)
                .Any(t => t.TableId == table.Id);
        }

        public List<string> FindAllTableColumnsNames(string databaseName, string tableName)
        {
            var columns = FindAllTableColumns(databaseName, tableName);
            if (columns == null) return null;
            var columnNames = columns.Select(c => c.Name).ToList();
            return columnNames;
        }
    }
}