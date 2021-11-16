using System.Collections.Generic;
using System.Linq;
using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class TablesDifferenceRepository
    {
        private readonly DatabaseStateHolder _databaseStateHolder;

        public TablesDifferenceRepository(DatabaseStateHolder databaseStateHolder)
        {
            _databaseStateHolder = databaseStateHolder;
        }
        
        public List<List<string>> FindTablesDifference(Table table1, Table table2)
        {
            //var table1 = _databaseStateHolder.FindTableByName(database1Name, table1Name);
            //var table2 = _databaseStateHolder.FindTableByName(database2Name, table2Name);

            //var similarColumns = new List<Column>();
            // var similarColumnNames =
            //     FindSimilarColumnNames(database1Name, table1Name, database2Name, table2Name); //new List<string>();

            var similarColumnNames =
                     FindSimilarColumnNames(table1,table2); //new List<string>();


            var finalTable1 = MakeFinalTable(table1, similarColumnNames);
            var finalTable2 = MakeFinalTable(table2, similarColumnNames);

            var table1List = MakeListList(finalTable1);
            var table2List = MakeListList(finalTable2);
            var tableDiff = new List<List<string>>();
            
            foreach (var rowTable1 in table1List)
            {
                foreach (var rowTable2 in table2List)
                {
                    if (tableDiff.Contains(rowTable1))
                    {
                        tableDiff.Remove(rowTable1);
                    }
                    if (rowTable1.SequenceEqual(rowTable2))
                    {
                        // if (tableDiff.Contains(rowTable1))
                        // {
                        //     tableDiff.Remove(rowTable1);
                        // }
                        break;
                    }

                    tableDiff.Add(rowTable1);
                }
            }

            //var res = tableDiff.Distinct();
            //return res as List<List<string>>;
            return tableDiff;
        }

        private List<List<string>> MakeListList(Table table)
        {
            var tableListList = new List<List<string>>();

            var rowCount = _databaseStateHolder.FindRowsCount(table);
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var tableRow = new List<string>();
                foreach (var column in table.Columns)
                {
                    tableRow.Add(column.Values[rowIndex]);
                }

                tableListList.Add(tableRow);
            }

            return tableListList;
        }

        private Table MakeFinalTable(Table table, List<string> similarColumnNames)
        {
            var finalTable1 = new Table();
            foreach (var column in table.Columns.Where(column1 => similarColumnNames.Contains(column1.Name)))
            {
                finalTable1.Columns.Add(column);
            }

            return finalTable1;
        }

        public List<string> FindSimilarColumnNames(Table table1, Table table2)
        {
            //var table1 = _databaseStateHolder.FindTableByName(database1Name, table1Name);
            //var table2 = _databaseStateHolder.FindTableByName(database2Name, table2Name);

            var similarColumnNames = new List<string>();
            foreach (var column1 in table1.Columns)
            {
                // table1Row.Add(column1.Values[rowIndex]);
                foreach (var column2 in table2.Columns)
                {
                    if (column1.Name != column2.Name) continue;
                    if (column1.Type != column2.Type) continue;
                    similarColumnNames.Add(column1.Name);

                }
            }

            return similarColumnNames;
        }
    }
}