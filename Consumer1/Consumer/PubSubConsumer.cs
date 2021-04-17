using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelInfra;
using Nest;
using System;
using System.Threading.Tasks;

namespace Consumer1 {
    internal class PublishConsumer : IConsumer<ProductMessage> {
        private readonly ILogger<PublishConsumer> logger;
        private readonly IConfiguration configuration;
        public PublishConsumer(ILogger<PublishConsumer> logger, IConfiguration configuration) {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task Consume(ConsumeContext<ProductMessage> context) {
            var node = new Uri(configuration["ElasticConfiguration:Uri"]);
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            context.Message.ProduceDate = DateTime.Now;
            var response = client.Index(context.Message, idx => idx.Index(configuration["ElasticConfiguration:DataIndex"]));
            throw new Exception("Test Exception");
            logger.LogInformation($"Got new message {context.Message.Name}");
        }
    }
}