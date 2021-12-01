import grpc

# import the generated classes
import tableProjection_pb2
import tableProjection_pb2_grpc

# open a gRPC channel
channel = grpc.insecure_channel('localhost:5000')

# create a stub (client)
stub = tableProjection_pb2_grpc.TableProjectionStub(channel)

# create a valid request message

databaseName = input("Input database name: ")
tableName = input("Input table name: ")
columns = input("Input columns names: ")

columnsNames = columns.split(", ")

request = tableProjection_pb2.TableProjectionRequest(databaseName=databaseName, tableName=tableName,
                                                     columnsNames=columnsNames)

print("\n-----Table Projection----",
      "\ndatabaseName: ", databaseName,
      "\ntableName: ", tableName,
      "\ncolumnsNames: ", columnsNames)

# make the call
response = stub.FindTableProjection(request)
print("\nWaiting for server response...")

print("\n\nServerResponse:")
if response.ErrorMessage != "":
    print("Table doesn't exist in database!")
else:
    i = 0
    for row in response.projectionRows:
        print("Row", i, row.row)
        i += 1
