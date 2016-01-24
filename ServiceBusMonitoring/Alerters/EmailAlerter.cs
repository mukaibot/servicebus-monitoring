using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ServiceBusMonitoring.Alerters
{
    public class EmailAlerter : IAlerter
    {
        private MailMessage message;
        private SmtpClient client;

        public EmailAlerter()
        {
            SetConfiguration();
        }

        public bool Send()
        {
            message.Subject = String.Format("ServiceBusMonitoring: Threshold exceeded for {0}", Queue);
            message.Body = String.Format("The Queue '{0}' in ServiceBus namespace '{1}' contains {2} messages. This exceeds the threshold of {3}", Queue, SBNamespace, MessageCount, Threshold);
            client.Send(message);
            return true;
        }

        public void SetMessageCount(long messageCount)
        {
            MessageCount = messageCount;
        }

        public void SetQueueName(string queueName)
        {
            Queue = queueName;
        }

        public void SetSBNamespace(string nameSpace)
        {
            SBNamespace = nameSpace;
        }

        public void SetThreshold(int threshold)
        {
            Threshold = threshold;
        }

        private void SetConfiguration()
        {
            var toAddr = ConfigurationManager.AppSettings["EmailTo"].ToString();
            var username = ConfigurationManager.AppSettings["EmailUser"].ToString();
            var password = ConfigurationManager.AppSettings["EmailPassword"].ToString();
            var mailHost = ConfigurationManager.AppSettings["EmailHost"].ToString();
            var mailPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);

            client = new SmtpClient(mailHost, mailPort);
            client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailEnableSSL"]);
            client.Credentials = new NetworkCredential(username, password);

            message = new MailMessage();
            message.To.Add(new MailAddress(toAddr));
            message.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"].ToString());
        }

        public string SBNamespace { get; private set; }

        public string Queue { get; private set; }

        public long MessageCount { get; private set; }

        public int Threshold { get; private set; }
    }
}
