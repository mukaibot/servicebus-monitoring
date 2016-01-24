namespace ServiceBusMonitoring.Alerters
{
    public interface IAlerter
    {
        void SetSBNamespace(string nameSpace);

        void SetQueueName(string queueName);

        void SetThreshold(int threshold);

        void SetMessageCount(long messageCount);

        bool Send();
    }
}
