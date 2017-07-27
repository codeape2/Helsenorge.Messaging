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
        private string _id;

        public HttpServiceBusSender(string url, string id)
        {
            _url = url;
            _id = id;
        }

        public bool IsClosed => false;

        public void Close()
        {
            //
        }

        public async Task SendAsync(IMessagingMessage message)
        {
            Debug.Assert(message is OutgoingHttpMessage);
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(new Uri(new Uri(_url), _id), new StringContent(CreateHttpBody((OutgoingHttpMessage)message).ToString()));
            response.EnsureSuccessStatusCode();
        }

        private XElement CreateHttpBody(OutgoingHttpMessage message)
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