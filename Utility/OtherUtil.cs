using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility
{
    public class OAuthUtil
    {
        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonce()
        {
            return new Random().Next(123400, 9999999).ToString();
        }

    }

    public class StringUtil
    {
        public static string[] Split(string input,string pattern)
        {
            string[] splitArray = null;
            try
            {
                splitArray = Regex.Split(input, pattern);
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return splitArray;
        }

        public static StringCollection GetIfMatchPattern(string input, string pattern)
        {
            var resultList = new StringCollection();
            try
            {
                Regex regexObj = new Regex(pattern);
                Match matchResult = regexObj.Match(input);
                while (matchResult.Success)
                {
                    resultList.Add(matchResult.Value);
                    matchResult = matchResult.NextMatch();
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return resultList;
        }

        public static string Replace(string input,string pattern, string newStr)
        {
            string resultString = null;
            try
            {
                resultString = Regex.Replace(input, pattern, newStr);
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return resultString;
        }

        public static List<string> MergeString(List<string> input)
        {
            var resultList = new List<string>();

            var tmp = new StringBuilder("");
            for(int i =0;i<input.Count;i++)
            {
                if(tmp.Length+input[i].Length < 2000)
                {
                    tmp.Append(input[i]+"\n");
                }else resultList.Add(tmp.ToString());
            }
            
            if(tmp.Length < 2000) resultList.Add(tmp.ToString());

            return resultList;
        }
    }
}
