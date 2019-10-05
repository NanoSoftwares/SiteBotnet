using System;
using System.Collections.Generic;
using Leaf.xNet;
using System.IO;
using Leaf.xNet.Services.Cloudflare;
using System.Net.NetworkInformation;
using System.Threading;
using ConfigParser;

namespace SiteBotnet3
{
    class Program
    {
        private static string AppName = "SiteBotnet";
        private static string AppVersion = "3.1";

        static void Main(string[] args)
        {
            Parser p = new Parser("config.txt");
            if (!File.Exists("proxies.txt")) File.WriteAllText("proxies.txt", "");

            string Target = p.ReadConfig("Target").ToString().Split('=')[1].Replace(" ", "");
            string Keyword = p.ReadConfig("Keyword").ToString().Split('=')[1].Replace(" ", "");
            int Threads = Convert.ToInt32(p.ReadConfig("Threads").ToString().Split('=')[1].Replace(" ", ""));
            string ProxyType = p.ReadConfig("ProxyType").ToString().Split('=')[1].Replace(" ", "");
            List<string> ProxyList = new List<string>(File.ReadAllLines("proxies.txt"));

            Console.Title = AppName + " v" + AppVersion;

            for (int i = 0; i < Threads; i++)
            {
                Requests req = new Requests("[ThreadHttp-" + i + "]");
                req.SetTarget(Target);
                req.SetKeyword(Keyword);
                req.SetThreads(Threads);
                req.SetProxyType(ProxyType);
                req.SetProxyList(ProxyList);
                req.Start();
            }

            return;
        }
    }
}
