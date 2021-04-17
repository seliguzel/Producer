using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelInfra;
using ModelInfra.Messages;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer1.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMessageController : ControllerBase {

        
        ILogger<ProductMessageController> _logger;
        IConfiguration _configuration;
        IElasticClientService client;
        public ProductMessageController(ILogger<ProductMessageController> logger, IConfiguration configuration,IElasticClientService clientService) {
            
            this._logger = logger;
            this._configuration = configuration;
            this.client = clientService;
        }

        [HttpGet("{id}")]
        public ActionResult<PublishResponce> GetPublishedMessageById(int id) {

            var message = client.Get<ProductMessage>(id, idx => idx.Index(_configuration["ElasticConfiguration:DataIndex"]));

            if (message == null) {
                return NotFound($"Message with {id} not found");
            }

            return Ok(new PublishResponce {
                IsSuccessful = true,
                ResultMessage = "Successful",
                PublishedMessage = message.Source,
            });
        }

        [HttpGet]
        public IActionResult GetPublishedMessages() {

            var message = client.Search<ProductMessage>(s => s
                .Index(_configuration["ElasticConfiguration:DataIndex"])
                .From(0)
                .Size(100)
            );
            if (message == null) {
                return NotFound($"Message with not found");
            }

            return Ok(message.Documents);
        }
    }


}
