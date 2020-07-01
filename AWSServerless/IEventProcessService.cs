using System.Threading.Tasks;

namespace AWSServerless
{
    internal interface IEventProcessService
    {
        Task HandleTask();
    }
}