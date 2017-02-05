using System.Linq;
using GenFu;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using sharpcms.content.model;
using sharpcms.content.model.queries;
using sharpcms.content.queries;
using sharpcms.database;
using sharpcms.json;
using sharpcms.web.api;
using sharpcms.web.api.Controllers;

namespace sharpcms.api.tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ContentControllerTests
    {
        private TestServer _server;

        private JsonService _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Startup.Db = "content";

            ContentController.Db = "content";

            var serverBuilder = new WebHostBuilder();

            _server = new TestServer(serverBuilder.UseStartup<Startup>());

            var client = _server.CreateClient();

            _client = new JsonService(client);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            var db = new DbConnectionService();

            db.DeleteIfDoesExist(ContentController.Db);
        }

        [Test]
        public void Should_be_able_to_get_empty_result()
        {
            var result = _client.Get<ContentFragmentPagedQueryResultsModel>("/api/content").Result;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_insert()
        {
            var fragment = A.New<ContentFragmentModel>();

            var result = _client.Post<ContentFragmentModel>("/api/content", fragment).Result;

            Assert.That(result.Id, Is.GreaterThan(0));
        }

        [Test]
        public void Should_be_able_to_update()
        {
            var fragment = A.New<ContentFragmentModel>();

            var insertResult = _client.Post<ContentFragmentModel>("/api/content", fragment).Result;

            Assert.That(insertResult.Id, Is.GreaterThan(0));

            insertResult.Content = "Changed";

            var updatedResult = _client.Put<ContentFragmentModel>("/api/content", insertResult).Result;

            Assert.That(insertResult.Id, Is.EqualTo(updatedResult.Id));
        }

        [Test]
        public void Should_be_able_to_delete()
        {
            var fragment = A.New<ContentFragmentModel>();

            var insertResult = _client.Post<ContentFragmentModel>("/api/content", fragment).Result;

            Assert.That(insertResult.Id, Is.GreaterThan(0));

            insertResult.Content = "Changed";

            var deletedResult = _client.Delete<ContentFragmentModel>($"/api/content/{insertResult.Id}").Result;

            var result = _client.Get<ContentFragmentPagedQueryResultsModel>("/api/content").Result;

            Assert.That(result.Results.Any(x => x.Id == deletedResult.Id), Is.False);
        }

        [Test]
        public void Should_be_able_to_search()
        {
            var fragment = A.New<ContentFragmentModel>();

            var insertResult = _client.Post<ContentFragmentModel>("/api/content", fragment).Result;

            Assert.That(insertResult.Id, Is.GreaterThan(0));

            var query = QueryBuilder.New().WithContent(fragment.Content).Exact().Build();

            var queryResult = _client.Post<ContentFragmentPagedQueryResultsModel>("/api/content/q", query).Result;

            Assert.That(queryResult.Results.First(), Is.EqualTo(insertResult));
        }
    }
}
