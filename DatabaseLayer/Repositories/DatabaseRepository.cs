using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayer.Repositories
{
    public class DatabaseRepository: IDatabaseRepository
    {
        //public List<Database> DatabasesList { get; set; }
        
        //private readonly MyDBMSContext _context;
        private readonly IRepository _context;
        
        public DatabaseRepository(IRepository myDbmsContext)
        {
            _context = myDbmsContext;
        }
        public Database FindDatabaseByName(string name)
        {
            return _context.Databases
                .Include(d=>d.Tables)
                .ThenInclude(t=>t.Columns)
                .ThenInclude(c=>c.Values)
                .FirstOrDefault(d => d.Name == name);
        }

        public List<string> GetAllDatabasesNames()
        {
            var databasesNames = _context.Databases.Select(database => database.Name).ToList();
            return databasesNames;
        }
        
        public void AddDatabase(string name)
        {
            var database = new Database {Name = name};
            _context.Databases.Add(database);
            _context.Save();
        }

        public bool RemoveDatabase(string databaseName)
        {
            var database = FindDatabaseByName(databaseName);
            if (database == null)
            {
                return false;
            }
            
            _context.Databases.Remove(database);
            _context.Save();
            return true;
        }

        public bool DatabaseExists(string databaseName)
        {
            return _context.Databases.Any(d => d.Name == databaseName);
        }

        /*Save and Get Database from Disk*/
        public void SaveDatabase(string databaseName)
        {
            Database database = FindDatabaseByName(databaseName);
            var filename = database.Name + ".json";
            var options = new JsonSerializerOptions {WriteIndented = true};
            string jsonString = JsonSerializer.Serialize(database, options);
            File.WriteAllText(filename, jsonString);
        }

        public int GetDatabase(string filePath)
        {
            if (!File.Exists(filePath)) return -1;
            var jsonString = File.ReadAllText(filePath);
            var database = JsonSerializer.Deserialize<Database>(jsonString);
            if (database == null) return -1;
            
            if (DatabaseExists(database.Name))
                return 1;
            
            //DatabasesList.Add(database);
            _context.Databases.Add(database);
            _context.Save();
            return 0;
        }
    }
}