using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteBotnet3.Utils
{
    public class UserAgent
    {
        public static string Firefox()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:72.0) Gecko/20100101 Firefox/72.0";
        }

        public static string WaterfoxCurrent()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.0) Gecko/20100101 Firefox/68.0";
        }

        public static string WaterfoxClassic()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0 Waterfox/56.3";
        }

        public static string Basilik()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.9) Gecko/20100101 Goanna/4.4 Firefox/68.9 Basilisk/20200112";
        }

        public static string Chrome()
        {
            return "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
        }

        public static string Random()
        {
            string userAgent = string.Empty;
            switch (Program.Rand.Next(5))
            {
                case 0: userAgent = Firefox(); break;
                case 1: userAgent = WaterfoxCurrent(); break;
                case 2: userAgent = WaterfoxClassic(); break;
                case 3: userAgent = Basilik(); break;
                case 4: userAgent = Chrome(); break;
            }
            return userAgent;
        }
    }
}
