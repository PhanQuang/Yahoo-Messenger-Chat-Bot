using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using Utility;

namespace YahooLib
{
    public class FetchData
    {
        public string Username { get; set; }
        public string Passwod { get; set; }
        public string ConsummerKey { get; set; }
        public string ConsummerSecret { get; set; }

        public string ReqestToken { get; set; }

        public string OauthToken { get; set; }
        public string OauthTokenSecret { get; set; }

        public string Crumb { get; set; }

        public FetchData(string user, string pass, string conKey, string conSec)
        {
            Username = user;
            Passwod = pass;
            ConsummerKey = conKey;
            ConsummerSecret = conSec;
        }

        public void FetchRequestToken()
        {
            var param = new Dictionary<string, string>();
            param["login"] = Username;
            param["passwd"] = Passwod;
            param["oauth_consumer_key"] = ConsummerKey;

            var url = new StringBuilder(CommonConst.URL_OAUTH_DIRECT);
            url.Append(HttpUtil.ToQueryString(param, true, ""));

            WebRequest request = WebRequest.Create(url.ToString());
            var result = HttpUtil.SendGetReq(request, false);

            ReqestToken = result.Split('=')[1];
        }

        public void FetchAccessToken()
        {
            var param = new Dictionary<string, string>();
            param["oauth_consumer_key"] = ConsummerKey;
            param["oauth_nonce"] = OAuthUtil.GenerateNonce();
            param["oauth_signature"] = HttpUtility.UrlEncode(ConsummerSecret + "&");
            param["oauth_signature_method"] = "PLAINTEXT";
            param["oauth_timestamp"] = OAuthUtil.GenerateTimeStamp();
            param["oauth_token"] = ReqestToken;
            param["oauth_version"] = "1.0";

            var url = new StringBuilder(CommonConst.URL_OAUTH_ACCESS_TOKEN);
            url.Append(HttpUtil.ToQueryString(param, true, ""));

            WebRequest request = WebRequest.Create(url.ToString());
            var result = HttpUtil.SendGetReq(request, false);
            var tmp = StringUtil.Split(result, "=|&");
            OauthToken = tmp[1].Trim();
            OauthTokenSecret = tmp[3].Trim();
        }

        public void FetchCrumb()
        {
            var param = new Dictionary<string, string>();
            param["oauth_consumer_key"] = ConsummerKey;
            param["oauth_nonce"] = OAuthUtil.GenerateNonce();
            param["oauth_signature"] = ConsummerSecret + "%26" + OauthTokenSecret;
            param["oauth_signature_method"] = "PLAINTEXT";
            param["oauth_timestamp"] = OAuthUtil.GenerateTimeStamp();
            param["oauth_token"] = OauthToken;
            param["oauth_version"] = "1.0";

            var url = new StringBuilder(CommonConst.URL_YM_SESSION);
            url.Append(HttpUtil.ToQueryString(param, true, ""));

            WebRequest request = WebRequest.Create(url.ToString());
            request.ContentType = "application/json; charset=utf-8";

            var result = HttpUtil.SendGetReq(request, false);
            Crumb = GetCrumb(result);
        }

        public string GetCrumb(string jsonString)
        {
            JObject o = JObject.Parse(jsonString);
            return o["crumb"].ToString();
        }
    }
}
