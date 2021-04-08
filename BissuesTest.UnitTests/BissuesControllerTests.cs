using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
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
    public class BissuesControllerTests
    {
        private BissuesController _sut;
        private UserManager<AppUser> _userManager;
        private DbContextOptions<ApplicationDbContext> _options;
        private NullLogger<BissuesController> _logger = new NullLogger<BissuesController>();
        public BissuesControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            _options  = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact]
        public void Index_ReturnsAView_WithBissuesIndexViewModel()
        {
            // Arrange
            int id = 1;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Index(id);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<BissuesIndexViewModel>
                                (viewResult.ViewData.Model);
            }
        }
        [Fact]
        public void Index_WithNullIndex_ReturnsAView_WithBissuesIndexViewModel()
        {
            // Arrange
            int? index = null;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Index(index);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<BissuesIndexViewModel>
                                (viewResult.ViewData.Model);
            }
        }
        [Fact]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Details(id,1);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task Details_WithNullBissue_ReturnsNotFound()
        {
            // Arrange
            int? id = 5;
            
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Details(id,1);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task Details_ReturnsAView_WithABissuesDetailsViewModel()
        {
            // Arrange
            using(var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project{
                    Id = 2,
                    Name = "Test Project 2",
                    Description = "Test Project 2 Description"
                });
                context.Bissues.Add(new Bissue{
                    Id = 6,
                    Title = "BissuesControllerTests Bissue",
                    Description = "BissuesControllerTests Bissue Description",
                    IsOpen = true, 
                    ProjectId = 2, 
                    Label = BissueLabel.Issue
                });
                context.SaveChanges();
            }
            int? id = 6;
            int? currentIndex = 1;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Details(id,currentIndex);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<BissuesDetailsViewModel>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public async Task DetailsWithNullIndex_ReturnsAView_WithABissuesDetailsViewModel()
        {
            // Arrange
            using(var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(new Bissue{
                    Id = 7,
                    Title = "BissuesControllerTests Bissue",
                    Description = "BissuesControllerTests Bissue Description",
                    IsOpen = true, 
                    ProjectId = 3, 
                    Label = BissueLabel.Issue
                });
                context.SaveChanges();
            }
            int? id = 7;
            int? currentIndex = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Details(id,currentIndex);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<BissuesDetailsViewModel>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public async Task DetailsWithNullMeToos_ReturnsAView()
        {
            // Arrange
            using(var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(new Bissue{
                    Id = 15,
                    Title = "BissuesControllerTests Bissue",
                    Description = "BissuesControllerTests Bissue Description",
                    IsOpen = true, 
                    ProjectId = 3, 
                    Label = BissueLabel.Issue,
                    MeToos = null
                });
                context.SaveChanges();
            }
            int? id = 15;
            int? currentIndex = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Details(id,currentIndex);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void CreateWithProjectId_ReturnsAView()
        {
            // Arrange
            int? pid = 1;//ProjectId
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Create(pid);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void CreateWithNullProjectId_ReturnsAView()
        {
            // Arrange
            int? pid = null;//ProjectId
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Create(pid);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task CreatePOST_ReturnsARedirect()
        {
            // while(!Debugger.IsAttached) Thread.Sleep(500);
            // Arrange
            int id = 222;
            int pid = 333;
            Project proj = new Project
            {
                Id = pid
            };
            Bissue bissue = new Bissue
            {
                Id = id,
                ProjectId = pid,
                Description = "asdf"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(proj);
                context.SaveChanges();
            }
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

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
                _sut = new BissuesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Create(bissue);
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
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Edit(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task EditWithNullBissue_ReturnsNotFound()
        {
            // Arrange
            int? id = 9999;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Edit(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task EditWith_MockUser_ReturnsAView()
        {
            // while(!Debugger.IsAttached) Thread.Sleep(500);
            // Arrange
            using(var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(new Bissue{
                    Id = 13,
                    Title = "BissuesControllerTests Bissue",
                    Description = "BissuesControllerTests Bissue Description",
                    IsOpen = true, 
                    ProjectId = 3, 
                    Label = BissueLabel.Issue,
                    AppUserId = "1"
                });
                context.SaveChanges();
            }
            int? id = 13;
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

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
                _sut = new BissuesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task EditWith_WrongUser_ReturnsForbid()
        {
            // while(!Debugger.IsAttached) Thread.Sleep(500);
            // Arrange
            using(var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(new Bissue{
                    Id = 14,
                    Title = "BissuesControllerTests Bissue",
                    Description = "BissuesControllerTests Bissue Description",
                    IsOpen = true, 
                    ProjectId = 3, 
                    Label = BissueLabel.Issue,
                    AppUserId = "2"
                });
                context.SaveChanges();
            }
            int? id = 14;
            var name = "user1@user1.com";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.Identity.Name)
                .Returns(name);

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
                _sut = new BissuesController(context, mockUser.Object, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id);
                var viewResult = Assert.IsType<ForbidResult>(result);
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
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Delete(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task DeleteWithNullBissue_ReturnsNotFound()
        {
            // Arrange
            int? id = 9999;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.Delete(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public void Search_WithNulls_ReturnsAView()
        {
            // Arrange
            int? index = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Search(index,null);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void Search_WithValues_ReturnsAView()
        {
            // Arrange
            int? index = 123;
            string query = "query";
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Search(index,query);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void Search_WithNullIndex_ReturnsAView()
        {
            // Arrange
            int? index = null;
            string query = "query";
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Search(index,query);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void Search_WithNullQuery_ReturnsAView()
        {
            // Arrange
            int? index = 123;
            string query = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = _sut.Search(index,query);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task AddMeToo_WithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.AddMeToo(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task AddMeToo_WithNullBissue_ReturnsNotFound()
        {
            // Arrange
            int? id = 111;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new BissuesController(context, _userManager, _logger);
                var result = await _sut.AddMeToo(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task AddMeToo_AddsMeToo()
        {
            // while(!Debugger.IsAttached) Thread.Sleep(500);
            // Arrange
            int? id = 18;
            using (var context = new ApplicationDbContext(_options))
            {
                var bissue = new Bissue
                {
                    Id = (int)id,
                    MeToos = null
                };
                context.Bissues.Add(bissue);
                context.SaveChanges();
            }
            var httpContext = new Mock<HttpContext>();
            var ipa = new Mock<IPAddress>();
            httpContext.Setup(hc => hc.Connection.RemoteIpAddress)
                .Returns(IPAddress.Parse("1.2.3.4"));
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
                _sut = new BissuesController(context, _userManager, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.AddMeToo(id);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                // Add check that metoo was added
                var metoos = context.MeToos.Where(mt => mt.Ip == "1.2.3.4").ToList();
                Assert.Equal("1.2.3.4", metoos[0].Ip);
            }
        }
        [Fact]
        public async Task AddMeToo_NotUnique_ReturnsRedirect()
        {
            // while(!Debugger.IsAttached) Thread.Sleep(500);
            // Arrange
            int? id = 19;
            var bissue = new Bissue{Id = (int)id};
            var me2 = new MeToo
            {
                Id = (int)id,
                Ip = "1.1.1.1",
                Bissue = bissue,
                BissueId = (int)id
            };
            var bMeToos = new List<MeToo>();
            bMeToos.Add(me2);
            bissue.MeToos = bMeToos;
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.MeToos.Add(me2);
                context.SaveChanges();
            }
            var httpContext = new Mock<HttpContext>();
            // var ipa = new Mock<IPAddress>();
            httpContext.Setup(hc => hc.Connection.RemoteIpAddress)
                .Returns(IPAddress.Parse("1.1.1.1"));
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
                _sut = new BissuesController(context, _userManager, _logger);
                _sut.ControllerContext = concontext;
                var result = await _sut.AddMeToo(id);
                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Details", redirectResult.ActionName);
            }
        }
    }
}
