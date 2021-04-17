﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Helper {
    public class RabbitMqConfig {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
