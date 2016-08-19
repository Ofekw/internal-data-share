using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coevolution.Models;

using Coevolution.Controllers;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        private String jsonString = "{\"Type\": \"node\",\"key\":\"The first thing created by the test!\"}";

        [TestMethod]
        public void TestMethod1()
        {
            var two = typeof(ItemsController).Assembly;
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:55426");
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}");
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            using (HttpClient client = new HttpClient())
            {
                server.OpenAsync().Wait();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:55426/api/items");
                var content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
                request.Content = content;
                using (request)
                using (HttpResponseMessage response = client.SendAsync(request).Result)
                {
                    var task = response.Content.ReadAsStringAsync();
                    task.Wait();
                    Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                }
                server.CloseAsync().Wait();
            };
        }
    }
}
