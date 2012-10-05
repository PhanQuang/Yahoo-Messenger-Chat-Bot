using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Utility;

namespace YahooLib
{
    public class SignManager
    {
        public FetchData FetchReqData { get; set; }

        public string OauthHeader { get; set; }

        #region SignOn Field
        public string Cookie { get; set; }
        public string SessionId { get; set; }
        public string PrimaryLoginId { get; set; }
        public string Server { get; set; }
        public string NotifyServer { get; set; }
        #endregion

        #region Init
        public SignManager(FetchData fetch)
        {
            FetchReqData = fetch;
        }
        #endregion

        #region SignOn Function
        public void SignOn(string status, int state)
        {
            var param = new Dictionary<string, string>();
            param["realm"] = "yahooapis.com";
            param["oauth_consumer_key"] = FetchReqData.ConsummerKey;
            param["oauth_nonce"] = OAuthUtil.GenerateNonce();
            param["oauth_signature_method"] = "PLAINTEXT";
            param["oauth_timestamp"] = OAuthUtil.GenerateTimeStamp();
            param["oauth_token"] = FetchReqData.OauthToken;
            param["oauth_version"] = "1.0";
            param["oauth_signature"] = FetchReqData.ConsummerSecret + "%26" + FetchReqData.OauthTokenSecret;
            OauthHeader = MakeRequestHeader(param);

            var url = new StringBuilder(CommonConst.URL_YM_SESSION);
            url.Append("?notifyServerToken=1");

            WebRequest request = WebRequest.Create(url.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", OauthHeader);

            var data = MakeJsonData(status, state);
            var result = HttpUtil.SendPostReq(request, data, true);
            var temp = StringUtil.Split(result, "<QuangPB>");
            Cookie = GetCookie(temp[0].Trim());
            GetOtherInfoOfSignOn(temp[1].Trim());
        }

        private string MakeJsonData(string status, int state)
        {
            JObject o = new JObject();
            o.Add("presenceState", state);
            o.Add("presenceMessage", status);
            return o.ToString();
        }

        private string GetCookie(string input)
        {
            var cookie = StringUtil.GetIfMatchPattern(input, "IM=(.+?);")[0];
            cookie = StringUtil.Replace(cookie, "IM=|;", "");
            return cookie;
        }

        private void GetOtherInfoOfSignOn(string jString)
        {
            JObject o = JObject.Parse(jString);
            SessionId = o["sessionId"].ToString();
            PrimaryLoginId = o["primaryLoginId"].ToString();
            Server = o["server"].ToString();
            NotifyServer = o["notifyServer"].ToString();
        }
        #endregion

        #region SignOff Function
        public void SignOff()
        {
            var url = new StringBuilder(CommonConst.URL_YM_SESSION);
            url.Append("?" + CommonConst.DeleteSuffix);
            url.Append("&sid=" + SessionId);

            WebRequest request = WebRequest.Create(url.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", OauthHeader);

            HttpUtil.SendPostReq(request, "", false);
        }
        #endregion

        private string MakeRequestHeader<T>(IDictionary<string, T> agrs)
        {
            var sb = new StringBuilder("OAuth ");
            foreach (var kvp in agrs)
            {
                sb.AppendFormat("{0}=\"{1}\",", kvp.Key, kvp.Value);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
