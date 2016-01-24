using System;
using System.Configuration;

using ServiceBusMonitoring.Alerters;

namespace ServiceBusMonitoring
{
    class Program
    {
        static int Main(string[] args)
        {
            QueueConfig queueConfigSection = ConfigurationManager.GetSection("QueueConfig") as QueueConfig;
            int failCount = 0;
            QueueConfigurationCollection queueConfig = new QueueConfigurationCollection();

            try
            {
                queueConfig = queueConfigSection.QueueConfiguration;
            }
            catch (NullReferenceException)
            {
                var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                Console.WriteLine("Configuration file '{0}' not found or invalid. See https://github.com/mukaibot/servicebus-monitoring for instructions", configFile);
                Environment.Exit(1);
            }

            failCount = ProcessQueues(failCount, queueConfig);

            if (failCount != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static int ProcessQueues(int failCount, QueueConfigurationCollection queueConfig)
        {
            foreach (QueueConfiguration queue in queueConfig)
            {
                Console.WriteLine("OK: Trying to get message count from {0} in ServiceBus namespace {1}", queue.QueueName, queue.SBNamespace);
                var threshold = queue.Threshold;
                var counter = new SBMessageCounter(queue.SBNamespace, queue.QueueName, queue.SASKeyName, queue.SASKey);
                if (counter.MessageCount <= threshold)
                {
                    Console.WriteLine("OK: MessageCount for Queue {0} is {1}, threshold {2}", queue.QueueName, counter.MessageCount, threshold);
                }
                else
                {
                    failCount += 1;
                    var alerterTypeFromConfig = ConfigurationManager.AppSettings["AlerterType"].ToString() ?? "null";
                    Console.WriteLine("FAIL: MessageCount for Queue {0} is {1}, threshold {2}. Sending alert via {3}", queue.QueueName, counter.MessageCount, threshold, alerterTypeFromConfig);
                    if (alerterTypeFromConfig != "null")
                    {
                        SendAlert(queue, counter, alerterTypeFromConfig);
                    }
                }
            }

            return failCount;
        }

        private static void SendAlert(QueueConfiguration queue, SBMessageCounter counter, string alerterTypeFromConfig)
        {
            Type alerterType = Type.GetType(String.Format("ServiceBusMonitoring.Alerters.{0}", alerterTypeFromConfig));
            IAlerter alerter = (IAlerter)(Activator.CreateInstance(alerterType));
            alerter.SetMessageCount(counter.MessageCount);
            alerter.SetQueueName(queue.QueueName);
            alerter.SetSBNamespace(queue.SBNamespace);
            alerter.SetThreshold(queue.Threshold);
            alerter.Send();
        }
    }
}
