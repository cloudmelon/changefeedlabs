using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.ChangeFeedProcessor;

namespace ChangeFeedProcessorApp
{
    class ChangeFeedProcessor 
    {
        static void Main(string[] args)
        {
            var p = new ChangeFeedProcessor();
            p.Run();
        }

        public void Run()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            DocumentCollectionInfo feedCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "movies",
                CollectionName = "feedraw",
                Uri = new Uri("https://team1-openhack.documents.azure.com:443"),
                MasterKey = "AKhEmDWTnqN5bXU27kmXeBYHLbDb6yzk6fxMkWei3PI7naVjoDVVrIJiChXp63DZAxHrytCPW6KkN3Z6gDzYbA=="
            };

            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "movies",
                CollectionName = "feedleases",
                Uri = new Uri("https://team1-openhack.documents.azure.com:443"),
                MasterKey = "AKhEmDWTnqN5bXU27kmXeBYHLbDb6yzk6fxMkWei3PI7naVjoDVVrIJiChXp63DZAxHrytCPW6KkN3Z6gDzYbA=="
            };

            ChangeFeedProcessorOptions feedProcessorOptions = new ChangeFeedProcessorOptions();

            feedProcessorOptions.LeaseRenewInterval = TimeSpan.FromSeconds(5);
            feedProcessorOptions.StartFromBeginning = false;

            var builder = new ChangeFeedProcessorBuilder();
            var processor = await builder
                .WithHostName("ChangeFeedHost")
                .WithFeedCollection(feedCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithObserver<ChangeFeedListener>()
                .WithProcessorOptions(feedProcessorOptions)
                .BuildAsync();

            await processor.StartAsync();

            Console.WriteLine("Change Feed Processor started. Press <Enter> key to stop...");

            Console.ReadLine();

            await processor.StopAsync();
        }
    }
}
