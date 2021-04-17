using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer1 {
    public interface IElasticClientService {

        GetResponse<TDocument> Get<TDocument>(DocumentPath<TDocument> id, Func<GetDescriptor<TDocument>, IGetRequest> selector = null) where TDocument : class;

        ISearchResponse<TDocument> Search<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> selector = null) where TDocument : class;
    }
}
