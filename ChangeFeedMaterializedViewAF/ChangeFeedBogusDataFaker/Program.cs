using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace ChangeFeedBogusDataFaker
{
    public class Program
    {
           private static readonly string _endpointUri = "<your Cosmos DB endpoint>";
           private static readonly string _primaryKey = "< of course your Cosmos DB access key >";

        public static async Task Main(string[] args)
          {
            using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
            {
                DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync("movies");

                Database targetDatabase = client.GetDatabase("movies");

                var containerProperties = new ContainerProperties("feedraw", "/id");
                var containerResponse = await targetDatabase.CreateContainerIfNotExistsAsync(containerProperties, 10000);
                var customContainer = containerResponse.Container;
                var eventTypes = new[] { "AddToCart", "details", "bla", "blabla", "haha" };
                var regions = new[] { "France", "UK", "USA" };
          

                var entities = new Bogus.Faker<Entity>()
                    .RuleFor(i => i.id, (fake) => Guid.NewGuid())
                    .RuleFor(i => i.UserId, (fake) => fake.Random.Int(0, 10000))
                    .RuleFor(i => i.ItemId, (fake) => fake.Random.Int(0, 10000))
                    .RuleFor(i => i.ContentId, (fake) => fake.Random.Int(0, 10000))
                    .RuleFor(i => i.EventType, (fake) => fake.PickRandom(eventTypes))
                    .RuleFor(i => i.OrderId, (fake) => fake.Random.Int(0, 10000))
                    .RuleFor(i => i.Region, (fake) => fake.PickRandom(regions))
                    .RuleFor(i => i.SessionId, (fake) => fake.Random.Int(0, 10000).ToString())
                    .RuleFor(i => i.Timestamp, (fake) => fake.Random.Int(0, 10000).ToString())
                    .RuleFor(i => i.PartitionId, (fake) => fake.Random.Int(0, 10000))
                    .GenerateLazy(500);

                foreach (var entity in entities)
                {
                    ItemResponse<Entity> result = await customContainer.CreateItemAsync(entity);

                    await Console.Out.WriteLineAsync($"\tDocument Created\t{result.Resource.id}");
                }



            }
        }
    }
}
