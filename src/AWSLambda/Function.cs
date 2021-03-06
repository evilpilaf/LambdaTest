using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using LambdaCore;
using LambdaCore.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace AWSLambda
{
    public class Function
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
            : this(LambdaBootstrapper.GetDefaultServiceProvider())
        { }

        public Function(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            try
            {
                foreach (var _ in evnt.Records)
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    using (LogContext.PushProperty("CorrelationId", context.AwsRequestId))
                    {
                        ILogger<Function> logger = scope
                                                   .ServiceProvider
                                                   .GetService<ILoggerFactory>()
                                                   .CreateLogger<Function>();
                        logger.LogDebug("Logger created");
                        try
                        {
                            var useCase = scope.ServiceProvider.GetService<UseCase>();
                            logger.LogDebug("useCase created");

                            IEnumerable<StockLocationAddress> result = await useCase.Execute();

                            foreach (StockLocationAddress stockLocationAddress in result)
                            {
                                logger.LogInformation(
                                    "Id: {StockLocationId} DisplayName: {DisplayName}",
                                    stockLocationAddress.StockLocationId, stockLocationAddress.DisplayName);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogCritical(ex, "Exception when executing the usecase");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine(ex.Message);
                throw;
            }
        }
    }
}