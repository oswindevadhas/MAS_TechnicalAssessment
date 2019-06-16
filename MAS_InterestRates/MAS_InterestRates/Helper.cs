using MAS_InterestRates.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MAS_InterestRates
{
    public class Helper
    {
        private const string DateRangeFormat = "&between[end_of_month]={0},{1}";
        private const string LimitFormat = "&limit={0}";
        private const string OffsetFormat = "&offset={0}";
        private static readonly string _userAgent = "Microsoft Visual Studio Community 2017  / Version 15.9.9 Technical Assessment";

        public static async Task<Response> GetData(string address, string resourceId, string fromDate = null, string toDate = null, string limit = null, string offset = null)
        {
            string url = string.Concat(address, string.Format("resource_id={0}", resourceId));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                url = string.Concat(url, string.Format(DateRangeFormat, fromDate, toDate));
            if (!string.IsNullOrEmpty(limit))
                url = string.Concat(url, string.Format(LimitFormat, limit));
            if (!string.IsNullOrEmpty(offset))
                url = string.Concat(url, string.Format(OffsetFormat, offset));

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            var content = await client.GetStringAsync(url);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            Response res = JsonConvert.DeserializeObject<Response>(content, jsonSerializerSettings);

            return res;
        }

        public static void WriteToFile(string file, string content)
        {
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                sw.WriteLine(content);
            }
        }
    }
}
