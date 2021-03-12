using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Http;
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

        [Fact]
        public async Task DetailsWithNullId_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new ProjectsController(context);
                int? id = null;
                int? index = null;
                var result = await _sut.Details(id,index);
                var viewResult = Assert.IsType<NotFoundResult>(result);

                id = 2;
                result = await _sut.Details(id,index);
                viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DetailsWithNullProject_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new ProjectsController(context);
                int? id = 2;
                int? index = null;
                var result = await _sut.Details(id,index);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DetailsWithNullIndex_ReturnsView__WithProjectsDetailViewModel()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            // Insert seed data into the database context
            using (var context = new ApplicationDbContext(options))
            {
                context.Projects.Add(new Project {Id = 3, Name = "Test Project 2", Description = "Test Project 2 Description"});
                context.SaveChanges();
            }
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new ProjectsController(context);
                int? id = 3;
                int? index = null;
                var result = await _sut.Details(id,index);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<ProjectsDetailViewModel>(viewResult.ViewData.Model);

            }
        }
        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            // Act
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "UserName"),
                new Claim(ClaimTypes.Role, "Admin")
            }));
            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                _sut = new ProjectsController(context);
                _sut.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                };
                /* I don't think this is working correctly, I think I'm being
                 * redirected to login view */
                var result = _sut.Create();
                System.Console.WriteLine("Result: " + result);
                var viewResult = Assert.IsType<ViewResult>(result);
                System.Console.WriteLine("viewResult.TempData: " + viewResult.TempData);
            }
        }

        
    }//END class ProjectsControllerTests
}