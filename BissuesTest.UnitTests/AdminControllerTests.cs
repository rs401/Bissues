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
    public class AdminControllerTests
    {
        private AdminController _sut;
        private DbContextOptions<ApplicationDbContext> _options;
        public AdminControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact]
        public void Index_ReturnsAView_WithAdminAreaViewModel()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<AdminAreaViewModel>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public void Bissues_ReturnsAView_WithAdminAreaViewModel()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = _sut.Bissues();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<AdminAreaViewModel>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public void Bugs_ReturnsAView_WithAdminBugsViewModel()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = _sut.Bugs();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<AdminBugsViewModel>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public async Task BissueDetails_WithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.BissueDetails(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task BissueDetails_WithNullBissue_ReturnsNotFound()
        {
            // Arrange
            int? id = 999;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.BissueDetails(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task BissueDetails_ReturnsAView_WithAdminBissueViewModel()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project{
                    Id = 10,
                    Name = "Test Project",
                    Description = "Test Project Description"
                });
                context.Bissues.Add(new Bissue 
                {
                    Id = 9, 
                    Title = "Test Bissue", 
                    Description = "Test Bissue Description", 
                    IsOpen = true, 
                    ProjectId = 10, 
                    Label = BissueLabel.Issue
                });
                context.SaveChanges();
            }
            int? id = 9;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.BissueDetails(id);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Bissue>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public async Task EditBissue_WithMismatchedIds_ReturnsNotFound()
        {
            // Arrange
            int id = 9;
            Bissue bissue = new Bissue
            {
                Id = 10
            };
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.EditBissue(id, bissue);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task EditBissue_Returnssomething()
        {
            // Arrange
            int id = 10;
            Bissue bissue = new Bissue
            {
                Id = 10,
                IsOpen = false,
                ClosedDate = null
            };
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.EditBissue(id, bissue);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

    }
}
