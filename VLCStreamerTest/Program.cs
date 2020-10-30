using System;
using System.Linq;
using System.Threading.Tasks;

namespace VLCStreamerTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting app");
            var helper = new ChromeCastHelper();

            // hold on a bit at first to give libvlc time to find the chromecast
            await Task.Delay(2000);

            var chromecast = helper.RendererItems.FirstOrDefault();
            var result = helper.StartCasting(new Uri(""), chromecast);

            if (result)
            {
                Console.WriteLine("Successfully connected and streaming");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Failed to connect to chromecast");
            }
        }
    }
}
