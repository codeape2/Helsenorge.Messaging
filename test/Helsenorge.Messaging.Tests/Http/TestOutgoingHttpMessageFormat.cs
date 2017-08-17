using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helsenorge.Messaging.Http;
using System.Xml.Linq;
using System.Linq;

namespace Helsenorge.Messaging.Tests.Http
{
    [TestClass]
    public class TestOutgoingHttpMessageFormat
    {
        [TestMethod]
        public void CheckOutgoingSerialization()
        {
            var om = new OutgoingHttpMessage
            {                
                ApplicationTimestamp = new DateTime(2017, 1, 1),
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

            var xelement = om.CreateHttpBody();
            Assert.IsNotNull(xelement);

            AssertChildElementEquals("2017-01-01T00:00:00", xelement, "ApplicationTimestamp");
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
