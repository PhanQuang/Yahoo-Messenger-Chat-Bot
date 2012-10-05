using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace YahooLib.ResponseData
{
    public class Message
    {
        public int Sequence { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }

        public string Msg { get; set; }

        public Message(string sender, string receiver, string msg, int seq)
        {
            Sender = sender;
            Receiver = receiver;
            Msg = msg;
            Sequence = seq;
        }

        public string MakeJsonMessage(string input)
        {
            JObject o = new JObject();
            o.Add("message", input);
            return o.ToString();
        }
    }
}
