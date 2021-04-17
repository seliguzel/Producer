using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelInfra;
using ModelInfra.Messages;
using Nest;
using System;
using System.Threading.Tasks;

namespace Producer.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PublishMessageController : ControllerBase {

        private readonly IPublishEndpoint publishEndpoint;
        ILogger<PublishMessageController> _logger;
        IConfiguration _configuration;
        public PublishMessageController(IPublishEndpoint publishEndpoint, ILogger<PublishMessageController> logger, IConfiguration configuration) {
            this.publishEndpoint = publishEndpoint;
            this._logger = logger;
            this._configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<PublishResponce>> Publish(ProductMessage message) {
            if (message == null) {
                return BadRequest("RequestIsEmpty");
            } else {
                try {
                    await publishEndpoint.Publish<ProductMessage>(message);
                    _logger.LogInformation("PublishMessageController executed at {date}", DateTime.UtcNow);

                    return new PublishResponce {
                        IsSuccessful = true,
                        ResultMessage = "Successful"
                    };

                } catch (Exception exception) {
                    return new PublishResponce {
                        ResultMessage = $"Error when publishing message to clients, {exception.Message}"
                    };
                }
            }
        }
    }
}


