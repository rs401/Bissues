using System;
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class HomeControllerTests
    {
        private HomeController _sut;
        public HomeControllerTests()
        {
        }

        [Fact]
        public async Task Index_ReturnsAView_WithIndexViewModel()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
            
            // Insert seed data into the database
            using (var context = new ApplicationDbContext(options))
            {
                context.Bissues.Add(new Bissue 
                {
                    Id = 1, 
                    Title = "Test Bissue 1", 
                    Description = "Test Bissue 1 Description", 
                    IsOpen = true, 
                    ProjectId = 1, 
                    Label = BissueLabel.Issue
                });
                context.Bissues.Add(new Bissue 
                {
                    Id = 2, 
                    Title = "Test Bissue 2", 
                    Description = "Test Bissue 2 Description", 
                    IsOpen = false, 
                    ProjectId = 1, 
                    Label = BissueLabel.Issue
                });
                context.Bissues.Add(new Bissue 
                {
                    Id = 3, 
                    Title = "Test Bug 1", 
                    Description = "Test Bug 1 Description", 
                    IsOpen = true, 
                    ProjectId = 1, 
                    Label = BissueLabel.Bug
                });
                context.Bissues.Add(new Bissue 
                {
                    Id = 4, 
                    Title = "Test Bug 2", 
                    Description = "Test Bug 2 Description", 
                    IsOpen = false, 
                    ProjectId = 1, 
                    Label = BissueLabel.Bug
                });
                context.SaveChanges();
            }
            
            // Act
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
                var result = _sut.Privacy();

                Assert.IsType<ViewResult>(result);
            }
        }
    }
}
