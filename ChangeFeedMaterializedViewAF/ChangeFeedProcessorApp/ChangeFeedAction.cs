using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using System.Net;
using Document = Microsoft.Azure.Documents.Document;

namespace ChangeFeedProcessorApp
{
    public class ChangeFeedAction
    {
        private Entity entity;
        private string databaseName = "movies";
        private string collectionName = "feedleases";
   
        public ChangeFeedAction()
        {
            this.entity = new Entity();
        }

        private static Lazy<DocumentClient> lazyClient = new Lazy<DocumentClient>(InitializeDocumentClient);
        private static DocumentClient client => lazyClient.Value;

        private static DocumentClient InitializeDocumentClient()
        {
            // Perform any initialization here
            var endpointUri = new Uri("your endpoint");
            var primaryKey = "you key";
            return new DocumentClient(endpointUri, primaryKey);
        }

        public async Task Update(IReadOnlyList<Document> documents)
        {
            foreach (var doc in documents)
            {

                var newEntity = JsonConvert.DeserializeObject<Entity>(doc.ToString());

                Uri databaseLink = UriFactory.CreateDatabaseUri(databaseName);
                Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);

                // Update player score

                Uri docDocumentUri = UriFactory.CreateDocumentUri(databaseName, collectionName, newEntity.id);

                try
                {
                    Document playerDoc = await client.ReadDocumentAsync(docDocumentUri, new RequestOptions { PartitionKey = new PartitionKey(newEntity.id) });
          
                    Entity updatedEntity = new Entity
                    {
                        id = entity.id,
                        UserId = entity.UserId,
                        ItemId = entity.ItemId,
                        ContentId = entity.ContentId,
                        EventType = entity.EventType,
                        OrderId = entity.OrderId,
                        Region = entity.Region,
                        SessionId = entity.SessionId,
                        PartitionId = entity.PartitionId,
                        Timestamp = entity.Timestamp
                    };
                    await client.UpsertDocumentAsync(collectionLink, updatedEntity);
                }

                catch (DocumentClientException ex)
                {

                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        Entity bnewEntity = new Entity
                        {
                            UserId = entity.UserId,
                            ItemId = entity.ItemId,
                            ContentId = entity.ContentId,
                            EventType = entity.EventType,
                            OrderId = entity.OrderId,
                            Region = entity.Region,
                            SessionId = entity.SessionId,
                            PartitionId = entity.PartitionId,
                            Timestamp = entity.Timestamp
                        };
                        await client.UpsertDocumentAsync(collectionLink, bnewEntity);
                    }

                }

            }
        }
    }
}
