using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMonitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueConfig queueConfigSection = ConfigurationManager.GetSection("QueueConfig") as QueueConfig;
            var queueConfig = queueConfigSection.QueueConfiguration;
            foreach(QueueConfiguration queue in queueConfig)
            {
                var threshold = queue.Threshold;
                var counter = new SBMessageCounter(queue.SBNamespace, queue.QueueName, queue.SASKeyName, queue.SASKey);
                Console.WriteLine("MessageCount for Queue {0} is {1}, threshold {2}", queue.QueueName, counter.MessageCount, threshold);
            }
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }
    }
}
