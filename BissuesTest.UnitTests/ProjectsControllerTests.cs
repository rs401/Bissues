using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class ProjectsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private ProjectsController _sut;

        public ProjectsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index_ReturnsAView_WithListOfProjects()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Insert seed data into the database using one instance of the context
            // using (var context = new ApplicationDbContext(options))
            // {
            //     context.Movies.Add(new Movie {Id = 1, Title = "Movie 1", YearOfRelease = 2018, Genre = "Action"});
            //     context.Movies.Add(new Movie {Id = 2, Title = "Movie 2", YearOfRelease = 2018, Genre = "Action"});
            //     context.Movies.Add(new Movie {Id = 3, Title = "Movie 3", YearOfRelease = 2019, Genre = "Action"});
            //     context.SaveChanges();
            // }
            
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Project>>(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public async Task Details_ReturnsAView_WithProjectsDetailViewModel()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Insert seed data into the database context
            using (var context = new ApplicationDbContext(options))
            {
                context.Projects.Add(new Project {Id = 1, Name = "Test Project 1", Description = "Test Project 1 Description"});
                context.SaveChanges();
            }
            
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Details(1,1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<ProjectsDetailViewModel>(viewResult.ViewData.Model);
            }
        }

        // [Theory]
        // [InlineData("/Projects/Index")]
        // [InlineData("/Projects/Edit")]/* OK because it redirects to login. */
        // [InlineData("/Projects/Create")]/* OK because it redirects to login. */
        // [InlineData("/Projects/Details/1")]
        // [InlineData("/Projects/Details/1?currentIndex=2")]
        // public async Task BaseTest(string url)
        // {
        //     // Arrange
        //     var client = _factory.CreateClient();

        //     // Act
        //     var response = await client.GetAsync(url);

        //     // Assert
        //     response.EnsureSuccessStatusCode(); // Status Code 200-299
        //     Assert.Equal("text/html; charset=utf-8",
        //         response.Content.Headers.ContentType.ToString());
        // }

        // [Theory]
        // [InlineData("/Projects/Fail")]
        // [InlineData("/Projects/Details")]
        // [InlineData("/Projects/Details/999999")]
        // public async Task NotFoundErrorTest(string url)
        // {
        //     // Arrange
        //     var client = _factory.CreateClient();

        //     // Act
        //     var response = await client.GetAsync(url);

        //     // Assert
        //     Assert.Equal(System.Net.HttpStatusCode.NotFound,response.StatusCode);
        // }

        // [Fact]
        // public void Test1()
        // {

        // }
    }//END class ProjectsControllerTests
}