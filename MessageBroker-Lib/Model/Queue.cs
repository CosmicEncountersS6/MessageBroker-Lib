using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker_Lib.Model
{
    public class Queue
    {
        public Queue(string queueName)
        {
            QueueName = queueName;
        }
        public string QueueName { get; set; }
        public bool IsExclusive { get; set; } = false;
        public bool IsDurable { get; set; } = true;
        public bool ShouldAutoDelete { get; set; } = false;
        public IDictionary<string, object>? Arguments { get; set; } = null;
        public Action<Dictionary<string, object>, string>? Callback { get; set; } = null;
    }
}
