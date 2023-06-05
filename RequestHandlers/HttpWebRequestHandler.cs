using RESTfulAPIConsume.Constants;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RESTfulAPIConsume.Constants;
using RESTfulAPIConsume.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
//using System.Net.WebClient.DownloadFile;
using System.IO;
using Newtonsoft.Json.Linq;


namespace RESTfulAPIConsume.RequestHandlers
{
    public class HttpWebRequestHandler : IRequestHandler
    {
        public string GetId(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.UserAgent = RequestConstants.UserAgentValue;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(RequestConstants.auth));

            var content = string.Empty;
            var contentReversed = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            //contentReversed 
            return content;
        }
    }
}