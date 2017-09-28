using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeChatVotesTestForZiXuan
{
    public class Request
    {
        /// <summary>
        /// 验证代理ip有效性
        /// </summary>
        /// <param name="proxyIp">代理IP</param>
        /// <param name="proxyPort">代理IP 端口</param>
        /// <param name="timeout">详情超时</param>
        /// <param name="url">请求的地址</param>
        /// <param name="success">成功的回调</param>
        /// <param name="fail">失败的回调</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task getAsync(string proxyIp, int proxyPort, int timeout, string url, Action success, Action<string> fail)
        {
            System.GC.Collect();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                //HttpWebRequest request = HttpWebRequest.CreateHttp(url);
                request.Timeout = timeout;
                request.KeepAlive = false;
                request.Proxy = new WebProxy(proxyIp, proxyPort);
                response = await request.GetResponseAsync() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    success();
                }
                else
                {
                    fail(response.StatusCode + ":" + response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                fail("请求异常" + ex.Message.ToString());
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 发起http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="success">成功的回调</param>
        /// <param name="fail">失败的回调</param>
        public static void get(string url, Action<string> success, Action<string> fail)
        {
            StreamReader reader = null;
            Stream stream = null;
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = WebRequest.Create(url);
                request.Timeout = 5000;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    stream = response.GetResponseStream();
                    reader = new StreamReader(stream);
                    string result = reader.ReadToEnd();
                    success(result);
                }
                else
                {
                    fail(response.StatusCode + ":" + response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                fail(ex.ToString());
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();
                if (response != null)
                    response.Close();
                if (request != null)
                    request.Abort();
            }
        }
    }
}
