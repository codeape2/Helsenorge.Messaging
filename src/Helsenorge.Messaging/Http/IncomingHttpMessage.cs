using Helsenorge.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helsenorge.Messaging.Http
{
    internal class IncomingHttpMessage : IMessagingMessage
    {
        public XDocument AMQPMessage { get; set; }
        private string GetAMQPMessageField(string name)
        {
            return GetAMQPMessageFieldElement(name).Value;
        }

        private XElement GetAMQPMessageFieldElement(string name) //TODO: Return single child
        {
            var element = AMQPMessage.Root.Element(name);
            if (element == null)
            {
                throw new ArgumentException($"Cannot find message field named '{name}'");
            }
            return element;
        }

        public byte[] BinaryPayload { get; set; }

        /* IMessagingMessage implementation */

        public int FromHerId
        {
            get
            {
                return int.Parse(GetAMQPMessageField("FromHerId"));
            }
            set { throw new NotImplementedException(); }
        }

        public int ToHerId
        {
            get
            {
                return int.Parse(GetAMQPMessageField("ToHerId"));
            }
            set { throw new NotImplementedException(); }
        }


        public DateTime ApplicationTimestamp { get; set; } = DateTime.Now; //TODO Implement parsing from file

        public string CpaId { get; set; }

        public DateTime EnqueuedTimeUtc { get; set; }

        public DateTime ExpiresAtUtc
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, object> Properties { get; set; }

        public long Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ContentType
        {
            get
            {
                return GetAMQPMessageField("ContentType");
            }
            set { throw new NotImplementedException(); }
        }


        public string CorrelationId { get; set; }

        public string MessageFunction
        {
            get
            {
                return GetAMQPMessageField("MessageFunction");
            }
            set { throw new NotImplementedException(); }
        }


        public string MessageId { get; set; }

        public string ReplyTo { get; set; }

        public DateTime ScheduledEnqueueTimeUtc { get; set; }

        public TimeSpan TimeToLive { get; set; }

        public string To { get; set; }

        public object OriginalObject
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Complete()
        {
            
        }

        public Task CompleteAsync()
        {
            return Task.CompletedTask;
        }

        public IMessagingMessage Clone()
        {
            throw new NotImplementedException();
        }

        public Stream GetBody()
        {
            var memoryStream = new MemoryStream();
            if (ContentType == "text/plain") // ... or SOAP
            {
                var writer = new StreamWriter(memoryStream);
                writer.Write(GetAMQPMessageFieldElement("Payload").ToString());
                writer.Flush();            
            }
            else
            {
                throw new ArgumentException($"Have no idea what to do with content type '{ContentType}'");
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public void AddDetailsToException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }

}


