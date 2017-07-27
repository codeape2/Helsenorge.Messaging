﻿using Helsenorge.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helsenorge.Messaging.Http
{
    class HttpServiceBusReceiver : IMessagingReceiver
    {
        private string _url;
        private string _id;

        public HttpServiceBusReceiver(string url, string id)
        {
            _url = url;
            _id = id;   
        }

        public bool IsClosed => false;

        public void Close()
        {
            
        }

        public async Task<IMessagingMessage> ReceiveAsync(TimeSpan serverWaitTime)
        {
            Console.Error.WriteLine("ReceiveAsync");
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(new Uri(new Uri(_url), _id));
            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "")
            {
                await Task.Delay(serverWaitTime);
                return null;
            }
            else
            {
                await Task.Delay(serverWaitTime);
                return new IncomingHttpMessage
                {
                    AMQPMessage = XDocument.Parse(responseString),
                    EnqueuedTimeUtc = DateTime.Now //TODO: Should be parsed?
                };
            }            
        }
    }
}
