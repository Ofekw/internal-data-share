using System;
using System.Linq;
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
            using (var db = new ModelContext())
            {
                db.Items.RemoveRange(db.Items);
                db.Labels.RemoveRange(db.Labels);
                db.Notes.RemoveRange(db.Notes);
                db.SaveChanges();
            }

            var two = typeof(ItemsController).Assembly;
            config = new HttpSelfHostConfiguration("http://localhost:55426");
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
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
            var requestJson = "{\"Type\": \"node\",\"key\":\"Object to created by TestPost!\"}";

            HttpRequestMessage request = PostItem(requestJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                MatchOnce(result, "\"Id\": *[0-9]+");
                MatchOnce(result, "\"Key\": *\"Object to created by TestPost!\"");
                MatchOnce(result, "\"Type\": *\"node\"");
                MatchOnce(result, "\"Parent\": *null");
                MatchOnce(result, "\"Deleted\": *false");
                MatchOnce(result, "\"Labels\": *\\[\\]");
                MatchOnce(result, "\"Notes\": *\\[\\]");
                MatchOnce(result, "\"LeafChildren\": *\\[\\]");
                MatchOnce(result, "\"NodeChildren\": *\\[\\]");
                MatchOnce(result, "\"CreatedOn\": *\"[0123456789\\-T:]+\"");
                MatchOnce(result, "\"UpdatedOn\": *\"[0123456789\\-T:]+\"");
            }

        }

        [TestMethod]
        public void TestGetOne()
        {
            int createdId;

            var jsonString = "{\"Type\": \"node\",\"key\":\"Object to created by TestGetOne!\"}";

            String firstResult;

            HttpRequestMessage request = PostItem(jsonString);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                firstResult = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                createdId = int.Parse(Regex.Match(firstResult, "\"Id\": *([0-9]+)").Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:55426/api/items/" + createdId);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var secondResult = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                Assert.AreEqual(firstResult, secondResult);
            }

        }

        [TestMethod]
        public void TestGetAll()
        {
            int createdId;

            var jsonString = "{\"Type\": \"node\",\"key\":\"Object to created by TestGetAll!\"}";

            HttpRequestMessage request = PostItem(jsonString);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                createdId = int.Parse(Regex.Match(result, "\"Id\": *([0-9]+)").Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:55426/api/items/");

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                MatchOnce(result, "^\\[");
                MatchOnce(result, "\"Id\": *" + createdId);
                MatchOnce(result, "\"Key\": *\"Object to created by TestGetAll!\"");
                MatchOnce(result, "\\]$");

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

        private void MatchOnce(String phrase, String regex)
        {
            Assert.AreEqual(1, Regex.Matches(phrase, regex).Count);
        }
    }
}
