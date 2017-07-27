using Helsenorge.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helsenorge.Messaging.Http
{
    public class HttpServiceBusSender : IMessagingSender
    {
        private string _url;

        public HttpServiceBusSender(string url)
        {
            _url = url;
        }

        public bool IsClosed => false;

        public void Close()
        {
            //
        }

        public async Task SendAsync(IMessagingMessage message)
        {
            Debug.Assert(message is HttpMessage);
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(_url, new StringContent(CreateHttpBody((HttpMessage)message).ToString()));
            response.EnsureSuccessStatusCode();
        }

        private XElement CreateHttpBody(HttpMessage message)
        {
            Debug.Assert(message != null);
            Debug.Assert(message.Payload != null);
            Debug.Assert(message.Payload.Root != null);
            Debug.Assert(message.BinaryPayload != null);
            
            return new XElement("AMQPMessage",
                new XElement("Payload", message.Payload.Root),
                new XElement("BinaryPayload", Convert.ToBase64String(message.BinaryPayload)),

                new XElement("ApplicationTimestamp", message.ApplicationTimestamp),
                new XElement("ContentType", message.ContentType),
                new XElement("CorrelationId", message.CorrelationId),
                new XElement("CpaId", message.CpaId),
                new XElement("ScheduledEnqueueTimeUtc", message.ScheduledEnqueueTimeUtc),
                new XElement("TimeToLive", message.TimeToLive),
                new XElement("To", message.To),
                new XElement("ToHerId", message.ToHerId)
            );
        }
    }
}