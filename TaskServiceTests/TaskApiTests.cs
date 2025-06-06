using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;

namespace TaskServiceTests
{
    public class TaskApiTests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseContentRoot(@"C:\Users\anewt\Source\Repos\MyEvilLittlePony\TaskService");

            });
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void Teardown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task TaskLifecycle_ShouldReturnCorrectStatuses()
        {
            var postResponse = await _client.PostAsync("/task", null);
            Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

            var postJson = await postResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = postJson.GetProperty("id").GetGuid();

            var getResponse1 = await _client.GetAsync($"/task/{id}");
            Assert.That(getResponse1.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var json1 = await getResponse1.Content.ReadFromJsonAsync<JsonElement>();
            var status1 = json1.GetProperty("status").GetString();
            Assert.That(status1, Is.AnyOf("Created", "Running"));

            await Task.Delay(TimeSpan.FromSeconds(125));

            var getResponse2 = await _client.GetAsync($"/task/{id}");
            Assert.That(getResponse2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var json2 = await getResponse2.Content.ReadFromJsonAsync<JsonElement>();
            var status2 = json2.GetProperty("status").GetString();
            Assert.That(status2, Is.EqualTo("Finished"));
        }

        [Test]
        public async Task GetTask_InvalidGuid_ShouldReturn400()
        {
            var response = await _client.GetAsync("/task/not-a-guid");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetTask_NonExistingGuid_ShouldReturn404()
        {
            var guid = Guid.NewGuid();
            var response = await _client.GetAsync($"/task/{guid}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

}