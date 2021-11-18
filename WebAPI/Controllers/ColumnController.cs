using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColumnController: ControllerBase
    {
        private readonly IColumnRepository _columnRepository;

        public ColumnController(IColumnRepository columnRepository)
        {
            _columnRepository = columnRepository;
        }
        
        [HttpGet("GetAllTableColumns")]
        public ActionResult<IEnumerable<Column>> GetColumns([Required]string databaseName, [Required]string tableName)
        {
            var tableColumns = _columnRepository.FindAllTableColumns(databaseName, tableName);
            if (tableColumns == null) return NotFound();
            return tableColumns;
        }
        
        [HttpGet("GetAllTableColumnsNames")]
        public ActionResult<IEnumerable<string>> GetColumnsNames([Required]string databaseName, [Required]string tableName)
        {
            var tableColumns = _columnRepository.FindAllTableColumnsNames(databaseName, tableName);
            if (tableColumns == null) return NotFound();
            return tableColumns;
        }

        [HttpGet("GetColumn")]
        public ActionResult<Column> GetColumn([Required] string databaseName, [Required] string tableName, [Required] string columnName)
        {
            if (!_columnRepository.ColumnExistsInTable(databaseName, tableName, columnName))
                return NotFound();
            return _columnRepository.FindColumnByName(databaseName, tableName, columnName);
        }
        
        [HttpPost("AddColumn")]
        public IActionResult PostColumn([Required] string databaseName, [Required] string tableName,
            [Required] string columnName, Type columnType)
        {
            if (_columnRepository.ColumnExistsInTable(databaseName,tableName,columnName))
                return BadRequest();
            var added =_columnRepository.AddColumn(databaseName,tableName, columnName, columnType);
            if (!added) return NotFound();
            
            return NoContent();
        }
        
        [HttpDelete("DeleteColumn")]
        public IActionResult DeleteColumn([Required] string databaseName, [Required] string tableName,
            [Required] string columnName)
        {
            if (!_columnRepository.ColumnExistsInTable(databaseName, tableName, columnName))
                return NotFound();
            _columnRepository.RemoveColumn(databaseName, tableName, columnName);
            return NoContent();
        }
    }
}