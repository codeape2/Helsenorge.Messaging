using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helsenorge.Messaging.Http;
using System.Xml.Linq;
using System.Linq;

namespace Helsenorge.Messaging.Tests.Http
{
    [TestClass]
    public class TestHttpSerialization
    {
        private OutgoingHttpMessage CreateOutgoingHttpMessage()
        {
            return new OutgoingHttpMessage
            {
                ApplicationTimestamp = new DateTime(2017, 1, 1, 13, 30, 10),
                ContentType = "text/plain",
                CorrelationId = "correlationid",
                CpaId = "cpaid",
                EnqueuedTimeUtc = new DateTime(2017, 1, 1),
                FromHerId = 123,
                MessageFunction = "msgfunc",
                MessageId = "msgid",
                ReplyTo = "replyto",
                ScheduledEnqueueTimeUtc = new DateTime(2017, 1, 1),
                TimeToLive = TimeSpan.FromSeconds(30),
                To = "to",
                ToHerId = 456
            };
        }

        [TestMethod]
        public void CheckOutgoingSerialization()
        {
            var xelement = CreateOutgoingHttpMessage().CreateHttpBody();
            Assert.IsNotNull(xelement);

            AssertChildElementEquals("2017-01-01T13:30:10", xelement, "ApplicationTimestamp");
            AssertChildElementEquals("text/plain", xelement, "ContentType");
            AssertChildElementEquals("correlationid", xelement, "CorrelationId");
            AssertChildElementEquals("cpaid", xelement, "CpaId");
            AssertChildElementEquals("2017-01-01T00:00:00", xelement, "EnqueuedTimeUtc");
            AssertChildElementEquals("123", xelement, "FromHerId");
            AssertChildElementEquals("msgfunc", xelement, "MessageFunction");
            AssertChildElementEquals("msgid", xelement, "MessageId");
            AssertChildElementEquals("replyto", xelement, "ReplyTo");
            AssertChildElementEquals("2017-01-01T00:00:00", xelement, "ScheduledEnqueueTimeUtc");
            AssertChildElementEquals("00:00:30", xelement, "TimeToLive");
            AssertChildElementEquals("to", xelement, "To");
            AssertChildElementEquals("456", xelement, "ToHerId");


            AssertChildElementEquals("#WARNING: No payload", xelement, "Payload");
            AssertChildElementEquals("#WARNING: No binary payload", xelement, "BinaryPayload");
        }

        [TestMethod]
        public void CheckIncomingSerialization()
        {
            var incomingMessage = new IncomingHttpMessage { AMQPMessage = CreateOutgoingHttpMessage().CreateHttpBody() };
            Assert.AreEqual(new DateTime(2017, 1, 1, 13, 30, 10), incomingMessage.ApplicationTimestamp);
            Assert.AreEqual("text/plain", incomingMessage.ContentType);
            Assert.AreEqual("correlationid", incomingMessage.CorrelationId);
            Assert.AreEqual("cpaid", incomingMessage.CpaId);
            Assert.AreEqual(new DateTime(2017, 1, 1), incomingMessage.EnqueuedTimeUtc);
            Assert.AreEqual(123, incomingMessage.FromHerId);
            Assert.AreEqual("msgfunc", incomingMessage.MessageFunction);
            Assert.AreEqual("msgid", incomingMessage.MessageId);
            Assert.AreEqual("replyto", incomingMessage.ReplyTo);
            Assert.AreEqual(new DateTime(2017, 1, 1), incomingMessage.ScheduledEnqueueTimeUtc);
            Assert.AreEqual(TimeSpan.FromSeconds(30), incomingMessage.TimeToLive);
            Assert.AreEqual("to", incomingMessage.To);
            Assert.AreEqual(456, incomingMessage.ToHerId);
        }

        //TODO: Check parsing of datetime styles:
        /*
         * 2017-08-18T10:13:59.0621301+02:00
2017-08-18T10:13:59.0621301
2017-08-18T10:13:59.0621301Z
*/

        private XElement AssertContains(XElement parent, string elementname)
        {
            var child = parent.Elements(elementname).SingleOrDefault();
            Assert.IsNotNull(child);
            return child;
        }

        private void AssertChildElementEquals(string expected, XElement parent, string elementname)
        {
            Assert.AreEqual(expected, AssertContains(parent, elementname).Value);
        }
    }
}
