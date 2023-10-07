using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace CSLiba.Core
{
    public class Request
    {
        public Request(string adress)
        {
            this.adress = adress;
            _headers = new Dictionary<string, string>();
        }

        public void Get()
        {
            _request = (HttpWebRequest)WebRequest.Create(adress);
            _request.Method = "Get";
            _request.Proxy = Proxy;
            _request.Accept = Accept;
            _request.Host = Host;
            _request.Referer = Referer;
            _request.UserAgent = UserAgent;

            foreach (KeyValuePair<string, string> pair in _headers)
            {
                _request.Headers.Add(pair.Key, pair.Value);
            }

            HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                Response = new StreamReader(stream).ReadToEnd();

            }
        }

        public void Post()
        {
            _request = (HttpWebRequest)WebRequest.Create(adress);
            _request.Method = "Post";
            _request.Proxy = Proxy;
            _request.Accept = Accept;
            _request.Host = Host;
            _request.Referer = Referer;
            _request.UserAgent = UserAgent;
            _request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(Data))
            {
                byte[] sentData = Encoding.UTF8.GetBytes(Data);

                _request.ContentLength = sentData.Length;

                Stream sendStream = _request.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
            }

            foreach (KeyValuePair<string, string> pair in _headers)
            {
                _request.Headers.Add(pair.Key, pair.Value);
            }

            HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                Response = new StreamReader(stream).ReadToEnd();

            }
        }

        public void AddHeader(string key, string value)
        {
            _headers.Add(key, value);
        }

        public string Response { get; set; }
        public string Accept { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public string ContentType { get; set; }
        public string Data { get; set; }
        public WebProxy Proxy { get; set; }

        public CookieContainer container;
        public string adress;

        private Dictionary<string, string> _headers;
        private HttpWebRequest _request;
    }
}
