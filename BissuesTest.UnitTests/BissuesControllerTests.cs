using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Http;
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
        // [Fact]
        // public async Task EditWith_MockUser()
        // {
        //     // Arrange
        //     using(var context = new ApplicationDbContext(_options))
        //     {
        //         context.Bissues.Add(new Bissue{
        //             Id = 8,
        //             Title = "BissuesControllerTests Bissue",
        //             Description = "BissuesControllerTests Bissue Description",
        //             IsOpen = true, 
        //             ProjectId = 3, 
        //             Label = BissueLabel.Issue
        //         });
        //         context.SaveChanges();
        //     }
        //     int? id = 8;
        //     // Thanks SO
        //     var controllerContextMock = new ControllerContext();
        //     var httpContextMock = new Mock<HttpContext>();
        //     var identityMock = new GenericIdentity("User");
        //     var principal = new GenericPrincipal(identityMock, null);
        //     // httpContextMock.Setup(x => x.User.IsInRole(It.Is<string>(s => s.Equals("Admin")))).Returns(true);
        //     httpContextMock.Setup(x => x.User).Returns(principal);
        //     controllerContextMock.HttpContext = httpContextMock.Object;

        //     // Act
        //     // Assert
        //     using (var context = new ApplicationDbContext(_options))
        //     {
        //         _sut = new BissuesController(context, _userManager);
        //         _sut.ControllerContext = controllerContextMock;
        //         var result = await _sut.Edit(id);
        //         var viewResult = Assert.IsType<NotFoundResult>(result);
        //     }
        // }
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
    }
}
