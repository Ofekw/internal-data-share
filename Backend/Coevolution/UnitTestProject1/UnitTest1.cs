using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coevolution.Models;

using Coevolution.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        private HttpSelfHostConfiguration config;
        private HttpSelfHostServer server;
        private HttpClient client;

        private String address = "http://localhost:55426";

        [TestInitialize]
        public void Setup()
        {
            using (var db = new ModelContext())
            {
                db.Items.RemoveRange(db.Items.Where(x => x.Id > 8));
                db.Labels.RemoveRange(db.Labels.Where(x => x.Id > 6));
                db.Notes.RemoveRange(db.Notes.Where(x => x.Id > 6));
                db.SaveChanges();
            }

            var two = typeof(ItemsController).Assembly;
            config = new HttpSelfHostConfiguration(address);
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

                MatchOnce(result, "\"Id\":\\s*[0-9]+");
                MatchOnce(result, "\"Key\":\\s*\"Object to created by TestPost!\"");
                MatchOnce(result, "\"Type\":\\s*\"node\"");
                MatchOnce(result, "\"Parent\":\\s*null");
                MatchOnce(result, "\"Deleted\":\\s*false");
                MatchOnce(result, "\"Labels\":\\s*\\[\\]");
                MatchOnce(result, "\"Notes\":\\s*\\[\\]");
                MatchOnce(result, "\"LeafChildren\":\\s*\\[\\]");
                MatchOnce(result, "\"NodeChildren\":\\s*\\[\\]");
                MatchOnce(result, "\"CreatedOn\":\\s*\"[0123456789\\-T:]+\"");
                MatchOnce(result, "\"UpdatedOn\":\\s*\"[0123456789\\-T:]+\"");
            }

        }

        [TestMethod]
        public void TestPostLabel()
        {
            var requestJson = "{\"Content\": \"Label to be created by TestPostLabel!\"}";
            HttpRequestMessage request = PostLabel(requestJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                MatchOnce(result, "\"Id\":\\s*[0-9]+");
                MatchOnce(result, "\"Content\":\\s*\"Label to be created by TestPostLabel!\"");
            }
        }

        [TestMethod]
        public void TestGetAllLabels()
        {
            int labelID;
            var requestJson = "{\"Content\": \"Label to be created by TestGetAllLabels!\"}";
            HttpRequestMessage request = PostLabel(requestJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                labelID = int.Parse(Regex.Match(result, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/labels/");

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                MatchOnce(result, "^\\[");
                MatchOnce(result, "\"Id\":\\s*" + labelID);
                Assert.IsTrue(1 < Regex.Matches(result, "\"Items\":\\s*\\[\\]").Count);
                MatchOnce(result, "\"Content\":\\s*\"Label to be created by TestGetAllLabels!\"");
                MatchOnce(result, "\\]$");

            }

        }

        [TestMethod]
        public void TestDeleteLabel()
        {
            int labelID;
            var requestJson = "{\"Content\": \"Label to be created by TestGetAllLabels!\"}";
            HttpRequestMessage request = PostLabel(requestJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                labelID = int.Parse(Regex.Match(result, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Delete, address + "/api/labels/" + labelID);

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }

            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/labels/");

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Regex label = new Regex("\"Id\":\\s*" + labelID);
                Assert.IsFalse(label.Match(result).Success);
            }
        }

        [TestMethod]
        public void TestPostChild()
        {
            int parentId;
            int childId;

            var parentJson = "{\"Type\": \"node\",\"key\":\"Parent to created by TestPostChild!\"}";

            String firstResult;

            // Create parent
            HttpRequestMessage request = PostItem(parentJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                firstResult = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                parentId = int.Parse(Regex.Match(firstResult, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            // Create child
            var childJson = "{\"Type\": \"node\",\"key\":\"Child to created by TestPostChild!\",\"Parent\":" + parentId + "}";
            request = PostItem(childJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var secondResult = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                MatchOnce(secondResult, "\"Parent\":\\s*" + parentId);

                childId = int.Parse(Regex.Match(secondResult, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            // Check parent has child
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + parentId);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var thirdResult = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                Regex nodeChildrenRegex = new Regex("\"NodeChildren\":\\s*\\[(\\{[^\\}]*\\})\\]");
                var child = nodeChildrenRegex.Match(thirdResult).Groups[1].Value;
                MatchOnce(child, "\"Id\":\\s*" + childId);
            }

            // Check child doesn't show in top level.
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/");
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var fourthResult = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Regex regChild = new Regex("\"Id\":\\s*" + childId);
                Assert.IsFalse(regChild.Match(fourthResult).Success);

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

                createdId = int.Parse(Regex.Match(firstResult, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + createdId);
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

                createdId = int.Parse(Regex.Match(result, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/");

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var result = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                MatchOnce(result, "^\\[");
                MatchOnce(result, "\"Id\":\\s*" + createdId);
                MatchOnce(result, "\"Key\":\\s*\"Object to created by TestGetAll!\"");
                MatchOnce(result, "\\]$");

            }

        }

        [TestMethod]
        public void TestPut()
        {
            int createdId;

            var jsonString = "{\"Type\": \"node\",\"key\":\"Object to created by TestPut!\"}";

            String firstResult;

            HttpRequestMessage request = PostItem(jsonString);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                firstResult = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                createdId = int.Parse(Regex.Match(firstResult, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }


            Regex reg = new Regex("Object to created by TestPut!");
            var newJson = reg.Replace(firstResult, "Object to edited by TestPut!");

            request = PutItem(createdId, newJson);
            Assert.AreEqual(HttpStatusCode.OK, GetRequestStatus(request));

            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + createdId);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var secondResult = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Regex reg2 = new Regex("Object to edited by TestPut!");
                Assert.IsTrue(reg2.Match(secondResult).Success);
            }

        }

        [TestMethod]
        public void TestDelete()
        {
            int parentId;
            int childId;

            var parentJson = "{\"Type\": \"node\",\"key\":\"Parent to created by TestDelete!\"}";

            String firstResult;

            // Posting the main item
            HttpRequestMessage request = PostItem(parentJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                firstResult = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                parentId = int.Parse(Regex.Match(firstResult, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            var childJson = "{\"Type\": \"node\",\"key\":\"Child to created by TestDelete!\",\"Parent\":" + parentId + "}";

            // Posting the child item
            request = PostItem(childJson);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var secondResult = task.Result;

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                MatchOnce(secondResult, "\"Parent\":\\s*" + parentId);

                childId = int.Parse(Regex.Match(secondResult, "\"Id\":\\s*([0-9]+)").Groups[1].Value);
            }

            // Getting the main item should tell us it is there
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + parentId);
            Assert.AreEqual(HttpStatusCode.OK, GetRequestStatus(request));

            // Delete the main item
            request = new HttpRequestMessage(HttpMethod.Delete, address + "/api/items/" + parentId);
            Assert.AreEqual(HttpStatusCode.OK, GetRequestStatus(request));

            // Getting the main item should tell us it isn't there
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + parentId);
            Assert.AreEqual(HttpStatusCode.NotFound, GetRequestStatus(request));

            // Getting the child item should tell us it isn't there
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + childId);
            Assert.AreEqual(HttpStatusCode.NotFound, GetRequestStatus(request));

            // Getting the main item should tell us it is there if we ask for deleted items
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + parentId + "?showDeleted=true");
            Assert.AreEqual(HttpStatusCode.OK, GetRequestStatus(request));

            // Getting the child item should tell us it is there if we ask for deleted items
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/" + childId + "?showDeleted=true");
            Assert.AreEqual(HttpStatusCode.OK, GetRequestStatus(request));

            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/");

            // Getting everything should tell us the main item isn't there
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var thirdResult = task.Result;
                Regex regParent = new Regex("\"Id\":\\s*" + parentId);
                Assert.IsFalse(regParent.Match(thirdResult).Success);

                Regex regChild = new Regex("\"Id\":\\s*" + childId);
                Assert.IsFalse(regChild.Match(thirdResult).Success);

            }

            // Getting everything should tell us the main item is there if we ask for deleted items
            request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items?showDeleted=true");

            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var fourthResult = task.Result;
                Regex reg = new Regex("\"Id\":\\s*" + parentId);
                Assert.IsTrue(reg.Match(fourthResult).Success);

            }
        }

        [TestMethod]
        public void TestNotFound()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/api/items/0");
            Assert.AreEqual(HttpStatusCode.NotFound, GetRequestStatus(request));

            request = PutItem(0, "{\"id\":\"0\", \"Type\":\"node\"}");
            Assert.AreEqual(HttpStatusCode.NotFound, GetRequestStatus(request));

            request = new HttpRequestMessage(HttpMethod.Delete, address + "/api/items/0");
            Assert.AreEqual(HttpStatusCode.NotFound, GetRequestStatus(request));
        }

        [TestMethod]
        public void TestBadRequest()
        {

            var request = PostItem("");
            Assert.AreEqual(HttpStatusCode.BadRequest, GetRequestStatus(request));

            request = PostItem("{\"Type\":\"node\"}");
            Assert.AreEqual(HttpStatusCode.BadRequest, GetRequestStatus(request));

            request = PostItem("{\"key\":\"this should fail\"}");
            Assert.AreEqual(HttpStatusCode.BadRequest, GetRequestStatus(request));

            request = PutItem(0, "");
            Assert.AreEqual(HttpStatusCode.BadRequest, GetRequestStatus(request));

            request = PutItem(0, "{\"id\":3}");
            Assert.AreEqual(HttpStatusCode.BadRequest, GetRequestStatus(request));
        }



        [TestMethod]
        public void TestSearchNodeKey()
        {

            String result;

            HttpRequestMessage request = SearchKey("Kiwibank");
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                result = task.Result;
                var item = JsonConvert.DeserializeObject<List<DtoSearchItem>>(result)[0];
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(1, item.Id);
                Assert.AreEqual(false, item.IsLeaf);
                Assert.AreEqual(null, item.Value);
                Assert.AreEqual("KiwiBank", item.Key);
                Assert.AreEqual(1, item.Path[0].Key);
            }
        }

        [TestMethod]
        public void TestSearchLeafKey()
        {

            String result;

            HttpRequestMessage request = SearchKey("Login");
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                result = task.Result;
                var item = JsonConvert.DeserializeObject<List<DtoSearchItem>>(result)[0];
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                //Actual node id is 7, but return parent because its a leaf
                Assert.AreEqual(6, item.Id);
                Assert.AreEqual(true, item.IsLeaf);
                Assert.AreEqual("Login", item.Key);
                Assert.AreEqual("Admin", item.Value);
                //two parents
                Assert.AreEqual(1, item.Path[0].Key);
                Assert.AreEqual(6, item.Path[1].Key);
            }
        }

        [TestMethod]
        public void TestSearchLeafValue()
        {

            String result;
            HttpRequestMessage request = SearchValue("testEmail");
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                result = task.Result;
                var item = JsonConvert.DeserializeObject<List<DtoSearchItem>>(result)[0];
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                //Actual node id is 7, but return parent because its a leaf
                Assert.AreEqual(1, item.Id);
                Assert.AreEqual(true, item.IsLeaf);
                Assert.AreEqual("Email", item.Key);
                Assert.AreEqual("example@testEmail.com", item.Value);
                Assert.AreEqual(1, item.Path[0].Key);
            }
        }

        [TestMethod]
        public void TestSearchLabel()
        {

            String result;
            HttpRequestMessage request = SearchLabel(1);
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                result = task.Result;
                var items = JsonConvert.DeserializeObject<List<DtoSearchItem>>(result);
                var item = items[0];
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(3, items.Count);
                Assert.AreEqual(1, item.Id);
                Assert.AreEqual(false, item.IsLeaf);
                Assert.AreEqual(null, item.Value);
                Assert.AreEqual("KiwiBank", item.Key);
                Assert.AreEqual(1, item.Path[0].Key);
            }
        }

        private HttpRequestMessage SearchKey(string query)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, address + "/api/Items/Search/Key/" + query);
            return request;
        }

        private HttpRequestMessage SearchValue(string query)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, address + "/api/Items/Search/Value/" + query);
            return request;
        }

        private HttpRequestMessage SearchLabel(int labelId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, address + "/api/Items/Search/Label/" + labelId);
            return request;
        }


        private HttpRequestMessage PostLabel(String jsonString)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, address + "/api/labels");
            PopulateBody(request, jsonString);
            return request;
        }

        private HttpRequestMessage PostItem(String jsonString)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, address + "/api/items");
            PopulateBody(request, jsonString);
            return request;
        }

        private HttpRequestMessage PutItem(int id, String jsonString)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, address + "/api/items/" + id);
            PopulateBody(request, jsonString);
            return request;
        }

        private HttpStatusCode GetRequestStatus(HttpRequestMessage request)
        {
            using (request)
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();

                return response.StatusCode;
            }
        }

        private void PopulateBody(HttpRequestMessage request, String jsonString)
        {
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            request.Content = content;
            return;
        }

        private void MatchOnce(String phrase, String regex)
        {
            Assert.AreEqual(1, Regex.Matches(phrase, regex).Count);
        }
    }
}
