using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColumnDataController: ControllerBase
    {
        private readonly ColumnDataRepository _columnDataRepository;
        private readonly ColumnRepository _columnRepository;

        public ColumnDataController(ColumnDataRepository columnDataRepository, ColumnRepository columnRepository)
        {
            _columnDataRepository = columnDataRepository;
            _columnRepository = columnRepository;
        }
        
        [HttpPost("ChangeDataInColumn")]
        public IActionResult Post([Required] string databaseName, [Required] string tableName,
            [Required] string columnName, string data, [Required] int rowNumber)
        {
            if (!_columnRepository.ColumnExistsInTable(databaseName, tableName, columnName))
                return NotFound();
            var changed = _columnDataRepository.ChangeDataInColumn(databaseName, tableName, columnName, rowNumber, data);
            if (!changed) return BadRequest();
            return NoContent();
        }
        
        [HttpGet("GetRow")]
        public ActionResult<List<DataValue>> GetRow([Required] string databaseName, [Required] string tableName, [Required] int rowNumber)
        {
            var row = _columnDataRepository.FindRowInTable(databaseName, tableName, rowNumber);
            if (row == null) return NotFound();
            if (row.TrueForAll(v => v == null)) return BadRequest();
            
            return row;
        }
        
        [HttpPost("AddRow")]
        public IActionResult PostRow([Required] string databaseName, [Required] string tableName)
        {
            var added = _columnDataRepository.AddRowToTable(databaseName, tableName);
            return added switch
            {
                -1 => NotFound(),
                1 => BadRequest(),
                0 => NoContent(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        [HttpDelete("ClearRow")]
        public IActionResult DeleteRow([Required] string databaseName, [Required] string tableName,
            [Required] int rowNumber)
        {
            var removed = _columnDataRepository.RemoveRowFromTable(databaseName, tableName, rowNumber);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}