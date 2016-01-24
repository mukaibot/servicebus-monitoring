using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Management;

namespace ServiceBusMonitoring
{
    public class SBMessageCounter
    {
        public SBMessageCounter(string ns, string queue, string keyName, string key)
        {
            var sb = ServiceBusEnvironment.CreateServiceUri("sb", ns, String.Empty);
            TokenProvider tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, key);
            NamespaceManager sbus = new NamespaceManager(sb, tokenProvider);
            var q = sbus.GetQueue(queue);
            MessageCount = q.MessageCount;
        }

        public long MessageCount
        {
            get; private set;
        }
    }
}
