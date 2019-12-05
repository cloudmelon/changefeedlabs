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
            Console.WriteLine("Processing " + documents.Count + " documents updates");
            ChangeFeedAction action = new ChangeFeedAction();
            action.Update(documents);

            return Task.CompletedTask;
        }
    }
}
