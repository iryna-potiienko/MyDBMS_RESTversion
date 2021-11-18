using System.Collections.Generic;
using DatabaseLayer.Models;

namespace DatabaseLayer.IRepositories
{
    public interface ITableProjectionRepository
    {
        List<List<string>> FindTableProjection(Table table, List<string> columnsNames);
    }
}