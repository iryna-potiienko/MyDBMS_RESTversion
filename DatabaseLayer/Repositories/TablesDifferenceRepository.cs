using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class TablesDifferenceRepository
    {
        public List<List<string>> FindTablesDifference(Table table1, Table table2)
        {
            var similarColumnNames =
                     FindSimilarColumnNames(table1,table2); 

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
                        break;
                    }

                    tableDiff.Add(rowTable1);
                }
            }

            return tableDiff;
        }

        private List<List<string>> MakeListList(Table table)
        {
            var tableListList = new List<List<string>>();

            var rowCount = table.RowsNumber;//_databaseStateHolder.FindRowsCount(table);
            for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
            {
                var index = rowIndex;
                var tableRow = table.Columns
                    .Select(column => column.Values
                        .First(v => v.RowNumber == index).Data
                    ).ToList();

                if (tableRow.TrueForAll(r => r != ""))
                {
                    tableListList.Add(tableRow);
                }
            }

            return tableListList;
        }

        private static Table MakeFinalTable(Table table, ICollection<string> similarColumnNames)
        {
            var finalTable1 = new Table{RowsNumber = table.RowsNumber};
            foreach (var column in table.Columns.Where(column1 => similarColumnNames.Contains(column1.Name)))
            {
                finalTable1.Columns.Add(column);
            }

            return finalTable1;
        }

        public List<string> FindSimilarColumnNames(Table table1, Table table2)
        {
            return (
                from column1 in table1.Columns 
                from column2 in table2.Columns 
                where column1.Name == column2.Name
                where column1.Type == column2.Type 
                select column1.Name
                ).ToList();
        }
    }
}