using Amazon.SQS;
using Amazon.SQS.Model;
using DiHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SqsSender
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.Write("\nEnter AWS Access Key ID:");
            var accessKeyId = Console.ReadLine();

            Console.Write("\nEnter AWS Secret Key:");
            var secretKey = Console.ReadLine();

            Console.Write("\nEnter queue messageBody:");
            var messageBody = Console.ReadLine();

            try
            {
                var serviceProvider = new DependencyInjectionService().BuildServices(accessKeyId, secretKey);
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var queueUrl = configuration["QueueUrl"];

                var sendMessagerequest = new SendMessageRequest
                {
                    QueueUrl = queueUrl,
                    MessageBody = $"{{'prop-1':'{messageBody}'}}"
                };

                var sqsClient = serviceProvider.GetRequiredService<IAmazonSQS>();
                await sqsClient.SendMessageAsync(sendMessagerequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);                
            }

            Console.ReadLine();
        }
    }
}
