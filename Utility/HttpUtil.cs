using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Utility
{
    public class HttpUtil
    {
        public static string SendPostReq(WebRequest request, string data, bool hasHeader)
        {
            // Create a request using a URL that can receive a post. 
            //WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            // Set the ContentType property of the WebRequest.
            //request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            var responseFromServer = new StringBuilder("");
            if(hasHeader)
            {
                responseFromServer.Append(response.Headers);
                responseFromServer.Append("<QuangPB>");
            }
            responseFromServer.Append(reader.ReadToEnd());
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer.ToString();
        }

        public static string SendGetReq(WebRequest request, bool hasHeader)
        {
            request.Method = "GET";
            var response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            var responseFromServer = new StringBuilder("");
            if (hasHeader)
            {
                responseFromServer.Append(request.Headers);
                responseFromServer.Append("<QuangPB>");
            }
            responseFromServer.Append(reader.ReadToEnd());
            return responseFromServer.ToString();
        }

        public static string ToQueryString<T>(IDictionary<string, T> agrs, bool questionMark, params string[] ignores)
        {
            var sb = new StringBuilder();
            if (questionMark)
            {
                sb.Append("?");
            }
            var l = new List<string>(ignores);
            foreach (var kvp in agrs)
            {
                if (l.Contains(kvp.Key)) continue;

                sb.AppendFormat("{0}={1}&", kvp.Key, kvp.Value);
            }
            return sb.ToString().TrimEnd('&');
        }
    }
}
