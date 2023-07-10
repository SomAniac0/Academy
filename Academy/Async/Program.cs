using BenchmarkDotNet.Running;

namespace Async
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var results = BenchmarkRunner.Run<ThreadTasks>();
            //ThreadTasks test = new ThreadTasks();


            //var test2 = await test.GetWebsitesAsync();
            //var test3 = test.GetWebsitesSync();



            //var asd = test.GetWebsiteContentParallel3();
            //var asd2 = test.GetWebsiteContentParallel10();


            //foreach (var item in asd) 
            //{
            //    Console.WriteLine(item.Length);
            //};

        }
    }
}