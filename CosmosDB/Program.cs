using Microsoft.Azure.Cosmos;

string cosmosEndpointUri = "https://qdapp.documents.azure.com:443/";
string cosmosDBKey = "UirRsYNaZmZ0zDaZhJ557omccnaU1pZRHrx0s3hPAsRLyZkXb5BFfwP4b22jWQWR3FTCkzEyuE7dACDbLJB73g==";

//await CreateDatabase("qddb");
await CreateContainer("qddb","Orders","/category");


async Task CreateDatabase(string dbName)
{
    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    await cosmosClient.CreateDatabaseAsync(dbName);
    Console.WriteLine("Database created");
}


async Task CreateContainer(string databaseName,string containerName,string partitionKey)
{
    CosmosClient cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosDBKey);

    Database database=cosmosClient.GetDatabase(databaseName);

    await database.CreateContainerAsync(containerName, partitionKey);
    Console.WriteLine("Container created");

}