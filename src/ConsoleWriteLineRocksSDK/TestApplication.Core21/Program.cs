using ConsoleWriteLineRocksSDK;
using System;
using System.Threading;

namespace TestApplication.Core21
{
    class Programs
    {
        static void Main(string[] args)
        {
            var feedName = "jxmhac-test-core21";
            Console.WriteLine($"You can see Console.WriteLine() output at http://console.writeline.rocks/feed/{feedName}");
            Console.WriteLine("Open that in a browser and hit ENTER to continue");
            Console.ReadLine();

            Console.SetOut(ConsoleWriteLineRocks.Feed(feedName));

            Console.WriteLine("1. line: Hey!");
            Console.Write("2. line: How");
            Console.Write(" ");
            Console.Write("are you doing?");
            Console.WriteLine();
            Console.WriteLine("3. line: Not bad.");
            Console.Write(true);

            var i = 0;
            while (true)
            {
                Console.Write(i.ToString().PadLeft(10));
                i++;
                Thread.Sleep(200);
            }
        }
    }
}
