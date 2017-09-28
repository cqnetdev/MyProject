using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeChatVotesTestForZiXuan
{
    public class ProxyIpHelper
    {
        private static string address_xicidaili = "http://www.xicidaili.com/nn/{0}";
        private static string address_66ip = "http://www.66ip.cn/nmtq.php?getnum=20&isp=0&anonymoustype=0&start=&ports=&export=&ipaddress=&area=1&proxytype=1&api=66ip";
        private static string address_ip3366 = "http://www.ip3366.net/?stype=1&page={0}";

        private static string address_kuaidaili = "https://www.kuaidaili.com/free/inha/{0}/";
        /// <summary>
        /// 检查代理IP是否可用
        /// </summary>
        /// <param name="ipAddress">ip</param>
        /// <param name="success">成功的回调</param>
        /// <param name="fail">失败的回调</param>
        /// <returns></returns>
        public static async Task CheckProxyIpAsync(string ipAddress, Action success, Action<string> fail)
        {
            int index = ipAddress.IndexOf(":");
            string proxyIp = ipAddress.Substring(0, index);
            int proxyPort = int.Parse(ipAddress.Substring(index + 1));
            await Request.getAsync(proxyIp, proxyPort, 3000, "http://r94n9t.baolong.fuaqd.cn", () =>
            {
                success();
            }, (error) =>
            {
                fail(error);
            });
        }
        /// <summary>
        /// 从xicidaili.com网页上去获取代理IP，可以分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<string> GetXicidailiProxy(int page)
        {
            List<string> list = new List<string>();
            for (int p = 1; p <= page; p++)
            {
                string url = string.Format(address_xicidaili, p);
                Request.get(url, (docText) =>
                {
                    if (!string.IsNullOrWhiteSpace(docText))
                    {
                        
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(docText);
                        var trNodes = doc.DocumentNode.SelectNodes("//table[@id='ip_list']")[0].SelectNodes("./tr");
                        if (trNodes != null && trNodes.Count > 0)
                        {
                            for (int i = 1; i < trNodes.Count; i++)
                            {
                                var tds = trNodes[i].SelectNodes("./td");
                                string ipAddress = tds[1].InnerText + ":" + int.Parse(tds[2].InnerText); ;
                                list.Add(ipAddress);
                            }
                        }
                    }
                }, (error) =>
                {
                    Console.WriteLine(error);
                });
            }
            return list;
        }
        /// <summary>
        /// 从ip3366.net网页上去获取代理IP，可以分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<string> GetIp3366Proxy(int page)
        {
            List<string> list = new List<string>();
            for (int p = 1; p <= page; p++)
            {
                string url = string.Format(address_ip3366, p);
                Request.get(url, (docText) =>
                {
                    if (!string.IsNullOrWhiteSpace(docText))
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(docText);
                        var trNodes1 = doc.DocumentNode.SelectNodes("//table")[0];
                        var trNodes2 = doc.DocumentNode.SelectNodes("//table")[0].SelectSingleNode("//tbody");
                        var trNodes = doc.DocumentNode.SelectNodes("//table")[0].SelectSingleNode("//tbody").SelectNodes("./tr");
                        if (trNodes != null && trNodes.Count > 0)
                        {
                            for (int i = 1; i < trNodes.Count; i++)
                            {
                                var tds = trNodes[i].SelectNodes("./td");
                                if (tds[3].InnerHtml == "HTTPS")
                                {
                                    string ipAddress = tds[0].InnerText + ":" + int.Parse(tds[1].InnerText); ;
                                    list.Add(ipAddress);
                                }
                            }
                        }
                    }
                }, (error) =>
                {
                    Console.WriteLine(error);
                });
            }
            return list;
        }
        /// <summary>
        /// 从66ip.cn中去获取，不需要分页
        /// </summary>
        /// <returns></returns>
        public static List<string> Get66ipProxy()
        {
            List<string> list = new List<string>();
            Request.get(address_66ip,
            (docText) =>
            {
                int count = 0;
                if (string.IsNullOrWhiteSpace(docText) == false)
                {
                    string regex = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\:\\d{1,5}";
                    Match mstr = Regex.Match(docText, regex);
                    while (mstr.Success && count < 20)
                    {
                        string tempIp = mstr.Groups[0].Value;
                        list.Add(tempIp);
                        mstr = mstr.NextMatch();
                        count++;
                    }
                }
            },
            (error) =>
            {
                Console.WriteLine(error);
            });
            return list;
        }



        /// <summary>
        /// 从ip3366.net网页上去获取代理IP，可以分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<string> GetKuaidailiProxy(int page)
        {
            List<string> list = new List<string>();
            for (int p = 1; p <= page; p++)
            {
                string url = string.Format(address_kuaidaili, p);
                Request.get(url, (docText) =>
                {
                    if (!string.IsNullOrWhiteSpace(docText))
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(docText);
                        var trNodes1 = doc.DocumentNode.SelectNodes("//table")[0];
                        var trNodes2 = doc.DocumentNode.SelectNodes("//table")[0].SelectSingleNode("//tbody");
                        var trNodes = doc.DocumentNode.SelectNodes("//table")[0].SelectSingleNode("//tbody").SelectNodes("./tr");
                        if (trNodes != null && trNodes.Count > 0)
                        {
                            for (int i = 1; i < trNodes.Count; i++)
                            {
                                var tds = trNodes[i].SelectNodes("./td");
                                if (tds[3].InnerHtml == "HTTP")
                                {
                                    string ipAddress = tds[0].InnerText + ":" + int.Parse(tds[1].InnerText); ;
                                    list.Add(ipAddress);
                                }
                            }
                        }
                    }
                }, (error) =>
                {
                    Console.WriteLine(error);
                });
            }
            return list;
        }
    }
}
