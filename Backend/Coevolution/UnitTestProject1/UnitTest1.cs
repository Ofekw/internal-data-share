using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        private HttpSelfHostConfiguration config;
        private HttpSelfHostServer server;
        private HttpClient client;

        [TestInitialize]
        public void Setup()
        {
            var two = typeof(ItemsController).Assembly;
            config = new HttpSelfHostConfiguration("http://localhost:55426");
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            server = new HttpSelfHostServer(config);
            client = new HttpClient();
            server.OpenAsync().Wait();
        }

        [TestCleanup]
        public void TearDown()
        {
            server.CloseAsync().Wait();
            client.Dispose();
            server.Dispose();
        }

        [TestMethod]
        public void TestPost()
        {
            var jsonString = "{\"Type\": \"node\",\"key\":\"Object to created by TestPost!\"}";

            var pattern = "^\\{\"Children\":\\[\\],\"Id\":([0-9]*),"
                            + "\"Key\":\"Object to created by TestPost!\","
                            + "\"Parent\":null,"
                            + "\"Date\":.*,"
                            + "\"Deleted\":false,"
                            + "\"Labels\":\\[\\],"
                            + "\"Notes\":\\[\\]\\}$";

            HttpRequestMessage request = PostItem(jsonString);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);

                var matchObject = Regex.Match(result, pattern);

                Assert.AreNotEqual(matchObject.Value, String.Empty);
            }

        }

        [TestMethod]
        public void TestGetOne()
        {
            int createdId;

            var jsonString = "{\"Type\": \"node\",\"key\":\"Object to created by TestGetOne!\"}";

            var pattern = "^\\{\"Children\":\\[\\],\"Id\":([0-9]*),"
                            + "\"Key\":\"Object to created by TestGetOne!\","
                            + "\"Parent\":null,"
                            + "\"Date\":.*,"
                            + "\"Deleted\":false,"
                            + "\"Labels\":\\[\\],"
                            + "\"Notes\":\\[\\]\\}$";

            HttpRequestMessage request = PostItem(jsonString);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);

                var matchObject = Regex.Match(result, pattern);

                Assert.AreNotEqual(matchObject.Value, String.Empty);

                createdId = int.Parse(matchObject.Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:55426/api/items/" + createdId);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

                var matchObject = Regex.Match(result, pattern);

                Assert.AreNotEqual(matchObject.Value, String.Empty);

                Assert.AreEqual(int.Parse(matchObject.Groups[1].Value), createdId);
            }

        }

        [TestMethod]
        public void TestGetAll()
        {
            int createdId;

            var jsonString = "{\"Type\": \"node\",\"key\":\"Object to created by TestGetAll!\"}";

            var patternOne = "^\\{\"Children\":\\[\\],\"Id\":([0-9]*),"
                            + "\"Key\":\"Object to created by TestGetAll!\","
                            + "\"Parent\":null,"
                            + "\"Date\":.*,"
                            + "\"Deleted\":false,"
                            + "\"Labels\":\\[\\],"
                            + "\"Notes\":\\[\\]\\}$";

            HttpRequestMessage request = PostItem(jsonString);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);

                var matchObject = Regex.Match(result, patternOne);

                Assert.AreNotEqual(matchObject.Value, String.Empty);

                createdId = int.Parse(matchObject.Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:55426/api/items/");
			
            var patternTwo = "\\{\"Children\":\\[\\],\"Id\":" + createdId + ","
                            + "\"Key\":\"Object to created by TestGetAll!\","
                            + "\"Parent\":null,"
                            + "\"Date\":.*,"
                            + "\"Deleted\":false,"
                            + "\"Labels\":\\[\\],"
                            + "\"Notes\":\\[\\]\\}";

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

                var matchObject = Regex.Match(result, patternTwo);

                Assert.AreNotEqual(matchObject.Value, String.Empty);
            }

        }

        private HttpRequestMessage PostItem(String jsonString)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:55426/api/items");
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            request.Content = content;
            return request;
        }
    }
}
