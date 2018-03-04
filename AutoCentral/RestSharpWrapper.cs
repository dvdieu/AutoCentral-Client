using Newtonsoft.Json;
using RestSharp;
using System;

namespace AutoCentral
{
    public class RestSharpWrapper
    {
        private static object syncRoot = new Object();
        private static volatile RestSharpWrapper instance;
        RestClient client = null;
       
        private RestSharpWrapper()
        {
            client = new RestClient();
        }

        public static RestSharpWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RestSharpWrapper();
                        }
                    }
                }            
                return instance;
            }
        }

        public string get(string url)
        {
            client.BaseUrl = new Uri(url);
            return client.Execute(new RestRequest()).Content;
        }

        public string post(string url, Object data) {
            client.BaseUrl = new Uri(ConfigCenter.Instance.getConfig("root-host"));
            RestRequest request = new RestRequest("play/confirm",Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(data), ParameterType.RequestBody);
            return client.Execute(request).Content;
        }
    }
}
