using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CosmosDataWritterAF
{
    public static class CosmosDataWritter
    {
        private static Lazy<DocumentClient> lazyClient = new Lazy<DocumentClient>(InitializeDocumentClient);
        private static DocumentClient client => lazyClient.Value;


        private static DocumentClient InitializeDocumentClient()
        {
            // Perform any initialization here
            var uri = new Uri(Environment.GetEnvironmentVariable($"endpoint"));
            var authKey = Environment.GetEnvironmentVariable($"primary_key");

            return new DocumentClient(uri, authKey);
        }

        [FunctionName("CosmosDataWritterAF")]
        public static async Task Run([EventHubTrigger("melonhubs", Connection = "EventHubConnectionAppSetting")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");

                    Uri collectionLink = UriFactory.CreateDocumentCollectionUri(Environment.GetEnvironmentVariable($"database_name"), Environment.GetEnvironmentVariable($"lwwcollection"));

                    // Art of possible :
                    Entity entity = JsonConvert.DeserializeObject<Entity>(messageBody);
  

                    ConnectionPolicy policy = new ConnectionPolicy
                    {
                        ConnectionMode = ConnectionMode.Direct,
                        ConnectionProtocol = Protocol.Tcp,
                        UseMultipleWriteLocations = true
                    };
                    policy.SetCurrentLocation("North Europe");



                    DocumentCollection lwwCollection = await client.CreateDocumentCollectionIfNotExistsAsync(
                      UriFactory.CreateDatabaseUri(Environment.GetEnvironmentVariable($"database_name")), new DocumentCollection
                      {
                          Id = Environment.GetEnvironmentVariable($"lwwcollection"),
                          ConflictResolutionPolicy = new ConflictResolutionPolicy
                          {
                              Mode = ConflictResolutionMode.LastWriterWins,
                              ConflictResolutionPath = "/id",
                          },
                      });
                    
                    WriteToCosmos(client, entity, collectionLink);

                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }



        static async Task WriteToCosmos(DocumentClient client, Entity entity, Uri collectionLink)
        {
            int attempts = 0;

            while (attempts < 3)
            {
                try
                {
                    var result = await client.UpsertDocumentAsync(collectionLink, entity);

                }
                catch (DocumentClientException de)
                {
                    if (de.StatusCode == HttpStatusCode.BadRequest)
                    {
                        await Task.Delay(de.RetryAfter);
                        attempts += 1;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
