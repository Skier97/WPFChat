using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WPFChat
{
    class RequestClass
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Password { get; set; }

        public RequestClass(string url, int id = 0, string password = "")
        {
            this.Url = url;
            this.Id = id;
            this.Password = password;
        }

        public WebRequest MakeGetRequest(string url, int id = 0, string pass = "")
        {
            WebRequest req = WebRequest.Create(url + "?id=" + id + "&password=" + pass);
            return req;
        }

        public string MakeGetResponse(WebRequest req)
        {
            WebResponse resp = req.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string resultGet = sr.ReadToEnd();

                    return resultGet;
                }
            }
        }

        public void MakePostReq(WebRequest request, Message message)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            var jsonData = JsonConvert.SerializeObject(message);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            using (WebResponse response = request.GetResponse())
            {
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                }
            }
        }

        public string GetMessageUser(WebRequest req)
        {
            req.Method = "GET";
            req.ContentType = "application/x-www-urlencoded";

            using (WebResponse response = req.GetResponse())
            {
                using (Stream s = response.GetResponseStream())
                {
                    using (StreamReader r = new StreamReader(s))
                    {
                        return r.ReadToEnd();
                    }
                }
            }
        }
    }
}
