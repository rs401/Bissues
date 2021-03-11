using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private HomeController _sut;
        public HomeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index_ReturnsAView_WithIndexViewModel()
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
                _sut = new HomeController(new NullLogger<HomeController>(), context);
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public void About_ReturnsAView()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new HomeController(new NullLogger<HomeController>(), context);
                var result = _sut.About();

                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public void Privacy_ReturnsAView()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new HomeController(new NullLogger<HomeController>(), context);
                var result = _sut.About();

                Assert.IsType<ViewResult>(result);
            }
        }

    }
}
