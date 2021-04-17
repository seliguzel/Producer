using System;

namespace ModelInfra {
    public class ProductMessage {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public DateTime ProduceDate { get; set; }
    }
}
