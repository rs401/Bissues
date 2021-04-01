using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class MessagesControllerTests
    {
        private MessagesController _sut;
        private UserManager<AppUser> _userManager;
        private DbContextOptions<ApplicationDbContext> _options;
        private NullLogger<MessagesController> _logger = new NullLogger<MessagesController>();
        public MessagesControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact]
        public async Task Index_ReturnsAView_WithAListOfMessages()
        {
            // Arrange
            // Insert seed data into the database
            using (var context = new ApplicationDbContext(_options))
            {
                context.Messages.Add(new Message 
                {
                    Id = 1, 
                    BissueId = 1,
                    Body = "Test Message"
                });
                context.SaveChanges();
            }
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Message>>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public void CreateWithBissueId_ReturnsAView()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project{
                    Id = 9,
                    Name = "Test Project 3",
                    Description = "Test Project 3 Description"
                });
                context.Bissues.Add(new Bissue{
                    Id = 8,
                    Title = "BissuesControllerTests Bissue",
                    Description = "BissuesControllerTests Bissue Description",
                    IsOpen = true, 
                    ProjectId = 9, 
                    Label = BissueLabel.Issue
                });
                context.SaveChanges();
            }
            int? id = 8;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = _sut.Create(id);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void CreateWithNullBissueId_ReturnsAView()
        {
            // Arrange
            int? id = null;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = _sut.Create(id);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task EditWithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task EditWithNullMessage_ReturnsNotFound()
        {
            // Arrange
            int? id = 9999;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task DeleteWithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task DeleteWithNullMessage_ReturnsNotFound()
        {
            // Arrange
            int? id = 99999;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
