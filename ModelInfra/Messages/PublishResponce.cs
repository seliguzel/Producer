using System;
using System.Collections.Generic;
using System.Text;

namespace ModelInfra.Messages {

    public class PublishResponce {
        public bool IsSuccessful { get; set; }
        public string ResultMessage { get; set; }
        public ProductMessage PublishedMessage { get; set; }
    }

}


