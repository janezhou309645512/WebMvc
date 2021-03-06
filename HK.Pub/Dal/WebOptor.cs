﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HK.Pub.Dal
{
    public class WebOptor
    {
        static Log log = new Log();
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        static public string UrlEncode(string context)
        {
            return System.Web.HttpUtility.UrlEncode(context);
        }
        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                log.Error("Exception message: " + e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                log.Error("HttpService " + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    log.Error("HttpService StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    log.Error("HttpService StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw e;
            }
            catch (Exception e)
            {
                log.Error("HttpService  " + e.ToString());
                throw e;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        /// <summary>
        /// 处理http Post请求，提交Json数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="json">提交的数据</param>
        /// <returns></returns>
        public static string PostJson(string url, string json)
        {
            return Post(url, "application/json", json, 20);
        }
        /// <summary>
        /// 处理http Post请求，提交Xml数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="xml">提交的数据</param>
        /// <returns>请求成功后 返回数据，失败抛出异常</returns>
        public static string PostXml(string url, string xml)
        {
            return Post(url, "text/xml", xml, 20);
        }
        /// <summary>
        /// 处理http post 请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="method">提交数据的类型</param>
        /// <param name="context">提交的数据</param>
        /// <param name="timeout">超时的时间</param>
        /// <returns>请求成功后 返回数据，失败抛出异常</returns>
        public static string Post(string url, string method, string context,  /*bool isUseCert, */int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = method;// "text/xml"; application/json
                byte[] data = System.Text.Encoding.UTF8.GetBytes(context);
                request.ContentLength = data.Length;

                //是否使用证书
                //if (isUseCert)
                //{
                //    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                //    X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
                //    request.ClientCertificates.Add(cert);
                //    Log.Debug("WxPayApi", "PostXml used cert");
                //}

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                //Log.Error("Exception message: {0}", e.Message);
                //HK.IO.Log.Ins.Write("HttpService:Thread - caught ThreadAbortException - resetting.");
                log.Error(string.Format("Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                log.Error("HttpService  " + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    log.Error("HttpService  StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    log.Error("HttpService  StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw e;
            }
            catch (Exception e)
            {
                log.Error("HttpService  " + e.Message);
                throw e;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        static public string GetCurrentRequestHtml()
        {
            Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
            byte[] requestByte = new byte[requestStream.Length];
            requestStream.Read(requestByte, 0, (int)requestStream.Length);
            string requestStr = Encoding.UTF8.GetString(requestByte);
            return requestStr;
        }
    }
}
