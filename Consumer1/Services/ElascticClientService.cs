using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer1 {
    public class ElascticClientService : IElasticClientService {
        
        ElasticClient client;
        public ElascticClientService( IConfiguration configuration) {
            var node = new Uri(configuration["ElasticConfiguration:Uri"]);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
        }
        public GetResponse<TDocument> Get<TDocument>(DocumentPath<TDocument> id, Func<GetDescriptor<TDocument>, IGetRequest> selector = null) where TDocument : class {
            return client.Get(id, selector);
        }

        public ISearchResponse<TDocument> Search<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> selector = null) where TDocument : class {
            return client.Search(selector);
        }
    }
}
