using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;

namespace MVCWebApp.Controllers
{
    public class TableProjectionController : Controller
    {
        private readonly TableRepository _tableRepository;
        private readonly TableProjectionRepository _tableProjectionRepository;

        public TableProjectionController(TableRepository tableRepository, TableProjectionRepository tableProjectionRepository)
        {
            _tableRepository = tableRepository;
            _tableProjectionRepository = tableProjectionRepository;
        }

        // GET
        // public IActionResult Index()
        // {
        //     if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
        //         return NotFound();
        //
        //     var table = _tableRepository.FindTableByName(databaseName, tableName);
        //     //return _tablesDifferenceRepository.FindTablesDifference(table1, table2);
        //     var projection =  _tableProjectionRepository.FindTableProjection(table, columnsNames);
        //     return PartialView(projection);
        // }
        
        [HttpGet]
        public IActionResult InsertValuesForTableProjection()
        {
            //if (id == null) return RedirectToAction("Index");
            //ViewBag.PhoneId = id;
            return View();
        }
        
        [HttpPost]
        public IActionResult InsertValuesForTableProjection(TableProjectionViewModel viewModel)
        {
            //if (id == null) return RedirectToAction("Index");
            //ViewBag.PhoneId = id;
            return RedirectToAction(nameof(Index), viewModel);
        }

        [HttpPost]
        public ViewResult Index(TableProjectionViewModel viewModel)
            //(string databaseName, string tableName, List<string> columnsNames)
        {
            //if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
            //return NotFound();

            var databaseName = viewModel.DatabaseName;
            var tableName = viewModel.TableName;
            var columnsNames = viewModel.ColumnsNames.Split(";").ToList();
            ViewBag.ColumnsNames = columnsNames;

            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var projection = _tableProjectionRepository.FindTableProjection(table, columnsNames);
            return View(projection);
        }
    }
}