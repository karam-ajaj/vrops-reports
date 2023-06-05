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
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;


namespace RESTfulAPIConsume
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing the API request ...");
            Console.WriteLine();

            IRequestHandler httpWebRequestHandler = new HttpWebRequestHandler();

            // 1st get request:
            // authentication 
            WebRequest reqID = WebRequest.Create(RequestConstants.UrlGetReportDefinitionId);
            reqID.Method = "GET";
            reqID.ContentType = "application/json";
            reqID.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(RequestConstants.auth));
            HttpWebResponse respID = reqID.GetResponse() as HttpWebResponse;

            // show date in English
            CultureInfo en = new CultureInfo("en-US");

            // create current month and year variables
            DateTime now = DateTime.Now;
            string month = now.ToString("MMM" , en);
            string year = now.ToString("yyyy", en);
            //Console.WriteLine("Month is " + month);
            //Console.WriteLine("Year is " + year);
            //Console.WriteLine("Today is " + now.ToString("MMM dd yyyy" , en));

            var response = GetId(httpWebRequestHandler);

            // show all JSON response in console
            //Console.WriteLine("response: {0}", response );

            // parse the response to dynamic data variable
            dynamic data = JObject.Parse(response);
   
            // show specific parameters in console
            //Console.WriteLine("host: {0}", data.host);
            //Console.WriteLine("version: {0}", data.info.version);
            //Console.WriteLine("id: {0}", data.reportSchedules[0].id);
            //Console.WriteLine("data: " + data);


            /*
            OTHER OPTIONS:

            // convert to string
            JArray array = JArray.Parse(data.reports);
            JArray sorted_data = new JArray(data.OrderBy(obj => (string)obj["completionTime"]));
            Console.WriteLine(sorted.ToString(Formatting.Indented));

            // sort data by completion time
            JArray sorted_data = new JArray(array1.OrderBy(obj => (string)obj["completionTime"]));
    		Console.WriteLine(sorted_data.ToString(Formatting.Indented));

            // pick elements from sorted data
            //var sorted_data = data.reports;
            //dynamic dynamic_sorted_data = JArray.Parse(sorted_data);
            dynamic test = sorted_data.First;
            //JObject sorted_data1 = new JObject(array.ToObject(sorted_data));
            //Console.WriteLine("reports: " + sorted_data);
            //JObject result = array.
            //Console.WriteLine("test: " + test);
            Console.WriteLine("type of sorted_data: " + sorted_data.GetType());
            Console.WriteLine("type of data: " + data.GetType());
            Console.WriteLine("type of data.reports[0]: " + data.reports[0].GetType());

            // try search item from a parsed JArray
            var jArrObject = JArray.Parse(response);
            var srchItem = jArrObject.SelectToken("$.[?(@.completionTime=='Mar')]");
            Console.WriteLine("srchItem: " + srchItem);

            // define report id
            //var ReportID = data.reports[0].id;
            var ReportID = test.id;
            Console.WriteLine("ReportID: " + ReportID);

            JArray sorted_data = new JArray(array1.OrderBy(obj => (string)obj["completionTime"]));
    		//Console.WriteLine(sorted_data.ToString(Formatting.Indented));

            //var sorted_data = data.reports;
            
            //dynamic dynamic_sorted_data = JArray.Parse(sorted_data);
            dynamic test = sorted_data.First;
            //array.ToObject<JArray<sorted_data>>();
            //JObject sorted_data1 = new JObject(array.ToObject(sorted_data));
            Console.WriteLine("type of sorted_data: " + sorted_data.GetType());
            Console.WriteLine("type of data: " + data.GetType());
            Console.WriteLine("type of data.reports[0]: " + data.reports[0].GetType());

            var jArrObject = JArray.Parse(response);
            var srchItem = jArrObject.SelectToken("$.[?(@.completionTime=='Mar')]");
            Console.WriteLine("srchItem: " + srchItem);
            


            // convert data to JArray
            // https://www.newtonsoft.com/json/help/html/m_newtonsoft_json_linq_jarray_parse.htm
            JArray array1 = data.reports;
            JArray array2 = array1.SelectTokens(string id);
            JToken array2 = data.SelectToken("$.reports[?(@.completionTime == '" + "Mar" + "')]");
            JToken value = data.SelectToken()
            JToken array2 = data.SelectToken("$.reports[?(@.completionTime == 'DayOfWeek.Tuesday')]");
             https://json-schema.org/understanding-json-schema/reference/regular_expressions.html
            JToken array2 = data.SelectToken("$.reports[?(@.completionTime =~ /Mar 06 /)]");
            
            */



            //string value = '/!?(Mar 08*)/g';
            // completion time value in the JSON response should include "MMM dd ", so I used regular expressions for filtering
            Regex regEx = new Regex("$.reports[?(@.completionTime =~ /" + month + " 01 */)]", RegexOptions.IgnoreCase);
            string regEx_string = regEx.ToString();
            JObject current_report_jobject = data.SelectToken(regEx_string);
            Console.WriteLine("current_report_jobject: " + current_report_jobject);
            // define report id
            var ReportID = current_report_jobject["id"];
            Console.WriteLine("ReportID: " + ReportID);
            //Console.WriteLine("type of ReportID: " + ReportID.GetType());
            //Console.WriteLine("type of current_report_jobject: " + current_report_jobject.GetType());
            //Console.WriteLine("regex: " + regEx.GetType());
            //Console.WriteLine("current_report_jobject: " + current_report_jobject);
            //Console.WriteLine("type of current_report_jobject: " + cuurent_JToken.GetType());

            /*
            // returns more than one value
            IEnumerable<JToken> array3 = data.SelectTokens("$.reports[?(@.completionTime >= 'Tue Mar 08 11:50:36 CET 2022')]");
            foreach (JToken item in array3)
            {
                Console.WriteLine(item);
            }
            */    
            
            // 2nd get request:
            // define the download URL from the reportID and download the CSV file to local storage
            var UrlGetReportNew = RequestConstants.UrlGetReport + "/" + ReportID + "/download?format=csv&_no_links=true";
            Console.WriteLine("url: " + RequestConstants.UrlGetReport);
            Console.WriteLine("url new: " + UrlGetReportNew);
            WebRequest req = WebRequest.Create(UrlGetReportNew);
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(RequestConstants.auth));
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            // display response on console
            //documentation: https://docs.microsoft.com/en-us/dotnet/api/system.net.webrequest?view=net-6.0
            //Console.WriteLine(resp.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = resp.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            resp.Close();
            
            // write reponse to local file
            File.WriteAllText(RequestConstants.path, responseFromServer);
            
            Console.WriteLine(respID);

            Console.ReadLine();
        }

        public static string GetId(IRequestHandler requestHandler)
        {
            return requestHandler.GetId(RequestConstants.UrlGetReport);

        }
    }
}