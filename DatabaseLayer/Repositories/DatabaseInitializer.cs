using System.Collections.Generic;
using DatabaseLayer.Models;

namespace DataLayer.Repositories
{
    public class DatabaseInitializer
    {
        public List<Database> DatabasesList { get; set; }

        public DatabaseInitializer()
        {
            DatabasesList = InitDatabases();
        }
        
        public List<Database> InitDatabases()
        {
            var valuesDict = new Dictionary<int, string>();
            var valuesDict2 = new Dictionary<int, string>();
            for (int i = 0; i < 5; i++)
            {
                valuesDict.Add(i, "Value" + i);
                if (i < 3)
                    valuesDict2.Add(i, "Value" + i);
            }

            var valuesList = new List<string>();
            var valuesList2 = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                valuesList.Add("Value" + i);
                if (i < 3)
                    valuesList2.Add("Value" + i);
            }

            var cols = new List<Column>
            {
                new Column {Name = "Column1", Type = Type.String, Values = valuesList},
                new Column {Name = "Column2", Type = Type.Integer, Values = valuesList2}
            };
            var tablesDatabase1 = new List<Table>
            {
                new() {Name = "TableTest1", Columns = cols},
                new() {Name = "TableTest2"}
            };
            var tablesDatabase2 = new List<Table>
            {
                new() {Name = "TableTest1Db2"}, //Columns = cols},
                new() {Name = "TableTest2Db2"}
            };

            var databasesList = new List<Database>
            {
                new() {Name = "Database_Test2", Tables = tablesDatabase2},
                new() {Name = "Database_Test1", Tables = tablesDatabase1}
            };
            return databasesList;
        }

    }
}