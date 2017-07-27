
using Helsenorge.Messaging.Abstractions;
using System;
using System.Diagnostics;
using System.IO;

namespace Helsenorge.Messaging.Http
{
    internal class HttpServiceBusFactory : IMessagingFactory
    {
        private string _baseurl;
        public HttpServiceBusFactory(string baseurl)
        {
            Debug.Assert(baseurl != null);
            _baseurl = baseurl;
        }
        public IMessagingReceiver CreateMessageReceiver(string id)
        {
            throw new NotImplementedException();
            //return new ServiceBusReceiver(_implementation.CreateMessageReceiver(id, ReceiveMode.PeekLock));
        }
        public IMessagingSender CreateMessageSender(string id)
        {
            return new HttpServiceBusSender(_baseurl); //TODO Add something for sender
        }
        bool ICachedMessagingEntity.IsClosed => false;
        void ICachedMessagingEntity.Close() {
        }
        public IMessagingMessage CreteMessage(Stream stream, OutgoingMessage outgoingMessage)
        {
            Debug.Assert(stream != null);
            Debug.Assert(outgoingMessage != null);

            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return new HttpMessage { Payload = outgoingMessage.Payload, BinaryPayload = memoryStream.ToArray() };
        }
    }

}
