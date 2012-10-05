using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Utility;

namespace YahooLib.ResponseData
{
    public class BuddyRequest
    {
        public int Sequence { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        private FetchData _fetch;
        private SignManager _sign;

        public BuddyRequest(FetchData fetch, SignManager sign, int seq, string sender, string rece)
        {
            _fetch = fetch;
            _sign = sign;
            Sequence = seq;
            Sender = sender;
            Receiver = rece;
        }

        public void ResponseContact(bool accept, string mess)
        {
            //prepare url
            var url = new StringBuilder(CommonConst.URL_YM_BUDDYREQUEST);
            url.Append("?oauth_consumer_key=" + _fetch.ConsummerKey);
            url.Append("&oauth_nonce=" + OAuthUtil.GenerateNonce());
            url.Append("&oauth_signature=" + _fetch.ConsummerSecret + "%26" + _fetch.OauthTokenSecret);
            url.Append("&oauth_signature_method=PLAINTEXT");
            url.Append("&oauth_timestamp=" + DateTime.Now.Ticks);
            url.Append("&oauth_token=" + _fetch.OauthToken);
            url.Append("&oauth_version=1.0");
            url.Append("&sid=" + _sign.SessionId);
            url.Replace("{{USER}}", Sender);

            WebRequest request = WebRequest.Create(url.ToString());
            request.ContentType = "application/json; charset=utf-8";
            var data = MakeJsonString(mess);
            HttpUtil.SendPostReq(request, data, false);
        }

        private string MakeJsonString(string reason)
        {
            JObject o = new JObject();
            o.Add("authReason", reason);
            return o.ToString();
        }

    }
}
