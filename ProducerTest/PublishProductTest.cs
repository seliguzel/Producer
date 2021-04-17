using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelInfra;
using Moq;
using Producer.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProducerTest {
    public class PublishTest {
        [Fact]
        public async Task PublishProductTest() {
            
            // arrange
            var publishEndPoint = new Mock<IPublishEndpoint>();
            var logger = new Mock<ILogger<PublishMessageController>>();
            var configuration = new Mock<IConfiguration>();
            PublishMessageController publishControlller = new PublishMessageController(publishEndPoint.Object, logger.Object,configuration.Object);
            ProductMessage prodMes = new ProductMessage();
            prodMes.Amount = 1;
            prodMes.Count = 2;
            prodMes.Id = 21;
            prodMes.Name = "Name";

            // act
            var result = await publishControlller.Publish(prodMes);

            // assert
            publishEndPoint.Verify(f => f.Publish(prodMes, CancellationToken.None), Times.Once);
        }


        [Fact]
        public async Task BadReqTest() {

            // arrange
            var publishEndPoint = new Mock<IPublishEndpoint>();
            var logger = new Mock<ILogger<PublishMessageController>>();
            var configuration = new Mock<IConfiguration>();
            PublishMessageController publishControlller = new PublishMessageController(publishEndPoint.Object, logger.Object, configuration.Object);

            // act
            var result = await publishControlller.Publish(null);
            //await publishControlller.PublishMessage(null);

            // assert
            publishEndPoint.Verify(f => f.Publish(It.IsAny<ProductMessage>(), CancellationToken.None), Times.Never);
        }

    }
}
