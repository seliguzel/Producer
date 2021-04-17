using MassTransit;
using Microsoft.Extensions.Logging;
using ModelInfra;
using System;
using System.Threading.Tasks;

namespace Consumer2 {
    public class ProductMessageFaultConsumer : IConsumer<Fault<ProductMessage>> {
        private readonly ILogger<ProductMessageFaultConsumer> logger;

        public ProductMessageFaultConsumer(ILogger<ProductMessageFaultConsumer> logger) {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<Fault<ProductMessage>> context) {
            
            //do some stuff
            await Console.Out.WriteLineAsync(context.Message.Message.ToString());
            logger.LogInformation($"Got new message {context.Message.Exceptions}");
        }
    }
}


