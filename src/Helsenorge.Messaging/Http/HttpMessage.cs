using Helsenorge.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helsenorge.Messaging.Http
{
    internal class HttpMessage : IMessagingMessage
    {
        public XDocument Payload { get; set; }
        public byte[] BinaryPayload { get; set; }

        /* IMessagingMessag implementation */

        public int FromHerId { get; set; }

        public int ToHerId { get; set; }

        public DateTime ApplicationTimestamp { get; set; }

        public string CpaId { get; set; }

        public DateTime EnqueuedTimeUtc
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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

        public string ContentType { get; set; }

        public string CorrelationId { get; set; }

        public string MessageFunction { get; set; }

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
            throw new NotImplementedException();
        }

        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public IMessagingMessage Clone()
        {
            throw new NotImplementedException();
        }

        public Stream GetBody()
        {
            throw new NotImplementedException();
        }

        public void AddDetailsToException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}


