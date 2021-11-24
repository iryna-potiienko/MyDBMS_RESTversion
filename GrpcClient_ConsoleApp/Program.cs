using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcService;

namespace GrpcConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // создаем канал для обмена сообщениями с сервером
            // параметр - адрес сервера gRPC
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            
            // создаем клиента
            var client = new TableProjection.TableProjectionClient(channel);
            while (true)
            {
                Console.Write("Input Database name: ");
                var databaseName = Console.ReadLine();
                Console.Write("Input Table name: ");
                var tableName = Console.ReadLine();
                Console.Write("Input Columns names: ");
                var columns = Console.ReadLine();

                List<string> columnsNames = new List<string>();
                if (columns != null)
                {
                    columnsNames = columns.Split(", ").ToList();
                }

                // обмениваемся сообщениями с сервером
                var reply = client.FindTableProjection(new TableProjectionRequest
                {
                    DatabaseName = databaseName,
                    TableName = tableName,
                    ColumnsNames = {columnsNames}
                });

                Console.WriteLine("\nServer response: ");
                if (!reply.ErrorMessage.Equals(string.Empty))
                    Console.WriteLine("Table doesn't exist in database!");
                else
                {
                    var res = reply.ProjectionRows;
                    foreach (var row in res)
                    {
                        Console.WriteLine(row.Row_);
                    }
                }
                Console.WriteLine("Do you want to exit?");
                var response = Console.ReadLine();
                if(response != null && (response.Equals("yes")||response.Equals("YES")||response.Equals("y")||response.Equals("Y")))
                    break;
            }
        }
    }
}