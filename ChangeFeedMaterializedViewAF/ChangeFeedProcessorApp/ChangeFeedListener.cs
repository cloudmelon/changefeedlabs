using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChangeFeedProcessorApp
{
    class ChangeFeedListener : IChangeFeedObserver
    {
        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            return Task.CompletedTask;
        }

        public Task OpenAsync(IChangeFeedObserverContext context)
        {
            return Task.CompletedTask;
        }

        public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> documents, CancellationToken cancellationToken)
        {
            ChangeFeedAction action = new ChangeFeedAction();
            action.Update(documents);
            Console.WriteLine("Updating " + documents.Count + " documents in lease collection");
            
            return Task.CompletedTask;
        }
    }
}
