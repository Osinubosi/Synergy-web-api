using Newtonsoft.Json;
using Synergy.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.Domain.Implementation
{
    public class HttpService : IHttpService
    {
        public async Task<HttpResponseMessage> GetAsync(string baseUrl, string referenceUrl, string authorizationToken, HttpMessageHandler messageHandler = null, string authenticationScheme = "Bearer")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient())
            {

                if (!string.IsNullOrEmpty(authorizationToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"{authenticationScheme} {authorizationToken}");

                client.BaseAddress = new Uri(baseUrl);
                var request = await client.GetAsync(referenceUrl);
                return request;

            }
        }

        public async Task<HttpResponseMessage> GetAsync(string baseUrl, string referenceUrl, List<KeyValuePair<string, string>> headers = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                if (headers != null)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    headers.ForEach(h => { client.DefaultRequestHeaders.Add(h.Key, h.Value); });

                }
                var request = await client.GetAsync(referenceUrl);
                return request;
            }
        }

        public async Task<HttpResponseMessage> PostAsync<T>(T model, string baseUrl, string authorizationToken, string referenceUrl, HttpMessageHandler messageHandler = null, string authenticationScheme = "Bearer")
        {
            messageHandler = messageHandler ?? new HttpClientHandler();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
     (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(messageHandler))
            {

                StringContent content;
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(authorizationToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"{authenticationScheme} {authorizationToken}");


                var jsonObject = string.Empty;
                if (model != null)
                {
                    jsonObject = JsonConvert.SerializeObject(model);
                }
                content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var request = await client.PostAsync(referenceUrl, content);

                return request;
            }
        }

        public async Task<HttpResponseMessage> PostAsync<T>(T model, string baseUrl, string referenceUrl, List<KeyValuePair<string, string>> headers = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
     (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient())
            {
                StringContent content;
                client.BaseAddress = new Uri(baseUrl);
                // ReSharper disable once AccessToDisposedClosure
                headers?.ForEach(h => { client.DefaultRequestHeaders.Add(h.Key, h.Value); });


                // client.DefaultRequestHeaders.Add("AppId", Constants.SwitchAppId);
                var jsonObject = string.Empty;
                if (model != null)
                {
                    jsonObject = JsonConvert.SerializeObject(model);
                }
                content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var request = await client.PostAsync(referenceUrl, content);

                return request;
            }
        }

        public async Task<HttpResponseMessage> PostEncrytionAsync(string model, string baseUrl, string referenceUrl, List<KeyValuePair<string, string>> headers = null, HttpMessageHandler messageHandler = null, string authenticationScheme = "Bearer")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
     (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient())
            {
                StringContent content;
                client.BaseAddress = new Uri(baseUrl);
                if (headers != null)
                {
                    headers.ForEach(h =>
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });
                }

                content = new StringContent(model, Encoding.UTF8, "application/json");

                var request = await client.PostAsync(referenceUrl, content);

                return request;
            }
        }

        public async Task<HttpResponseMessage> SendAsync<T>(HttpMethod httpMethod, T model, string baseUrl, string referenceUrl, List<KeyValuePair<string, string>> headers, string authorizationToken, HttpMessageHandler messageHandler = null, string authenticationScheme = "Bearer")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
     (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient())
            {
                StringContent content;

                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(authorizationToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"{authenticationScheme} {authorizationToken}");
                // ReSharper disable once AccessToDisposedClosure
                headers.ForEach(h => { client.DefaultRequestHeaders.Add(h.Key, h.Value); });

                var requestMessage = new HttpRequestMessage(httpMethod, referenceUrl);

                var jsonObject = string.Empty;
                if (model != null)
                {
                    jsonObject = JsonConvert.SerializeObject(model);
                }
                content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                requestMessage.Content = content;

                var request = await client.SendAsync(requestMessage);

                return request;
            }
        }
    }
}
