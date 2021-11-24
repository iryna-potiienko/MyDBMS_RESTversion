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

        [HttpGet]
        public IActionResult InsertValuesForTableProjection()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult InsertValuesForTableProjection(TableProjectionViewModel viewModel)
        {
            //if (id == null) return RedirectToAction("ShowTableProjection");
            ViewBag.TableName = viewModel.TableName;
            if (!_tableRepository.TableExistsInDatabase(viewModel.DatabaseName, viewModel.TableName))
                return RedirectToAction(nameof(TableNotExist), viewModel);
            return RedirectToAction(nameof(ShowTableProjection), viewModel);
        }

        [HttpPost]
        public IActionResult ShowTableProjection(TableProjectionViewModel viewModel)
            //(string databaseName, string tableName, List<string> columnsNames)
        {
            var databaseName = viewModel.DatabaseName;
            var tableName = viewModel.TableName;
            var columnsNames = viewModel.ColumnsNames.Split(", ").ToList();
            
            ViewBag.DatabaseName = viewModel.DatabaseName;
            ViewBag.TableName = viewModel.TableName;
            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
                return RedirectToAction(nameof(TableNotExist), viewModel);
            
            ViewBag.ColumnsNames = columnsNames;

            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var projection = _tableProjectionRepository.FindTableProjection(table, columnsNames);
            return View(projection);
        }

        [HttpGet]
        public ViewResult TableNotExist(TableProjectionViewModel viewModel)
            //(string databaseName, string tableName, List<string> columnsNames)
        {
            ViewBag.DatabaseName = viewModel.DatabaseName;
            ViewBag.TableName = viewModel.TableName;
            return View();
        }
    }
}