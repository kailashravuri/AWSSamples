using Amazon.SQS;
using Amazon.SQS.Model;
using DiHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SqsReceiver
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.Write("\nEnter AWS Access Key ID:");
            var accessKeyId = Console.ReadLine();

            Console.Write("\nEnter AWS Secret Key:");
            var secretKey = Console.ReadLine();

            var serviceProvider = new DependencyInjectionService().BuildServices(accessKeyId, secretKey);
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            try
            {
                var sqsClient = serviceProvider.GetRequiredService<IAmazonSQS>();

                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = configuration["QueueUrl"]            
                };

                var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

                foreach (var message in receiveMessageResponse.Messages)
                {
                    Console.WriteLine(message.Body);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);               
            }

            Console.ReadLine();
        }
    }
}
