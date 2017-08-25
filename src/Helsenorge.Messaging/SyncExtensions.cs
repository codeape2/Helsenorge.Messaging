using Helsenorge.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helsenorge.Messaging.Abstractions
{
    public static class SyncExtensions
    {
        public static void SendAndContinue(this IMessagingClient messagingClient, ILogger logger, OutgoingMessage message)
        {
            Task.Run(async () => await messagingClient.SendAndContinueAsync(logger, message)).GetAwaiter().GetResult();
        }

        public static XDocument SendAndWait(this IMessagingClient messagingClient, ILogger logger, OutgoingMessage message)
        {
            return Task.Run(async () => await messagingClient.SendAndWaitAsync(logger, message)).GetAwaiter().GetResult();
        }
    }
}
