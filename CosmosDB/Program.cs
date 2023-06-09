﻿using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosEndpointUri = "https://jtappaccount.documents.azure.com:443/";
string cosmosDBKey = "4BEh0nnOwf6N8cQHjFs992TT0vmzlSoHQn3AYHMcPBkhu2Roz99GMuIx2wQUL1jiBUSB1UkWPYPdACDbL1x8oA==";
string databaseName = "appdb";
string containerName = "Orders";
string containerCustomer = "Customers";
string leasesContainerName = "leases";

await StartChangeProcessor();

async Task StartChangeProcessor()
{
    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Container container = cosmosClient.GetContainer(databaseName, leasesContainerName);

    ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(databaseName, containerName)
        .GetChangeFeedProcessorBuilder<Order>(processorName: "ManageChanges", onChangesDelegate: ManageChanges)
        .WithInstanceName("appHost")
        .WithLeaseContainer(container)
        .Build();

    Console.WriteLine("Starting the Change Feed Processor");
    await changeFeedProcessor.StartAsync();
    Console.Read();
    await changeFeedProcessor.StopAsync();
}

static async Task ManageChanges(ChangeFeedProcessorContext context, IReadOnlyCollection<Order> itemCollection, CancellationToken cancellationToken)
{
    foreach (Order item in itemCollection)
    {
        Console.WriteLine("Id {0}", item.id);
        Console.WriteLine("Order Id {0}", item.orderId);
        Console.WriteLine("Creation time {0}", item.creationTime);
    }
}

/*
await CreateItems();

async Task CreateItems()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Container container = cosmosClient.GetContainer(databaseName, containerName);

    dynamic orderItem = new
        {
            id = Guid.NewGuid().ToString(),
            orderId = "O5",
            category = "Tivi"
        };

    await container.CreateItemAsync(orderItem, null, new ItemRequestOptions { PreTriggers = new List<string> { "validateItem" } });

    Console.WriteLine("Item has been inserted");
};
*/
/*
await CallStoredProcedure();

async Task CallStoredProcedure()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Container container = cosmosClient.GetContainer(databaseName, containerName);

    dynamic[] orderItems = new dynamic[]
    {
        new {
            id = Guid.NewGuid().ToString(),
            orderId = "01",
            category  = "Laptop",
            quantity  = 100
        },
        new {
            id = Guid.NewGuid().ToString(),
            orderId = "02",
            category  = "Laptop",
            quantity  = 200
        },
        new {
            id = Guid.NewGuid().ToString(),
            orderId = "03",
            category  = "Laptop",
            quantity  = 75
        },
    };

    var result = await container.Scripts.ExecuteStoredProcedureAsync<string>("createItems", new PartitionKey("Laptop"), new[] { orderItems });

    Console.WriteLine(result);
}*/

/*
function createItems(items)
{
    var context = getContext();
    var response = context.getResponse();

    if (!items)
    {
        response.setBody("Error: Items are undefined");
        return;
    }

    var numOfItems = items.length;
    checkLength(numOfItems);

    for (let i = 0; i < numOfItems; i++)
    {
        createItem(items[i]);
    }

    response.setBody("Items added to collection");

    function checkLength(itemLength)
    {
        if (itemLength == 0)
        {
            response.setBody("Error: There are no items to add");
            return;
        }
    }

    function createItem(item)
    {
        var collection = getContext().getCollection();
        var collectionLink = collection.getSelfLink();
        collection.createDocument(collectionLink, item);
    }
}*/

/*
await CallStoredProcedure();

async Task CallStoredProcedure()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Container container = cosmosClient.GetContainer(databaseName, containerName);

    var result = await container.Scripts.ExecuteStoredProcedureAsync<string>("Display",new PartitionKey(""),null);

    Console.WriteLine(result);
}
// In cosmosdb create a stored procedures
function Display()
{
    var context = getContext();
    var response = context.getResponse();

    response.setBody("This is a stored procedure");
}*/

//await CreateDatabase("qddb");
//await CreateContainer("qddb","Orders","/category");
//await CreateContainer(databaseName, containerCustomer, "/customerName");

//async Task CreateDatabase(string dbName)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    await cosmosClient.CreateDatabaseAsync(dbName);
//    Console.WriteLine("Database created");
//}

//async Task CreateContainer(string databaseName, string containerName, string partitionKey)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);

//    await database.CreateContainerAsync(containerName, partitionKey);
//    Console.WriteLine("Container created");

//}

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

//await AddItem("C1", "CustomeA", "New York", new List<Order>()
//{
//    new Order()
//    {
//        orderId="O1",
//        category="Laptop",
//        quantity=100
//    },
//    new Order()
//    {
//        orderId="O2",
//        category="Mobile",
//        quantity=10
//    }
//});

//await AddItem("C2", "CustomeB", "Chicago", new List<Order>()
//{
//    new Order()
//    {
//        orderId="O3",
//        category="Laptop",
//        quantity=20
//    }
//});

//await AddItem("C3", "CustomeC", "Miami", new List<Order>()
//{
//    new Order()
//    {
//        orderId="O4",
//        category="Desktop",
//        quantity=30
//    },
//    new Order()
//    {
//        orderId="O5",
//        category="Mobile",
//        quantity=40
//    }
//});

//async Task AddItem(string customerId, string customerName, string customerCity, List<Order> orders)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerCustomer);

//    Customer customer = new Customer()
//    {
//        customerId = customerId,
//        customerName = customerName,
//        customerCity = customerCity,
//        orders = orders
//    };

//    await container.CreateItemAsync<Customer>(customer, new PartitionKey(customerName));

//    Console.WriteLine("Added Customer with id: {0}", customerId);
//}

//await ReadItem();

//async Task ReadItem()
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerCustomer);

//    string sqlQuery = "SELECT c.id,c.customerName,c.customerCity,c.orders FROM Customers c";

//    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

//    FeedIterator<Customer> feedIterator = container.GetItemQueryIterator<Customer>(queryDefinition);

//    while (feedIterator.HasMoreResults)
//    {
//        FeedResponse<Customer> customers = await feedIterator.ReadNextAsync();
//        foreach (Customer customer in customers)
//        {
//            Console.WriteLine("Customer Id {0}", customer.customerId);
//            Console.WriteLine("Customer Name {0}", customer.customerName);
//            Console.WriteLine("Customer City {0}", customer.customerCity);
//            foreach (Order order in customer.orders)
//            {
//                Console.WriteLine("Order Id {0}", order.orderId);
//                Console.WriteLine("Category {0}", order.category);
//                Console.WriteLine("Quantity {0}", order.quantity);
//            }
//        }
//    }
//}

//await AddItemArrayOfObject("C2", "06", "Desktop", 300);

//async Task AddItemArrayOfObject(string customerId, string orderId, string category, int quantity)
//{
//    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

//    Database database = cosmosClient.GetDatabase(databaseName);
//    Container container = database.GetContainer(containerCustomer);

//    string sqlQuery = $"SELECT c.id,c.customerName FROM Customers c WHERE c.id='{customerId}'";

//    string customerName = "";

//    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

//    FeedIterator<Customer> feedIterator = container.GetItemQueryIterator<Customer>(queryDefinition);

//    while (feedIterator.HasMoreResults)
//    {
//        FeedResponse<Customer> customers = await feedIterator.ReadNextAsync();
//        foreach (Customer customer in customers)
//        {
//            customerName = customer.customerName;
//        }
//    }

//    ItemResponse<Customer> response = await container.ReadItemAsync<Customer>(customerId, new PartitionKey(customerName));

//    var item = response.Resource;
//    item.orders.Add(new Order { orderId = orderId, category = category, quantity = quantity });

//    await container.ReplaceItemAsync<Customer>(item, customerId, new PartitionKey(customerName));

//    Console.WriteLine("Item is updated");
//}