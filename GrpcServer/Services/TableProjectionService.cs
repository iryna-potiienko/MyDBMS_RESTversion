using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLayer.IRepositories;
using DatabaseLayer.Repositories;
using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace GrpcService
{
    public class TableProjectionService : TableProjection.TableProjectionBase
    {
        private readonly ILogger<TableProjectionService> _logger;
        private readonly ITableRepository _tableRepository;
        private readonly TableProjectionRepository _tableProjectionRepository;

        public TableProjectionService(ILogger<TableProjectionService> logger, ITableRepository tableRepository, TableProjectionRepository tableProjectionRepository)
        {
            _logger = logger;
            _tableRepository = tableRepository;
            _tableProjectionRepository = tableProjectionRepository;
        }

        public override Task<TableProjectionReply> FindTableProjection(TableProjectionRequest request,
            ServerCallContext context)
        {
            //string errorMessage;
            var databaseName = request.DatabaseName;
            var tableName = request.TableName;

            if (!_tableRepository.TableExistsInDatabase(databaseName, tableName))
            {
                //errorMessage = "Table doesn't exist in database";
                return Task.FromResult(new TableProjectionReply()
                {
                    ProjectionRows = {},
                    ErrorMessage = "Table doesn't exist in database"
                });;
            }

            var columnsNames = request.ColumnsNames.ToList();
            
            var table = _tableRepository.FindTableByName(databaseName, tableName);
            var projection = _tableProjectionRepository.FindTableProjection(table, columnsNames);

            var proj = new RepeatedField<TableProjectionReply.Types.Row>();
            foreach (var r in projection.Select(row => Task.FromResult(new TableProjectionReply.Types.Row
            {
                Row_ = { new RepeatedField<string> {row} }
            })))
            {
                proj.Add(r.Result);
            }

            var rows = Task.FromResult(new TableProjectionReply()
            {
                ProjectionRows = {proj}
            });
            return rows;
        }
    }
}
