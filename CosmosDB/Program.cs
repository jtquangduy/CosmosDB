using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosEndpointUri = "https://qdapp.documents.azure.com:443/";
string cosmosDBKey = "UirRsYNaZmZ0zDaZhJ557omccnaU1pZRHrx0s3hPAsRLyZkXb5BFfwP4b22jWQWR3FTCkzEyuE7dACDbLJB73g==";
string databaseName = "qddb";
string containerName = "Orders";

//await CreateDatabase("qddb");
//await CreateContainer("qddb","Orders","/category");

//async Task CreateDatabase(string dbName)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    await cosmosClient.CreateDatabaseAsync(dbName);
//    Console.WriteLine("Database created");
//}

//async Task CreateContainer(string databaseName,string containerName,string partitionKey)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database=cosmosClient.GetDatabase(databaseName);

//    await database.CreateContainerAsync(containerName, partitionKey);
//    Console.WriteLine("Container created");

//}

await AddItem("O1", "Laptop", 100);
await AddItem("O2", "Mobiles", 200);
await AddItem("O3", "Desktop", 75);
await AddItem("04", "Laptop", 25);

async Task AddItem(string orderId, string category, int quantity)
{
    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    Order order = new Order()
    {
        id = Guid.NewGuid().ToString(),
        orderId = orderId,
        category = category,
        quantity = quantity
    };

    ItemResponse<Order> response = await container.CreateItemAsync<Order>(order, new PartitionKey(category));

    Console.WriteLine("Added item with Order Id {0}", order.orderId);
    Console.WriteLine("Request Units {0}", response.RequestCharge);
}