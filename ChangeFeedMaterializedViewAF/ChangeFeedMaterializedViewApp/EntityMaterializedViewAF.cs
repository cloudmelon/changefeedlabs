using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ChangeFeedMaterializedViewAF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ChangeFeedMaterializedViewApp
{
    public class EntityMaterializedViewAF
    {
        [FunctionName("EntityMaterializedViewAF")]
        public async void Run([CosmosDBTrigger(
            databaseName: "movies",
            collectionName: "feedraw",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "feedleases")]IReadOnlyList<Document> documents, ILogger log)
        {
            if (documents != null && documents.Count > 0)
            {
                log.LogInformation("Documents modified " + documents.Count);
                log.LogInformation("Modified document Id " + documents[0].Id);

               using (DocumentClient client = new DocumentClient(new Uri(Environment.GetEnvironmentVariable($"endpoint")), Environment.GetEnvironmentVariable($"primary_key")))
               {
                    foreach (var doc in documents)
                    {
                        var entity = Entity.FromDocument(doc);

                        var tasks = new List<Task>();

                        tasks.Add(UpdateMaterializedView(client, entity, Environment.GetEnvironmentVariable($"database_name"), Environment.GetEnvironmentVariable($"lease_collection")));

                        await Task.WhenAll(tasks);

                 }

                }

            }
      
        }

        static async Task UpdateMaterializedView(DocumentClient client, Entity entity, string databaseName, string collectionName)
        {
            int attempts = 0;

            while (attempts < 3)
            {
                try
                {
                    var result = await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                      new EntityMaterializedView
                      {
                          Id = entity.Id,
                          UserId = entity.UserId,
                          ItemId = entity.ItemId,
                          ContentId = entity.ContentId,
                          EventType = entity.EventType,
                          OrderId = entity.OrderId,
                          Region = entity.Region,
                          SessionId = entity.SessionId,
                          PartitionId = entity.PartitionId,
                          Timestamp = entity.Timestamp,
                          NewVal = "whatever"
                      });

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
