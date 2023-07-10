using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Async
{
    [SimpleJob(RuntimeMoniker.Net70)]
    public class ThreadTasks
    {
        [Benchmark]
        public List<string> GetWebsitesSync()
        {
            List<string> result = new List<string>();
            WebClient client = new ();
            
            result.Add(client.DownloadString("https://www.rfc-editor.org/rfc/rfc2616"));
            result.Add(client.DownloadString("https://www.rfc-editor.org/rfc/rfc2822"));
            result.Add(client.DownloadString("https://www.rfc-editor.org/rfc/rfc1180"));

            return result;
        }

        [Benchmark]
        public async Task<int[]> GetWebsitesAsync()
        {
            var t1 = Task.Run(() => FetchAsync("https://www.rfc-editor.org/rfc/rfc2616"));
            var t2 = Task.Run(() => FetchAsync("https://www.rfc-editor.org/rfc/rfc2822"));
            var t3 = Task.Run(() => FetchAsync("https://www.rfc-editor.org/rfc/rfc1180"));

            var results = await Task.WhenAll(new[] { t1, t2, t3 });

            return results;
        }

        [Benchmark]
        public List<string> GetWebsiteContentParallel3()
        {
            List<string> websiteUrlList = GetWebsuteUrls();
            List<string> result = new();
            Parallel.ForEach(websiteUrlList, new ParallelOptions() { MaxDegreeOfParallelism = 3 }, url =>
            {
                WebClient client = new();
                result.Add(client.DownloadString(url));
            });
            return result;
        }

        [Benchmark]
        public List<string> GetWebsiteContentParallel10()
        {
            List<string> websiteUrlList = GetWebsuteUrls();
            List<string> result = new();
            Parallel.ForEach(websiteUrlList, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, url =>
            {
                WebClient client = new();
                result.Add(client.DownloadString(url));
            });
            return result;
        }

        static async Task<int> FetchAsync(string url)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return content.Length;
        }

        public List<string> GetWebsuteUrls()
        {
            List<string> websiteUrlList = new()
            {
                "https://www.rfc-editor.org/rfc/rfc2616",
                "https://www.rfc-editor.org/rfc/rfc2822",
                "https://www.rfc-editor.org/rfc/rfc1180"
            };
            return websiteUrlList;
        }
    }
}
