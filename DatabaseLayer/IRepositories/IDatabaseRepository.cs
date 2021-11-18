using System.Collections.Generic;
using DatabaseLayer.Models;

namespace DatabaseLayer.IRepositories
{
    public interface IDatabaseRepository
    {
        Database FindDatabaseByName(string name);
        List<string> GetAllDatabasesNames();
        void AddDatabase(string name);
        bool RemoveDatabase(string databaseName);
        bool DatabaseExists(string databaseName);
        void SaveDatabase(string name);
        int GetDatabase(string filename);
    }
}