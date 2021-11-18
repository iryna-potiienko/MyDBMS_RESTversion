using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using Xunit;

namespace MockTestsProject
{
    public class UnitTest1
    {
        // [Fact]
        // public void Test1()
        // {
        //     var mockRepo = new Mock<IRepository>();
        //     mockRepo.Setup(repo => repo.Databases.Find(It.IsAny<int>()))
        //         .Returns(new Database()
        //         {
        //             Name = "SomeDatabase"
        //         }).Verifiable();
        //     var controller = new DatabaseRepository(mockRepo.Object);
        //
        //     // Act
        //     var result = controller.FindDatabaseByName("SomeDatabase");
        //     Assert.NotNull(result);
        // }

        [Fact]
        public void FindDatabaseTest()
        {
            // Arrange
            var databaseName = "FindDatabase";
            var mock = new Mock<IDatabaseRepository>();
            mock.Setup(repo => repo.FindDatabaseByName(databaseName))
                .Returns(GetTestDatabases().FirstOrDefault(p => p.Name == databaseName));
            var controller = new DatabaseController(mock.Object);
        
            // Act
            var result = controller.Get(databaseName);
        
            // Assert
            var viewResult = Assert.IsType<ActionResult<Database>>(result);
            var model = Assert.IsType<Database>(viewResult.Value);
            Assert.Equal(databaseName, model.Name);
        }
        
        private List<Database> GetTestDatabases()
        {
            var databases = new List<Database>
            {
                new Database {Name = "Database1"},
                new Database {Name = "Database2"},
                new Database {Name = "FindDatabase"}
            };
            return databases;
        }
        
        [Fact]
        public void AddDatabaseTest()
        {
            // Arrange
            var databaseName = "AddDatabase";
            var mockRepo = new Mock<IDatabaseRepository>();

            var controller = new DatabaseController(mockRepo.Object);
            // Act
            var result = controller.Post(databaseName);

            //Assert
            mockRepo.Verify(r => r.AddDatabase(databaseName));
            Assert.NotNull(result);
        }
        
        [Fact]
        public void GetAllTablesNamesTest()
        {
            // Arrange
            var databaseName = "MockDatabase";
            var mock = new Mock<ITableRepository>();
            ITableProjectionRepository proj = new TableProjectionRepository();
            mock.Setup(repo => repo.GetAllDatabaseTablesNames(databaseName)).Returns(GetTestTablesNames());
            var controller = new TableController(mock.Object, new TablesDifferenceRepository(),proj);
 
            // Act
            var result = controller.Get(databaseName);
 
            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var model = Assert.IsAssignableFrom<List<string>>(viewResult.Value);
            Assert.Equal(GetTestTablesNames().Count, model.Count);
        }

        private List<string> GetTestTablesNames()
        {
            var tableNames = new List<string>
            {
                "MockTable1",
                "MockTable2"
            };
            return tableNames;
        }
        
        [Fact]
        public void AddTableTest()
        {
            // Arrange
            var databaseName = "AddDatabase";
            var tableName = "AddTable";
            var mockRepo = new Mock<ITableRepository>();

            var controller = new TableController(mockRepo.Object, new TablesDifferenceRepository(),new TableProjectionRepository());
            // Act
            var result = controller.Post(databaseName, tableName);

            //Assert
            mockRepo.Verify(r => r.AddTable(databaseName,tableName));
            Assert.NotNull(result);
        }
        
        [Fact]
        public void RemoveTableIfExistTest()
        {
            // Arrange
            var databaseName = "AddDatabase";
            var tableName = "AddTable";
            var mockRepo = new Mock<ITableRepository>();
            mockRepo.Setup(repo => repo.TableExistsInDatabase(databaseName,tableName)).Returns(true);

            var controller = new TableController(mockRepo.Object, new TablesDifferenceRepository(),new TableProjectionRepository());
            // Act
            var result = controller.DeleteTable(databaseName, tableName);

            //Assert
            mockRepo.Verify(r => r.RemoveTable(databaseName,tableName));
            Assert.NotNull(result);
        }
        
        [Fact]
        public void RemoveTableIfNotExistTest()
        {
            // Arrange
            var databaseName = "AddDatabase";
            var tableName = "AddTable";
            var mockRepo = new Mock<ITableRepository>();

            var controller = new TableController(mockRepo.Object, new TablesDifferenceRepository(),new TableProjectionRepository());
            
            // Act
            var result = controller.DeleteTable(databaseName, tableName);

            //Assert
            mockRepo.Verify(r => r.TableExistsInDatabase(databaseName,tableName));
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void GetAllColumnsTest()
        {
            // Arrange
            var databaseName = "MockDatabase";
            var tableName = "MockTable";
            
            var mock = new Mock<IColumnRepository>();
            mock.Setup(repo => repo.FindAllTableColumns(databaseName,tableName)).Returns(GetTestColumns());
            var controller = new ColumnController(mock.Object);
 
            // Act
            var result = controller.GetColumns(databaseName,tableName);
 
            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<Column>>>(result);
            var model = Assert.IsAssignableFrom<List<Column>>(viewResult.Value);
            Assert.Equal(GetTestTablesNames().Count, model.Count);
        }

        private List<Column> GetTestColumns()
        {
            var columns = new List<Column>
            {
                new Column {Name = "MockCol1"},
                new Column {Name = "MockCol2"}
            };
            return columns;
        }

        [Fact]
        public void TableProjectionTest()
        {
            // Arrange
            var databaseName = "MockDatabase";
            var tableName = "MockTable";

            var database = CreateTestDatabase();
            var table = database.Tables.First();
            
            var mock = new Mock<ITableProjectionRepository>();
            var mockTable = new Mock<ITableRepository>();
            
            mockTable.Setup(repo => repo.TableExistsInDatabase(databaseName,tableName)).Returns(true);
            mockTable.Setup(repo => repo.FindTableByName(databaseName,tableName)).Returns(table);

            var controller = new TableController(mockTable.Object, new TablesDifferenceRepository(), mock.Object);

            var columnsNames = new List<string>
            {
                "Column1", "Column2"
            };
            
            // Act
            var result = controller.GetTableProjection(databaseName,tableName,columnsNames);
 
            // Assert
            mock.Verify(r => r.FindTableProjection(table,columnsNames));
            Assert.NotNull(result);
            // var viewResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            // var model = Assert.IsAssignableFrom<List<string>>(viewResult.Value);
            // Assert.Equal(GetTestTablesNames().Count, model.Count);
        }

        private Database CreateTestDatabase()
        {
            var database = new Database
            {
                Name = "MockDatabase",
                Tables =
                {
                    new Table
                    {
                        Name = "MockTable", Columns =
                        {
                            new Column
                            {
                                Name = "Column1", Values =
                                {
                                    new DataValue {Data = "val", RowNumber = 1},
                                    new DataValue {Data = "val", RowNumber = 2}
                                }
                            },
                            new Column
                            {
                                Name = "Column2", Values =
                                {
                                    new DataValue {Data = "val", RowNumber = 1},
                                    new DataValue {Data = "val2", RowNumber = 2}
                                }
                            },
                            new Column
                            {
                                Name = "Column3", Values =
                                {
                                    new DataValue {Data = "val1", RowNumber = 1},
                                    new DataValue {Data = "val", RowNumber = 2}
                                }
                            }
                        }
                    }
                }
            };
            return database;
        }
    }
}