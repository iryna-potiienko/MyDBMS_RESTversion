using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TableController: ControllerBase
    {
        private readonly TableRepository _tableRepository;
        private readonly TablesDifferenceRepository _tablesDifferenceRepository;

        public TableController(TableRepository tableRepository, TablesDifferenceRepository tablesDifferenceRepository)
        {
            _tableRepository = tableRepository;
            _tablesDifferenceRepository = tablesDifferenceRepository;
        }
        
        [HttpGet("GetAllTables/{databaseName}")]
        public ActionResult<IEnumerable<string>> Get(string databaseName)
        {
            var databaseTablesNames = _tableRepository.GetAllDatabaseTablesNames(databaseName);
            if (databaseTablesNames == null) return NotFound();
            return databaseTablesNames;
        }
        
        [HttpGet("GetTable")]
        public ActionResult<Table> GetTable([Required]string databaseName, [Required]string tableName)
        {
            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
            {
                return NotFound();
            }
            return _tableRepository.FindTableByName(databaseName,tableName);
        }
        
        [HttpPost("AddTable")]
        public IActionResult Post([Required]string databaseName, [Required]string tableName)
        {
            if (_tableRepository.TableExistsInDatabase(databaseName,tableName))
                return BadRequest();
            var added = _tableRepository.AddTable(databaseName, tableName);
            if (!added) return NotFound();
            
            return NoContent();
        }
        
        [HttpDelete("DeleteTable")]
        public IActionResult DeleteTable([Required]string databaseName, [Required]string tableName)
        {
            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
            {
                return NotFound();
            }
            
            _tableRepository.RemoveTable(databaseName,tableName);
            return NoContent();
        }

        [HttpGet("FindTablesDifference")]
        public ActionResult<List<List<string>>> GetTablesDifference([Required] string database1Name, [Required] string table1Name,
            [Required] string database2Name, [Required] string table2Name)
        {
            if (!_tableRepository.TableExistsInDatabase(database1Name, table1Name))
                return NotFound();
            if (!_tableRepository.TableExistsInDatabase(database2Name, table2Name))
                return NotFound();

            var table1 = _tableRepository.FindTableByName(database1Name, table1Name);
            var table2 = _tableRepository.FindTableByName(database2Name, table2Name);
            return _tablesDifferenceRepository.FindTablesDifference(table1, table2);
        }
    }
}