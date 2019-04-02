using System;
using System.Threading.Tasks;
using Splat;

namespace Splat203
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var resource = await BitmapLoader.Current.LoadFromResource("res:image", 300, 300);

            resource.Dispose();
        }
    }
}
