using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpLearnings.src.Parallel_programing.Async_Await.AsyncAwaitExample
{
    public class Demo
    {
        // async-await example
        public static async Task<int> CalculateValueAsyncV2()
        {
            // Create a task on the thread pool which automatilly awaits 5000 ms
            await Task.Delay(5000);
            return 123;
        }

        // Next step
        public static Task<int> CalculateValueAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return 123;
            });
        }

        public static int BadCalculateValue()
        {
            Thread.Sleep(5000); // Blocking UI Thread
            return 123;
        }
        public static async void Run()
        {
            // Bad - block UI thrad
            // Console.WriteLine(BadCalculateValue());

            // 2nd implementation
            var calculation = CalculateValueAsync().ContinueWith(t =>
            {
                // Happens on the UI thread.
                return t.Result;
            });

            // await implementation:
            int value = await CalculateValueAsyncV2();

            await Task.Delay(5000);
            using (var wc = new WebClient())
            {
                string data = await wc.DownloadStringTaskAsync("http://google.com/robots.txt");

                Console.Write(data.Split("\n")[0].Trim());
            }

            Console.ReadKey();
        }
    }
}
