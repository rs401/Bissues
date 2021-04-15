using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class AppUserControllerTests
    {
        private AppUserController _sut;
        private DbContextOptions<ApplicationDbContext> _options;
        private UserManager<AppUser> _userManager;
        private NullLogger<AppUserController> _logger = new NullLogger<AppUserController>();
        public AppUserControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }
        [Fact]
        public async Task Index_WithNullUser_ReturnsNotFound()
        {
            // Arrange
            var name = "some name";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, _userManager){ControllerContext = concontext};
                var result = await _sut.Index();

                Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task Developer_WithNullUser_ReturnsNotFound()
        {
            // Arrange
            var name = "some name";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, _userManager){ControllerContext = concontext};
                var result = await _sut.Developer();

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Index_ReturnsAView()
        {
            // Arrange
            var name = "admin@admin.com";
            var user = new AppUser{
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Istrator",
                DisplayName = "Admin",
                EmailConfirmed = true
            };

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            
            var store = new Mock<IUserStore<AppUser>>();
            store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
                .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);//.Object;
            mockUser.Setup( userManager => userManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, mockUser.Object){ControllerContext = concontext};
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task Developer_ReturnsAView()
        {
            // Arrange
            var name = "admin@admin.com";
            var user = new AppUser{
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Istrator",
                DisplayName = "Admin",
                EmailConfirmed = true
            };

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            
            var store = new Mock<IUserStore<AppUser>>();
            store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
                .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);//.Object;
            mockUser.Setup( userManager => userManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, mockUser.Object){ControllerContext = concontext};
                var result = await _sut.Developer();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task ReadNotification_WithNullNotification_RedirectsToIndex()
        {
            // Arrange
            int id = 9999;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, _userManager);
                var result = await _sut.ReadNotification(id);

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectResult.ActionName);
            }
        }
        [Fact]
        public async Task ReadNotification_WithNullBissue_RedirectsToIndex()
        {
            // Arrange
            int id = 123;
            using (var context = new ApplicationDbContext(_options))
            {
                var notification = new Notification{ Id = id };
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, _userManager);
                var result = await _sut.ReadNotification(id);

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectResult.ActionName);
            }
        }
        [Fact]
        public async Task ReadNotification_RemovesNotification_RedirectsToBissuesDetails()
        {
            // Arrange
            int id = 124;
            int bid = 321;
            using (var context = new ApplicationDbContext(_options))
            {
                var bissue = new Bissue{ Id = bid };
                context.Bissues.Add(bissue);
                var notification = new Notification{ Id = id, BissueId = bid };
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(_logger, context, _userManager);
                var result = await _sut.ReadNotification(id);

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Details", redirectResult.ActionName);

                var notification = context.Notifications.Where(n => n.Id == id).FirstOrDefault();
                Assert.Null(notification);
            }
        }
    }
}
