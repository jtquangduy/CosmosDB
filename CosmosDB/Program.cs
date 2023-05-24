using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosEndpointUri = "https://qdapp.documents.azure.com:443/";
string cosmosDBKey = "UirRsYNaZmZ0zDaZhJ557omccnaU1pZRHrx0s3hPAsRLyZkXb5BFfwP4b22jWQWR3FTCkzEyuE7dACDbLJB73g==";
string databaseName = "qddb";
string containerName = "Orders";
string containerCustomer = "Customers";

//await CreateDatabase("qddb");
//await CreateContainer("qddb","Orders","/category");
await CreateContainer(databaseName, containerCustomer, "/customerName");

//async Task CreateDatabase(string dbName)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    await cosmosClient.CreateDatabaseAsync(dbName);
//    Console.WriteLine("Database created");
//}

async Task CreateContainer(string databaseName, string containerName, string partitionKey)
{
    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);

    await database.CreateContainerAsync(containerName, partitionKey);
    Console.WriteLine("Container created");

}

//await AddItem("O1", "Laptop", 100);
//await AddItem("O2", "Mobiles", 200);
//await AddItem("O3", "Desktop", 75);
//await AddItem("04", "Laptop", 25);

//async Task AddItem(string orderId, string category, int quantity)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerName);

//    Order order = new Order()
//    {
//        id = Guid.NewGuid().ToString(),
//        orderId = orderId,
//        category = category,
//        quantity = quantity
//    };

//    ItemResponse<Order> response = await container.CreateItemAsync<Order>(order, new PartitionKey(category));

//    Console.WriteLine("Added item with Order Id {0}", order.orderId);
//    Console.WriteLine("Request Units {0}", response.RequestCharge);
//}

//await ReadItem();

//async Task ReadItem()
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerName);

//    string sqlQuery = "SELECT o.orderId,o.category,o.quantity FROM Orders o";

//    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

//    FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDefinition);

//    while (feedIterator.HasMoreResults)
//    {
//        FeedResponse<Order> orders = await feedIterator.ReadNextAsync();
//        foreach (Order order in orders)
//        {
//            Console.WriteLine("Order Id {0}", order.orderId);
//            Console.WriteLine("Category {0}", order.category);
//            Console.WriteLine("Quantity {0}", order.quantity);
//        }
//    }
//}

//await ReplaceItem("O1");

//async Task ReplaceItem(string orderId)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerName);

//    string sqlQuery = $"SELECT o.id,o.category FROM Orders o WHERE o.orderId='{orderId}'";

//    string id = "";
//    string category = "";

//    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

//    FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDefinition);

//    while (feedIterator.HasMoreResults)
//    {
//        FeedResponse<Order> orders = await feedIterator.ReadNextAsync();
//        foreach (Order order in orders)
//        {
//            id = order.id;
//            category = order.category;
//        }
//    }

//    ItemResponse<Order> response = await container.ReadItemAsync<Order>(id, new PartitionKey(category));

//    var item = response.Resource;
//    item.quantity = 150;

//    await container.ReplaceItemAsync<Order>(item, id, new PartitionKey(category));

//    Console.WriteLine("Item is updated");
//}

//await DeleteItem("O1");

//async Task DeleteItem(string orderId)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerName);

//    string sqlQuery = $"SELECT o.id,o.category FROM Orders o WHERE o.orderId='{orderId}'";

//    string id = "";
//    string category = "";

//    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

//    FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDefinition);

//    while (feedIterator.HasMoreResults)
//    {
//        FeedResponse<Order> orders = await feedIterator.ReadNextAsync();
//        foreach (Order order in orders)
//        {
//            id = order.id;
//            category = order.category;
//        }
//    }

//    ItemResponse<Order> response = await container.DeleteItemAsync<Order>(id, new PartitionKey(category));
//    //var item = response.Resource;

//    //await container.DeleteItemAsync<Order>(id,new PartitionKey(category));

//    Console.WriteLine("Item is deleted");
//}

await AddItem("C1", "CustomeA", "New York", new List<Order>()
{
    new Order()
    {
        orderId="O1",
        category="Laptop",
        quantity=100
    },
    new Order()
    {
        orderId="O2",
        category="Mobile",
        quantity=10
    }
});

await AddItem("C2", "CustomeB", "Chicago", new List<Order>()
{
    new Order()
    {
        orderId="O3",
        category="Laptop",
        quantity=20
    }
});

await AddItem("C3", "CustomeC", "Miami", new List<Order>()
{
    new Order()
    {
        orderId="O4",
        category="Desktop",
        quantity=30
    },
    new Order()
    {
        orderId="O5",
        category="Mobile",
        quantity=40
    }
});

async Task AddItem(string customerId, string customerName, string customerCity, List<Order> orders)
{
    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerCustomer);

    Customer customer = new Customer()
    {
        customerId = customerId,
        customerName = customerName,
        customerCity = customerCity,
        orders = orders
    };

    await container.CreateItemAsync<Customer>(customer, new PartitionKey(customerName));

    Console.WriteLine("Added Customer with id: {0}", customerId);
}