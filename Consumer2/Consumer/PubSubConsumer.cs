using MassTransit;
using Microsoft.Extensions.Logging;
using ModelInfra;
using System;
using System.Threading.Tasks;

namespace Consumer2 {
    internal class PublishConsumer : IConsumer<ProductMessage> {
        private readonly ILogger<PublishConsumer> logger;

        public PublishConsumer(ILogger<PublishConsumer> logger) {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductMessage> context) {
            await Console.Out.WriteLineAsync(context.Message.Name);
            //throw new Exception("Test Exception");
            logger.LogInformation($"Got new message {context.Message.Name}");
        }
    }
}