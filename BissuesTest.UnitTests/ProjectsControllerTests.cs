using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bissues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class ProjectsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ProjectsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Projects/Index")]
        [InlineData("/Projects/Edit")]/* OK because it redirects to login. */
        [InlineData("/Projects/Create")]/* OK because it redirects to login. */
        [InlineData("/Projects/Details/1")]
        [InlineData("/Projects/Details/1?currentIndex=2")]
        public async Task BaseTest(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Projects/Fail")]
        [InlineData("/Projects/Details")]
        [InlineData("/Projects/Details/999999")]
        public async Task NotFoundErrorTest(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound,response.StatusCode);
        }

        // [Fact]
        // public void Test1()
        // {

        // }
    }//END class ProjectsControllerTests
}