using Helsenorge.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helsenorge.Messaging.Http
{
    internal class IncomingHttpMessage : IMessagingMessage
    {
        public XDocument AMQPMessage { get; set; }
        //TODO: Rename all GetAMQPxxx to GetValue from XMLAMQPMessage or similar
        private string GetAMQPMessageField(string name)
        {
            return GetAMQPMessageFieldElement(name).Value;
        }

        private XElement GetAMQPMessageFieldElement(string name) //TODO: Return single child
        {
            var element = AMQPMessage.Root.Elements(name).SingleOrDefault();
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
                //TODO: Is this reasonable?
                return DateTime.Now + TimeSpan.FromMinutes(5);
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


        public string CorrelationId
        {
            get
            {
                return GetAMQPMessageField("CorrelationId");
            }
            set { throw new NotImplementedException(); }
        }

        public string MessageFunction
        {
            get
            {
                return GetAMQPMessageField("MessageFunction");
            }
            set { throw new NotImplementedException(); }
        }

        
        public string MessageId {
            get => GetAMQPMessageField("MessageId");
            set => throw new NotImplementedException();
        }

        //TODO: Other fields should also be parsed
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
                var reader = GetAMQPMessageFieldElement("Payload").CreateReader();
                reader.MoveToContent();

                var writer = new StreamWriter(memoryStream);
                writer.Write(reader.ReadInnerXml());
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
            // Do nothing
        }

        public void Dispose()
        {
            
        }
    }

}


