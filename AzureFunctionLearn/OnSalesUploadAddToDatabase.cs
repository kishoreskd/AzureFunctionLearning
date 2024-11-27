using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionLearn
{
    public class OnSalesUploadAddToDatabase
    {
        private readonly AzureTangyDbContext _context;

        public OnSalesUploadAddToDatabase(AzureTangyDbContext context)
        {
            _context = context;
        }

        [FunctionName("OnSalesUploadAddToDatabase")]
        public async Task Run([QueueTrigger("salesRequestBound", Connection = "AzureWebJobsStorage")] SalesRequest myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

                myQueueItem.Id = 0;
                myQueueItem.Status = "Submitted";
                await _context.SalesRequests.AddAsync(myQueueItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
             {
                log.LogInformation($"Erro: {ex.StackTrace}");

            }
        }
    }
}
