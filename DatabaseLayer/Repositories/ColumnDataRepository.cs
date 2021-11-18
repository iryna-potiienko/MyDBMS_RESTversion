using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class ColumnDataRepository
    {
        private readonly TableRepository _tableRepository;
        private readonly ValidateRepository _validateRepository;
        private readonly ColumnRepository _columnRepository;
        //private readonly MyDBMSContext _context;
        private readonly Models.IRepository _context;

        public ColumnDataRepository(Models.IRepository myDbmsContext, TableRepository tableRepository, ColumnRepository columnRepository, ValidateRepository validateRepository)
        {
            _context = myDbmsContext;
            _tableRepository = tableRepository;
            _validateRepository = validateRepository;
            _columnRepository = columnRepository;
        }
        /*Column Data Actions*/
        public bool ChangeDataInColumn(string databaseName, string tableName, string columnName, int rowIndex, string data)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var column = _columnRepository.FindColumnByName(databaseName, tableName, columnName);
            var columnData = FindAllDataFromColumn(databaseName, tableName, columnName);
            var dataValue = columnData.Find(c => c.RowNumber == rowIndex);
            
            //var dataValue = new DataValue {Data = data, ColumnId = column.Id, RowNumber = rowIndex};

            if (!_validateRepository.Validate(column.Type, data))
                return false;
            if (rowIndex > table.RowsNumber) return false;

            if (dataValue != null)
            {
                dataValue.Data = data;
                _context.DataValues.Update(dataValue);
            }

            _context.Save();
            // if (rowIndex > columnData.Count - 1)
            // {
            //     columnData.Add(dataValue); //.Insert(rowIndex, dataValue); //.Add(dataValue);
            // }

            //_context.DataValues.Add(dataValue);
            //columnData[rowIndex] = data;
            return true;
        }

        private List<DataValue> FindAllDataFromColumn(string databaseName, string tableName, string columnName)
        {
            //var table = FindTableByName(databaseName, tableName);
            var column = _columnRepository.FindColumnByName(databaseName, tableName, columnName);
            return column.Values;
        }

        
        /*Table Rows actions*/
        // public void InitEmptyRows(string databaseName, string tableName, int count)
        // {
        //     //int count = table.Columns.Select(column => column.Values.Count).Max();
        //     var table = _tableRepository.FindTableByName(databaseName, tableName);
        //     foreach (var column in table.Columns.Where(column => column.Values.Count < count))
        //     {
        //         for (int i = column.Values.Count; i < count; i++)
        //         {
        //             column.Values.Add(string.Empty);
        //         }
        //     }
        // }
        //
        // public int FindRowsCount(string databaseName, string tableName)
        // {
        //     var table = _tableRepository.FindTableByName(databaseName, tableName);
        //
        //     var count = FindRowsCount(table);
        //     return count;
        // }
        //
        // public int FindRowsCount(Table table)
        // {
        //     //var table = FindTableByName(databaseName, tableName);
        //
        //     int count = 0;
        //     if (table.Columns.Count != 0)
        //     {
        //         count = table.Columns.Select(column => column.Values.Count).Max();
        //     }
        //
        //     return count;
        // }
        
        public int AddRowToTable(string databaseName, string tableName)
        {
            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
                return -1;
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            //if (table == null) return -1;
            var tableColumns = _columnRepository.FindAllTableColumns(databaseName, tableName);
            
            if (tableColumns.Count == 0) return 1;
            foreach (var column in tableColumns)
            {
                //column.Values.Add(string.Empty);
                _context.DataValues.Add(new DataValue {Data = "", ColumnId = column.Id, RowNumber = table.RowsNumber + 1});
            }

            table.RowsNumber += 1;
            _context.Tables.Update(table);
            _context.Save();

            return 0;
        }

        public List<DataValue> FindRowInTable(string databaseName, string tableName, int rowIndex)
        {
            var tableColumns = _columnRepository.FindAllTableColumns(databaseName, tableName);

            if (tableColumns == null) return null;
            var row = tableColumns.Select(c => c.Values.FirstOrDefault(d => d.RowNumber == rowIndex)).ToList();
            return row; 
        }
        public bool RemoveRowFromTable(string databaseName, string tableName, int rowIndex)
        {
            var row = FindRowInTable(databaseName, tableName, rowIndex);
            if (row==null)
            {
                return false;
            }
            foreach (var value in row)
            {
                value.Data = "";
                _context.DataValues.Update(value);
            }
            //_context.DataValues.RemoveRange(row);
            //_context.DataValues.UpdateRange(row);
            _context.Save();
            return true;
        }
    }
}