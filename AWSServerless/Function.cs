using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSServerless
{
    public class Function
    {
        private IAmazonS3 S3Client { get;}
        internal IEventProcessService RequestProcessService { get; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            var serviceCollectionDecorator = new ServiceCollectionDecorator();
            var serviceProvider = serviceCollectionDecorator.BuildServiceProvider();
            RequestProcessService = serviceProvider.GetRequiredService<IEventProcessService>();
            S3Client = serviceProvider.GetRequiredService<IAmazonS3>();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        internal Function(IAmazonS3 s3Client, IEventProcessService requestProcessService)
        {
            this.S3Client = s3Client;
            RequestProcessService = requestProcessService;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(JsonContent content, ILambdaContext context)
        {
            
            if (content == null)
            {
                return;
            }

            try
            {
                //var response = await this.S3Client.GetObjectMetadataAsync(s3Event.Bucket.Name, s3Event.Object.Key);
                await RequestProcessService.HandleTask();
                //return response.Headers.ContentType;
            }
            catch (Exception e)
            {                
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }
    }
}
