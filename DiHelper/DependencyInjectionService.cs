using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DiHelper
{
    public class DependencyInjectionService
    {
        public IServiceProvider BuildServices(string awsAccessKeyId, string awsSecretKey)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            var awsOptions = config.GetAWSOptions();
            awsOptions.Credentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretKey);
            var sqsConfig = new AmazonSQSConfig()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IConfiguration>(x => configBuilder.Build());
            serviceCollection.AddScoped<IAmazonSQS>(c => new AmazonSQSClient(awsAccessKeyId, awsSecretKey, sqsConfig));
            return serviceCollection.BuildServiceProvider();
        }
    }
}
