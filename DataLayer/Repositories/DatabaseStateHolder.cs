using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DataLayer.Models;
using WinFormsApp.Repositories; //using System.Windows.Forms;

namespace DataLayer.Repositories
{
    public class DatabaseStateHolder
    {
        private readonly ValidateRepository _validateRepository;
        public readonly DatabaseRepository _databaseRepository;
        private readonly TableRepository _tableRepository;
        private readonly ColumnRepository _columnRepository;
         public List<Database> DatabasesList { get; set; }

        //private readonly DatabaseInitializer _databaseInitializer;

        public DatabaseStateHolder(DatabaseInitializer databaseInitializer)
        {
            //DatabasesList = new List<Database>();
            _validateRepository = new ValidateRepository();

           // _databaseInitializer = databaseInitializer;
            //InitDatabases();
            // _databaseRepository = new DatabaseRepository(DatabasesList);
            // _tableRepository = new TableRepository(_databaseRepository);
            // _columnRepository = new ColumnRepository(_tableRepository);
        }

        // public void InitDatabases()
        // {
        //     var valuesDict = new Dictionary<int, string>();
        //     var valuesDict2 = new Dictionary<int, string>();
        //     for (int i = 0; i < 5; i++)
        //     {
        //         valuesDict.Add(i, "Value" + i);
        //         if (i < 3)
        //             valuesDict2.Add(i, "Value" + i);
        //     }
        //
        //     var valuesList = new List<string>();
        //     var valuesList2 = new List<string>();
        //     for (int i = 0; i < 5; i++)
        //     {
        //         valuesList.Add("Value" + i);
        //         if (i < 3)
        //             valuesList2.Add("Value" + i);
        //     }
        //
        //     var cols = new List<Column>
        //     {
        //         new Column {Name = "Column1", Type = Type.String, Values = valuesList},
        //         new Column {Name = "Column2", Type = Type.Integer, Values = valuesList2}
        //     };
        //     var tablesDatabase1 = new List<Table>
        //     {
        //         new() {Name = "TableTest1", Columns = cols},
        //         new() {Name = "TableTest2"}
        //     };
        //     var tablesDatabase2 = new List<Table>
        //     {
        //         new() {Name = "TableTest1Db2"}, //Columns = cols},
        //         new() {Name = "TableTest2Db2"}
        //     };
        //
        //     DatabasesList = new List<Database>
        //     {
        //         new() {Name = "Database_Test2", Tables = tablesDatabase2},
        //         new() {Name = "Database_Test1", Tables = tablesDatabase1}
        //     };
        // }

        public Database FindDatabaseByName(string name)
        {
            return DatabasesList.First(d => d.Name == name);
        }
        
        public List<string> GetAllDatabasesNames()
        {
            var databasesNames = DatabasesList.Select(database => database.Name).ToList();
            return databasesNames;
        }
        
        public bool AddDatabase(string name)
        {
            var database = new Database {Name = name};
            DatabasesList.Add(database);
            return true;
        }
        
        public void RemoveDatabase(string databaseName)
        {
            var database = FindDatabaseByName(databaseName);
            DatabasesList.Remove(database);
        }
        
        /*Tables actions*/
        public Table FindTableByName(string databaseName, string tableName)
        {
            var database = FindDatabaseByName(databaseName);
            var table = database.Tables.Find(t => t.Name == tableName);
            return table;
        }
        
        public List<string> GetAllDatabaseTablesNames(string databaseName)
        {
            var database = FindDatabaseByName(databaseName);
            var tablesNames = database.Tables.Select(table => table.Name).ToList();
            return tablesNames;
        }
        
        public void AddTable(string databaseName, string tableName)
        {
            var database = FindDatabaseByName(databaseName);
        
            var table = new Table {Name = tableName};
            database.Tables.Add(table);
        }
        
        public void RemoveTable(string databaseName, string tableName)
        {
            var database = FindDatabaseByName(databaseName);
            var table = FindTableByName(databaseName, tableName);
        
            database.Tables.Remove(table);
        }

        /*Columns actions*/
        public Column FindColumnByName(string databaseName, string tableName, string columnName)
        {
            var table = FindTableByName(databaseName, tableName);
            var column = table.Columns.Find(c => c.Name == columnName);
            return column;
        }
        
        public List<Column> FindAllTableColumns(string databaseName, string tableName)
        {
            var table = FindTableByName(databaseName, tableName);
            return table.Columns;
        }
        
        public void AddColumn(string databaseName, string tableName, string columnName, Models.Type columnType)
        {
            var table = FindTableByName(databaseName, tableName);
            var column = new Column {Name = columnName, Type = columnType};
            table.Columns.Add(column);
        }
        
        public void RemoveColumn(string databaseName, string tableName, string columnName)
        {
            var table = FindTableByName(databaseName, tableName);
            var column = FindColumnByName(databaseName, tableName, columnName);
            table.Columns.Remove(column);
        }

        /*Column Data Actions*/
        public bool AddDataToColumn(string databaseName, string tableName, string data, int columnIndex, int rowIndex)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            //var columnIndex = table.Columns.FindIndex(column => column.Name == columnName);

            //var colValues = table.Columns[columnIndex].Values;
            //var column = FindColumnByName(databaseName, tableName, columnName);

            var column = FindColumnByIndex(table, columnIndex);
            var columnData = FindAllDataFromColumn(databaseName, tableName, column.Name);
            // if (rowIndex > column.Values.Count-1)
            // {
            //     column.Values.Insert(rowIndex,data);
            // }
            // column.Values[rowIndex] = data;

            if (!_validateRepository.Validate(column.Type, data))
                return false;
            if (rowIndex > columnData.Count - 1)
            {
                columnData.Insert(rowIndex, data);
            }

            columnData[rowIndex] = data;
            return true;

            //var colValues = table.Columns[e.ColumnIndex].Values;
            // if (e.RowIndex > colValues.Count-1)
            // {
            //     table.Columns[e.ColumnIndex].Values.Insert(e.RowIndex,currentCellData);
            // }
            // table.Columns[e.ColumnIndex].Values[e.RowIndex] = currentCellData;
        }

        public int FindColumnIndexByColumnName(string databaseName, string tableName, string columnName)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var columnIndex = table.Columns.FindIndex(column => column.Name == columnName);
            return columnIndex;
        }

        private Column FindColumnByIndex(Table table, int columnIndex)
        {
            return table.Columns[columnIndex];
        }

        public List<string> FindAllDataFromColumn(string databaseName, string tableName, string columnName)
        {
            //var table = FindTableByName(databaseName, tableName);
            var column = _columnRepository.FindColumnByName(databaseName, tableName, columnName);
            return column.Values;
        }

        /*Table Rows actions*/
        public void InitEmptyRows(string databaseName, string tableName, int count)
        {
            //int count = table.Columns.Select(column => column.Values.Count).Max();
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            foreach (var column in table.Columns.Where(column => column.Values.Count < count))
            {
                for (int i = column.Values.Count; i < count; i++)
                {
                    column.Values.Add(string.Empty);
                }
            }
        }

        public int FindRowsCount(string databaseName, string tableName)
        {
            var table = _tableRepository.FindTableByName(databaseName, tableName);

            var count = FindRowsCount(table);
            // int count = 0;
            // if (table.Columns.Count != 0)
            // {
            //     count = table.Columns.Select(column => column.Values.Count).Max();
            // }
            //
            return count;
        }

        public int FindRowsCount(Table table)
        {
            //var table = FindTableByName(databaseName, tableName);

            int count = 0;
            if (table.Columns.Count != 0)
            {
                count = table.Columns.Select(column => column.Values.Count).Max();
            }

            return count;
        }

        public void AddRowToTable(string databaseName, string tableName)
        {
            var tableColumns = _columnRepository.FindAllTableColumns(databaseName, tableName);
            foreach (var column in tableColumns)
            {
                column.Values.Add(string.Empty);
            }
        }

        public void RemoveRowFromTable(string databaseName, string tableName, int rowIndex)
        {
            var tableColumns = _columnRepository.FindAllTableColumns(databaseName, tableName);

            foreach (var column in tableColumns)
            {
                column.Values.RemoveAt(rowIndex);
            }
        }

        /*Save and Get Database from Disk*/
        public void SaveDatabase(string databaseName)
        {
            Database database = _databaseRepository.FindDatabaseByName(databaseName);
            var filename = @"D:\" + database.Name + ".json";
            var options = new JsonSerializerOptions {WriteIndented = true};
            string jsonString = JsonSerializer.Serialize(database, options);
            File.WriteAllText(filename, jsonString);
        }

        public void GetDatabase(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            var database = JsonSerializer.Deserialize<Database>(jsonString);
            DatabasesList.Add(database);
        }
    }
}