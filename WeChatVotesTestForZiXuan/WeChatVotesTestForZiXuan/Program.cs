using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeChatVotesTestForZiXuan
{
    class Program
    {
        public static List<string> proxyips = new List<string>();
        static void Main(string[] args)
        {
            bool UseProxy = ConfigurationManager.AppSettings["UseProxy"].ToString() == "1" ? true : false;
            if (UseProxy)
            {
                proxyips.Add("");
                Task t = Task.Run(() => GetProxyIPList());
            }
            int count = 1327;
            while (true)
            {
                if (DateTime.Now.Hour > 6 && DateTime.Now.Hour < 24)
                {
                    string result = string.Empty;
                    string validip = GetValidProxyIp();
                    result = GetVotesResult(validip);
                    if (result.Contains("成功"))
                    {
                        if (result.Contains("点赞成功"))
                        {
                            count++;
                        }
                        Console.WriteLine(result + "    |||当前已刷赞：" + count.ToString());
                        Random r = new Random();
                        System.Threading.Thread.Sleep(r.Next(9867, 72341));
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000 * 60 * 43);
                    }
                }
            }
        }

        public static async Task GetProxyIPList()
        {
            for (int i = 1; i < 3; i++)
            {
                List<string> currentPageProxys = ProxyIpHelper.GetIp3366Proxy(i);
                foreach (var item in currentPageProxys)
                {
                    await ProxyIpHelper.CheckProxyIpAsync(item, () =>
                    {
                        proxyips.Add(item);
                        System.Diagnostics.Debug.WriteLine(item);
                    }, (error) => { });
                }
            }
        }

        public static string GetValidProxyIp()
        {
            string retValue = string.Empty;
            if (proxyips != null && proxyips.Count > 1)
            {
                Random r = new Random();
                int currentProxy = r.Next(0, proxyips.Count - 1);
                retValue = proxyips[currentProxy];
            }
            return retValue;
        }


        public static string GetVotesResult(string ipAddress)
        {
            string result = string.Empty;
            var handler = new HttpClientHandler() { UseCookies = false };

            if (!string.IsNullOrEmpty(ipAddress))
            {
                Console.WriteLine("执行当前操作的代理IP为：" + ipAddress);
                int index = ipAddress.IndexOf(":");
                string proxyIp = ipAddress.Substring(0, index);
                int proxyPort = int.Parse(ipAddress.Substring(index + 1));
                WebProxy proxy = new WebProxy(proxyIp, proxyPort);
                handler.Proxy = proxy;
            }

            string url = ConfigurationManager.AppSettings["VoteUrl"].ToString();// "http://r94n9t.baolong.fuaqd.cn/app/index.php?i=2&c=entry&rid=911&id=95243&do=vote&m=tyzm_diamondvote";

            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Host", "r94n9t.baolong.fuaqd.cn");
            httpClient.DefaultRequestHeaders.Add("Connection", " keep-alive");
            //httpClient.DefaultRequestHeaders.Add("Content-Length", "0");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript; q=0.01");
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            httpClient.DefaultRequestHeaders.Add("Origin", "http://r94n9t.baolong.fuaqd.cn");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MicroMessenger/6.5.2.501 NetType/WIFI WindowsWechat QBCore/3.43.1021.400 QQBrowser/9.0.2524.400");
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", " XMLHttpRequest");
            httpClient.DefaultRequestHeaders.Add("Referer", "http://r94n9t.baolong.fuaqd.cn/app/index.php?i=2&c=entry&rid=911&id=95259&do=view&m=tyzm_diamondvote&wxref=mp.weixin.qq.com&wxref=mp.weixin.qq.com");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", " gzip, deflate");
            httpClient.Timeout = new TimeSpan(0, 0, 5);

            var TimeStamp = GetTimeStamp().ToString();
            var a1 = Guid.NewGuid().ToString().Replace("-", "");
            var a2 = Guid.NewGuid().ToString().Replace("-", "");

            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("Cookie", "PHPSESSID=" + a1 + "; Hm_lvt_" + a2 + "=" + TimeStamp + "; Hm_lpvt_" + a2 + "=" + TimeStamp);
            //var task = httpClient.PostAsync(new Uri(url), null);
            try
            {


                var task = httpClient.SendAsync(message);
                HttpResponseMessage response = task.Result;
                if (response != null)
                {
                    using (GZipStream gz = new GZipStream(response.Content.ReadAsStreamAsync().Result, CompressionMode.Decompress))
                    {
                        using (var resultStream = new MemoryStream())
                        {
                            gz.CopyTo(resultStream); UTF8Encoding utf8 = new UTF8Encoding();
                            result = utf8.GetString(resultStream.ToArray());
                            JObject obj = JObject.Parse(result);
                            if (obj != null)
                            {
                                result = obj["msg"].ToString() + "|||a1【" + a1 + "】  a2【" + a2 + "】";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                return "请求发送但是响应异常，不算成功！！！";
            }
            return result;
        }

        private static int GetTimeStamp()
        {
            DateTime dt = DateTime.Now;
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            return timeStamp;
        }


        private byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static string HttpPost(string url, string paramData, Dictionary<string, string> headerDic = null)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/x-www-form-urlencoded";
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(paramData);
                if (headerDic != null && headerDic.Count > 0)
                {
                    foreach (var item in headerDic)
                    {
                        wbRequest.Headers.Add(item.Key, item.Value);
                    }
                }
                using (Stream requestStream = wbRequest.GetRequestStream())
                {
                    using (StreamWriter swrite = new StreamWriter(requestStream))
                    {
                        swrite.Write(paramData);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sread = new StreamReader(responseStream))
                    {
                        result = sread.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            { }

            return result;
        }
    }
}
