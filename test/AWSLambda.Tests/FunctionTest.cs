using Amazon.Lambda.SQSEvents;
using FluentAssertions;
using LambdaCore;
using LambdaCore.Adapters;
using LambdaCore.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Serilog.Sinks.InMemory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace AWSLambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestSQSEventLambdaFunction()
        {
            var sqsEvent = new SQSEvent {
                Records = new List<SQSEvent.SQSMessage>
                {
                    new SQSEvent.SQSMessage()
                }
            };

            var log = new LoggerConfiguration()
                      .WriteTo.InMemory()
                      .CreateLogger();

            var mockStockLocationQuery = new Mock<IGetStockLocationsQuery>();
            var mockUseCase = new UseCase(mockStockLocationQuery.Object);

            var serviceProvider = new ServiceCollection()
                                  .AddLogging(c => c.AddSerilog(log))
                                  .AddSingleton<UseCase>(mockUseCase)
                                  .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            mockStockLocationQuery.Setup(m => m.GetStockLocations())
                       .ReturnsAsync(new[] { new StockLocationAddress(1, 1, nameof(StockLocationAddress)), });

            var function = new Function(serviceProvider);
            ILogger<Function> logger = loggerFactory.CreateLogger<Function>();
            await function.FunctionHandler(sqsEvent, logger);

            InMemorySink.Instance.LogEvents.Should()
                        .ContainSingle(
                            e => e.MessageTemplate.Text.Equals("Id: {StockLocationId} DisplayName: {DisplayName}"));
        }
    }
}
