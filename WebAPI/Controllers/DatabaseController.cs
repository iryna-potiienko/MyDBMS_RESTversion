using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController: ControllerBase
    {
        private readonly IDatabaseRepository _databaseRepository;

        public DatabaseController(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        [HttpGet("GetAllDatabasesnames")]
        public IEnumerable<string> Get()
        {
            return _databaseRepository.GetAllDatabasesNames();
        }
        
        [HttpGet("GetDatabaseByName/{name}")]
        public ActionResult<Database> Get(string name)
        {
            var database = _databaseRepository.FindDatabaseByName(name);
            if (database == null)
            {
                return NotFound();
            }
            return database;
        }
        
        [HttpPost("AddDatabase/{name}")]
        public IActionResult Post(string name)
        {
            if (_databaseRepository.DatabaseExists(name))
                return BadRequest();
            _databaseRepository.AddDatabase(name);
            return NoContent();
        }
        
        [HttpDelete("RemoveDatabase/{name}")]
        public IActionResult DeleteDatabase(string name)
        {
            var removed=_databaseRepository.RemoveDatabase(name);
            
            if (!removed) return NotFound();
            return NoContent();
        }
        
        [HttpGet("SaveDatabaseToFile/{name}")]
        public ActionResult<Database> SaveDatabaseToFile(string name)
        {
            if (!_databaseRepository.DatabaseExists(name))
                return NotFound();
            _databaseRepository.SaveDatabase(name);
            return NoContent();
            // var database = _databaseRepository.FindDatabaseByName(name);
            // if (database == null)
            // {
            //     return NotFound();
            // }
            // return database;
        }
        
        [HttpGet("GetDatabaseFromFile/{filename}")]
        public ActionResult<Database> GetDatabaseFromFile(string filename)
        {
            var got = _databaseRepository.GetDatabase(filename);
            return got switch
            {
                -1 => BadRequest("Cannot open file " + filename),
                1 => BadRequest(),
                _ => NoContent()
            };
            // var database = _databaseRepository.FindDatabaseByName(name);
            // if (database == null)
            // {
            //     return NotFound();
            // }
            // return database;
        }
    }
}