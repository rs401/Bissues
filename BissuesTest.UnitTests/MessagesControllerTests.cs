using System;
using System.Collections.Generic;
using System.Net.Http;
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
        public async Task CreatePOST_ReturnsRedirect()
        {
            // Arrange
            int id = 111;
            int bid = 334;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new Message
            {
                Id = id,
                BissueId = bid,
                Body = "Test<script>alert('test')</script>"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = auId,
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Create(msg);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
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
        public async Task Edit_WithWrongUser_ReturnsForbid()
        {
            // Arrange
            int id = 112;
            string auId = "4";
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = "1",
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<ForbidResult>(result);
            }
        }
        
        [Fact]
        public async Task Edit_ReturnsAView()
        {
            // Arrange
            int id = 113;
            string auId = "4";
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = auId,
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task EditPOST_ReturnsRedirect()
        {
            // Arrange
            int id = 114;
            int bid = 335;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = auId,
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id, msg);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
            }
        }

        [Fact]
        public async Task EditPOST_BadIds_ReturnsNotFound()
        {
            // Arrange
            int id = 115;
            int badId = 116;
            int bid = 336;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = auId,
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(badId, msg);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task EditPOST_WrongUser_ReturnsForbid()
        {
            // Arrange
            int id = 116;
            int bid = 337;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = "1",
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id, msg);

                var viewResult = Assert.IsType<ForbidResult>(result);
            }
        }

        [Fact]
        public async Task Delete_WrongUser_ReturnsForbid()
        {
            // Arrange
            int id = 117;
            int bid = 338;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = "1",
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<ForbidResult>(result);
            }
        }
        [Fact]
        public async Task Delete_ReturnsAVieww()
        {
            // Arrange
            int id = 118;
            int bid = 339;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new Message
            {
                Id = id,
                AppUserId = auId,
                Body = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);
            httpContext.Setup(m => m.User.IsInRole("Admin"))
                .Returns(false);

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            var user1 = new AppUser{
                Id = auId,
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByNameAsync(name)).ReturnsAsync(user1);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<ViewResult>(result);
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

        [Fact]
        public async Task DeleteConfirm_Deletes_ReturnsRedirect()
        {
            // Arrange
            int id = 119;
            var msg = new Message { Id = id };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Messages.Add(msg);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.DeleteConfirmed(id);
                var delMsg = await context.Messages.FirstOrDefaultAsync(m => m.Id == id);
                Assert.Null(delMsg);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
            }
        }

        [Fact]
        public async Task DeleteConfirm_WithNullMsg_ReturnsRedirect()
        {
            // Arrange
            int id = 119;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new MessagesController(context, _userManager, _logger);
                var result = await _sut.DeleteConfirmed(id);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
            }
        }
    }
}
