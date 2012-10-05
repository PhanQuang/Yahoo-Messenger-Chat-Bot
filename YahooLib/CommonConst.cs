using System;
using System.Collections.Generic;
using System.Text;

namespace YahooLib
{
    public class CommonConst
    {
        public static string DeleteSuffix = "_method=delete";
        public static string PutSuffix = "_method=put";
        public static string CreateSuffix = "_method=create";

        public static string URL_OAUTH_DIRECT = "https://login.yahoo.com/WSLogin/V1/get_auth_token";
        public static string URL_OAUTH_ACCESS_TOKEN = "https://api.login.yahoo.com/oauth/v2/get_token";
        public static string URL_YM_SESSION = "http://developer.messenger.yahooapis.com/v1/session";
        public static string URL_YM_PRESENCE = "http://developer.messenger.yahooapis.com/v1/presence";
        public static string URL_YM_CONTACT = "http://developer.messenger.yahooapis.com/v1/contacts";
        public static string URL_YM_MESSAGE = "http://developer.messenger.yahooapis.com/v1/message/yahoo/{{USER}}";
        public static string URL_YM_NOTIFICATION = "http://developer.messenger.yahooapis.com/v1/notifications";
        public static string URL_YM_NOTIFICATION_LONG = "http://{{NOTIFICATION_SERVER}}/v1/pushchannel/{{USER}}";
        public static string URL_YM_BUDDYREQUEST = "http://developer.messenger.yahooapis.com/v1/buddyrequest/yahoo/{{USER}}";
        public static string URL_YM_GROUP = "http://developer.messenger.yahooapis.com/v1/group/{{GROUP}}/contact/yahoo/{{USER}}";
    }
}
