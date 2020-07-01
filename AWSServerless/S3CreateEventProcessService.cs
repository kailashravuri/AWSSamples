using System.Threading.Tasks;

namespace AWSServerless
{
    internal class S3CreateEventProcessService : IEventProcessService
    {
        public async Task HandleTask()
        {
            await Task.CompletedTask;
        }
    }
}