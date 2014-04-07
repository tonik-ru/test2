using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WheelsScraper
{
	public class PageRetriever
	{
        public bool Googlebot { get; set; }
		public string ContentType { get; set; }
		public string Origin { get; set; }

		public PageRetriever()
		{
			cc = new CookieContainer();
			ContentType = "application/x-www-form-urlencoded";
		}

		protected CookieContainer cc;

		public List<ProxyInfo> Proxies { get; set; }

		public string Referer { get; set; }

		protected virtual void SetHeaders(HttpWebRequest r, bool useCookie = false)
		{
			r.KeepAlive = true;
			r.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			r.ContentType = ContentType;
			if(Googlebot)
				r.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
			else
				r.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";

			//if (Referer == null)
			//	r.Referer = r.RequestUri.ToString();
			r.Referer = Referer ?? r.RequestUri.ToString();

			r.Headers["Origin"] = Origin;
			
			r.Accept = "text/html, application/xhtml+xml, */*";

			if (useCookie)
				r.CookieContainer = cc;
			BugFix_CookieDomain(cc);
			OnSetHeaders(r);
		}

		protected void SetProxy(HttpWebRequest r)
		{
			var proxy = GetRandomProxy();
			if (proxy != null)
			{
				var myProxy = new WebProxy();
				var strP = proxy.Address;
				if (!strP.ToLower().StartsWith("http"))
					strP = "http://" + strP;
				Uri newUri = new Uri(strP);
				myProxy.Address = newUri;
				if (proxy.Login != null)
					myProxy.Credentials = new NetworkCredential(proxy.Login, proxy.Password);
				r.Proxy = myProxy;
			}
		}

		static Random rnd = new Random();

		public ProxyInfo GetRandomProxy()
		{
			if (Proxies == null)
				return null;
			if (Proxies.Count == 0)
				return null;
			int r = rnd.Next(Proxies.Count);
			var p = Proxies[r];
			return p;
		}

		public static byte[] GetFromServer(string url)
		{
			CookieContainer cc = new CookieContainer();
			var r = (HttpWebRequest)HttpWebRequest.Create(url);
			r.ContentType = "application/x-www-form-urlencoded";
			r.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.8.1.7) Gecko/20070914 Firefox/2.0.0.7";
			r.CookieContainer = cc;

			using (var resp = r.GetResponse())
			{
				var respStream = resp.GetResponseStream();
				using (var fs = new MemoryStream())
				{
					byte[] inBuf = new byte[resp.ContentLength];
					int bytesToRead = (int)inBuf.Length;
					int bytesRead = 0;
					while (true)
					{
						int n = respStream.Read(inBuf, 0, bytesToRead);
						if (n == 0)
							break;
						bytesRead += n;
						fs.Write(inBuf, 0, n);
					}
					fs.Flush();
					return fs.ToArray();
				}
			}
		}

		public void SaveFromServer(string url, string localPath)
		{
			SaveFromServer(url, localPath, false);
		}

		public void SaveFromServer(string url, string localPath, bool useCookie = false)
		{
			CookieContainer cc = new CookieContainer();
			var r = (HttpWebRequest)HttpWebRequest.Create(url);
			SetHeaders(r, useCookie);

			using (var resp = r.GetResponse())
			{
				var respStream = resp.GetResponseStream();
				using (var fs = File.OpenWrite(localPath))
				{
					byte[] inBuf = new byte[102400];
					int bytesToRead = (int)inBuf.Length;
					int bytesRead = 0;
					while (true)
					{
						int n = respStream.Read(inBuf, 0, bytesToRead);
						if (n == 0)
							break;
						bytesRead += n;
						fs.Write(inBuf, 0, n);
					}
					fs.Flush();
				}
			}
		}

		public void SaveFromServerWithPost(string url, string data, string localPath, bool useCookie = false)
		{
			CookieContainer cc = new CookieContainer();
			var r = (HttpWebRequest)HttpWebRequest.Create(url);
			SetHeaders(r, useCookie);

			r.Method = "POST";
			byte[] byteArray = Encoding.UTF8.GetBytes(data);
			r.ContentLength = byteArray.Length;
			var rS = r.GetRequestStream();
			rS.Write(byteArray, 0, byteArray.Length);
			rS.Close();

			using (var resp = r.GetResponse())
			{
				var respStream = resp.GetResponseStream();
				using (var fs = File.Create(localPath))
				{
					byte[] inBuf = new byte[102400];
					int bytesToRead = (int)inBuf.Length;
					int bytesRead = 0;
					while (true)
					{
						int n = respStream.Read(inBuf, 0, bytesToRead);
						if (n == 0)
							break;
						bytesRead += n;
						fs.Write(inBuf, 0, n);
					}
					fs.Flush();
				}
			}
		}

		public string GetRedirUrl(string url, bool useCookie, bool allowRedirect)
		{
			var r = (HttpWebRequest)HttpWebRequest.Create(url);
			SetHeaders(r, useCookie);
			SetProxy(r);
			r.AllowAutoRedirect = allowRedirect;

			using (var resp = (HttpWebResponse)r.GetResponse())
			{
				using (var respStream = resp.GetResponseStream())
				{
					var sr = new StreamReader(respStream, Encoding.UTF8);
					if (resp.Headers["location"] != null)
						return resp.Headers["location"];
				}
			}
			return url;
		}

		public string ReadFromServer(string url, bool useCookie = false)
		{
			return ReadFromServer(url, useCookie, false);
		}

		public string ReadFromServer(string url, bool useCookie, bool ignoreErrors)
		{
			//log4net.LogManager.GetLogger("").Debug("GET: " + url);
			//DateTime dt1 = new DateTime(2013, 2, 10);
			//if (DateTime.Now > dt1)
			//	throw new Exception("Error");

			var r = (HttpWebRequest)HttpWebRequest.Create(url);
			SetHeaders(r, useCookie);
			SetProxy(r);

			try
			{
				using (var resp = (HttpWebResponse)r.GetResponse())
				{
					if(useCookie)
						FixCookiesInResponse(resp);
					using (var respStream = resp.GetResponseStream())
					{
						var sr = new StreamReader(respStream, Encoding.UTF8);
						var html = sr.ReadToEnd();
						return html;
					}
				}
			}
			catch (System.Net.WebException ex)
			{
				var resp = (HttpWebResponse)ex.Response;
				if (ignoreErrors && resp != null)// && resp.StatusCode == HttpStatusCode.Forbidden)
				{
					using (var respStream = resp.GetResponseStream())
					{
						var sr = new StreamReader(respStream, Encoding.UTF8);
						var html = sr.ReadToEnd();
						return html;
					}
				}
				else
					throw;
			}
		}

		public event Action<HttpWebRequest, CookieContainer> AfterSetHeaders;
		protected void OnSetHeaders(HttpWebRequest req)
		{
			if (AfterSetHeaders != null)
				AfterSetHeaders(req, cc);
		}

		public string WriteToServer(string url, string data, bool useCookie = false, bool allowRedirect = true, bool readResponse = true)
		{
			//log4net.LogManager.GetLogger("").Debug("POST: DATA: " + data + " URL: " + url);
			var r = (HttpWebRequest)HttpWebRequest.Create(url);
			SetHeaders(r, useCookie);
			SetProxy(r);
			r.Method = "POST";
			r.AllowAutoRedirect = allowRedirect;
			//r.Referer = "http://www.prestigeautotechcorp.com/bsv-521.aspx";

			byte[] byteArray = Encoding.UTF8.GetBytes(data);
			r.ContentLength = byteArray.Length;
			var rS = r.GetRequestStream();
			//StreamWriter stOut = new StreamWriter(rS, System.Text.Encoding.ASCII);
			//stOut.Write(data);
			//stOut.Close();
			rS.Write(byteArray, 0, byteArray.Length);
			rS.Close();

			//IAsyncResult result = (IAsyncResult)r.BeginGetResponse(null, null);
			//result.AsyncWaitHandle.WaitOne();
			//using (var resp = r.EndGetResponse(result))

			using (var resp = (HttpWebResponse)r.GetResponse())
			{
				if(useCookie)
					FixCookiesInResponse(resp);
				string html = null;
				if (readResponse)
				{
					using (var respStream = resp.GetResponseStream())
					{
						var sr = new StreamReader(respStream, Encoding.UTF8);
						html = sr.ReadToEnd();
					}
				}
				else
				{
					html = resp.Headers["location"];
				}
				resp.Close();
				return html;
			}
		}

		
		private void BugFix_CookieDomain(CookieContainer cookieContainer)
		{
			var _ContainerType = cc.GetType();
			Hashtable table = (Hashtable)_ContainerType.InvokeMember("m_domainTable",
			System.Reflection.BindingFlags.NonPublic |
			System.Reflection.BindingFlags.GetField |
			System.Reflection.BindingFlags.Instance,
			null,
			cookieContainer,
			new object[] { });
			ArrayList keys = new ArrayList(table.Keys);
			foreach (string keyObj in keys)
			{
				string key = (keyObj as string);
				if (key[0] == '.')
				{
					string newKey = key.Remove(0, 1);
					table[newKey] = table[keyObj];
				}
			}
		}

		protected void FixCookiesInResponse(HttpWebResponse resp)
		{
			var cr = resp.GetResponseHeader("Set-cookie");
			if (cr != null)
			{
				var cSpl = cr.Split(',');
				string cStr = "";
				for (int i = 0; i < cSpl.Length; i++)
				{
					cStr = cSpl[i];
					if (i != cSpl.Length - 1 && cSpl[i + 1].StartsWith(" "))
					{
						cStr += cSpl[i + 1];
						i++;
					}
					var pSpl = cStr.Split(';');
					cStr = "";
					string domain = resp.ResponseUri.Host;
					foreach (var p2 in pSpl)
						if (p2.Contains("Domain="))
							domain = p2.Replace("Domain=", "").Trim();
					if(domain.StartsWith("."))
						domain = domain.Substring(1);
					var cVal = pSpl[0].Split('=');
					var p1 = pSpl[0].IndexOf('=');
					if (p1 > -1)
						cc.Add(new Uri(resp.ResponseUri.Scheme + "://" + domain), new System.Net.Cookie(pSpl[0].Substring(0, p1), pSpl[0].Substring(p1 + 1)));
				}
			}
		}

	}
}
