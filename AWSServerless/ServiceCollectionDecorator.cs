using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AWSServerless
{
    internal class ServiceCollectionDecorator : ServiceCollection
    {    

        public ServiceCollectionDecorator()
        {
            BuildServices();
        }

        private void BuildServices()
        {
            //var s3Config = new AmazonS3Config() { RegionEndpoint = Amazon.RegionEndpoint.USWest2 };
            //var credentials = new BasicAWSCredentials("demo", "secret");
            this.AddTransient<IEventProcessService, S3CreateEventProcessService>();
            this.AddSingleton<IAmazonS3>(x=> new AmazonS3Client(Amazon.RegionEndpoint.EUWest2));
        }
    }
}