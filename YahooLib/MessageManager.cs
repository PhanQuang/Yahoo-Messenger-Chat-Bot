using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Utility;

namespace YahooLib
{
    public class MessageManager
    {
        public FetchData FetchReqData { get; set; }
        public SignManager SignMan { get; set; }
        public string Test { get; set; }

        public MessageManager(FetchData fetch, SignManager sign)
        {
            FetchReqData = fetch;
            SignMan = sign;
        }

        public void SendMessage(string user, string msg)
        {
            //prepare url
            var url = new StringBuilder(CommonConst.URL_YM_MESSAGE);
            url.Append("?sid=" + SignMan.SessionId);
            url.Replace("{{USER}}", user);

            WebRequest request = WebRequest.Create(url.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", SignMan.OauthHeader);

            HttpUtil.SendPostReq(request, msg, false);
        }

        public string FetchLongNotification(int seq)
        {
            var url = new StringBuilder(CommonConst.URL_YM_NOTIFICATION_LONG);
            url.Append("?sid=" + SignMan.SessionId);
            url.Append("&seq=" + seq);
            url.Append("&format=json");
            url.Append("&count=100");
            url.Append("&idle=120");
            url.Append("&rand=" + OAuthUtil.GenerateNonce());
            url.Replace("{{NOTIFICATION_SERVER}}", SignMan.NotifyServer);
            url.Replace("{{USER}}", SignMan.PrimaryLoginId);

            WebRequest request = WebRequest.Create(url.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", SignMan.OauthHeader);
            request.Headers.Add("Cookie", "IM=" + SignMan.Cookie);
            request.Timeout = 160 * 1000;

            return HttpUtil.SendGetReq(request, false);
        }

    }
}
