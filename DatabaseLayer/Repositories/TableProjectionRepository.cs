using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class TableProjectionRepository : ITableProjectionRepository
    {
        public List<List<string>> FindTableProjection(Table table, List<string> columnsNamesList)
        {
            var finalTable = MakeFinalTable(table, columnsNamesList);

            var tableList = MakeListList(finalTable);
            var tableProjection = new List<List<string>>();

            foreach (var row in tableList)
            {
                if (tableProjection.Any(o => o.SequenceEqual(row)))
                {
                    continue;
                }

                tableProjection.Add(row);
            }

            return tableProjection;
        }

        private List<List<string>> MakeListList(Table table)
        {
            var tableListList = new List<List<string>>();

            var rowCount = table.RowsNumber;
            for (var rowIndex = 1; rowIndex <= rowCount; rowIndex++)
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

        private static Table MakeFinalTable(Table table, ICollection<string> columnNames)
        {
            var finalTable1 = new Table{RowsNumber = table.RowsNumber};
            foreach (var column in table.Columns.Where(column1 => columnNames.Contains(column1.Name)))
            {
                finalTable1.Columns.Add(column);
            }

            return finalTable1;
        }
    }
}