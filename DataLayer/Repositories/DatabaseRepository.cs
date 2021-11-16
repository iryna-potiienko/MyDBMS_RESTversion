using System.Collections.Generic;
using System.Linq;
using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class DatabaseRepository
    {
        public List<Database> DatabasesList { get; set; }
        public DatabaseRepository(List<Database> databasesList)
        {
            DatabasesList = databasesList; //new List<Database>();
        }
        public Database FindDatabaseByName(string name)
        {
            return DatabasesList.First(d => d.Name == name);
        }

        public List<string> GetAllDatabasesNames()
        {
            var databasesNames = DatabasesList.Select(database => database.Name).ToList();
            return databasesNames;
        }
        
        public void AddDatabase(string name)
        {
            var database = new Database {Name = name};
            DatabasesList.Add(database);
        }

        public void RemoveDatabase(string databaseName)
        {
            var database = FindDatabaseByName(databaseName);
            DatabasesList.Remove(database);
        }

    }
}