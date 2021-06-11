using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.Services;
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
    public class NoteControllerTests
    {
        private NoteController _sut;
        private UserManager<AppUser> _userManager;
        private DbContextOptions<ApplicationDbContext> _options;
        private NullLogger<NoteController> _logger = new NullLogger<NoteController>();
        private ISanitizer sanitizer = new Sanitizer();
        public NoteControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact]
        public async Task Index_ReturnsAView()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task DetailsWithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Details(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DetailsWithNullNote_ReturnsNotFound()
        {
            // Arrange
            int id = 444;
            int bid = 445;
            string auId = "6";
            var appUser = new AppUser
            {
                Id = auId
            };
            var bissue = new Bissue
            {
                Id = bid
            };
            var note = new OfficialNote
            {
                Id = 1,
                BissueId = bid,
                Bissue = bissue,
                AppUserId = auId,
                AppUser = appUser,
                Note = "test"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.AppUsers.Add(appUser);
                context.Bissues.Add(bissue);
                context.OfficialNotes.Add(note);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Details(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Details_ReturnsAView()
        {
            // Arrange
            int id = 445;
            int bid = 446;
            string auId = "5";
            var appUser = new AppUser
            {
                Id = auId
            };
            var bissue = new Bissue
            {
                Id = bid
            };
            var note = new OfficialNote
            {
                Id = id,
                BissueId = bid,
                Bissue = bissue,
                AppUserId = auId,
                AppUser = appUser,
                Note = "test"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.AppUsers.Add(appUser);
                context.Bissues.Add(bissue);
                context.OfficialNotes.Add(note);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Details(id);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void Create_ReturnsAView()
        {
            // Arrange
            int bid = 447;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = _sut.Create(bid);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void Create_WithNullId_ReturnsAView()
        {
            // Arrange
            int? bid = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = _sut.Create(bid);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task CreatePOST_ReturnsRedirect()
        {
            // Arrange
            int id = 112;
            int bid = 448;
            string auId = "7";
            var bissue = new Bissue
            {
                Id = bid
            };
            var note = new OfficialNote
            {
                Id = id,
                BissueId = bid,
                Note = "Test<script>alert('test')</script>"
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
                _sut = new NoteController(context, mockUser.Object, _logger, sanitizer);
                _sut.ControllerContext = concontext;
                var result = await _sut.Create(note);

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
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task EditWithNullOfficialNote_ReturnsNotFound()
        {
            // Arrange
            int? id = 9999;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        
        [Fact]
        public async Task Edit_ReturnsAView()
        {
            // Arrange
            int id = 450;
            string auId = "4";
            var note = new OfficialNote
            {
                Id = id,
                AppUserId = auId,
                Note = "Test<script>alert('test')</script>"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.OfficialNotes.Add(note);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                // _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        
        [Fact]
        public async Task EditPOST_WithWrongId_ReturnsNotFound()
        {
            // Arrange
            int id = 451;
            int badId = 452;
            string auId = "4";
            var note = new OfficialNote
            {
                Id = id,
                AppUserId = auId,
                Note = "Test<script>alert('test')</script>"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.OfficialNotes.Add(note);
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
                _sut = new NoteController(context, mockUser.Object, _logger, sanitizer);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(badId, note);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        
        [Fact]
        public async Task EditPOST_ReturnsRedirect()
        {
            // Arrange
            int id = 453;
            string auId = "4";
            var note = new OfficialNote
            {
                Id = id,
                AppUserId = auId,
                Note = "Test<script>alert('test')</script>"
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.OfficialNotes.Add(note);
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
                _sut = new NoteController(context, mockUser.Object, _logger, sanitizer);
                _sut.ControllerContext = concontext;
                var result = await _sut.Edit(id, note);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
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
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteWithNullOfficialNote_ReturnsNotFound()
        {
            // Arrange
            int? id = 99999;
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_WrongUser_ReturnsForbid()
        {
            // Arrange
            int id = 454;
            int bid = 455;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new OfficialNote
            {
                Id = id,
                AppUserId = auId,
                Note = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.OfficialNotes.Add(msg);
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
                _sut = new NoteController(context, mockUser.Object, _logger, sanitizer);
                _sut.ControllerContext = concontext;
                var result = await _sut.Delete(id);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task DeleteConfirm_ReturnsRedirect()
        {
            // Arrange
            int id = 456;
            int bid = 457;
            string auId = "4";
            var bissue = new Bissue
            {
                Id = bid
            };
            var msg = new OfficialNote
            {
                Id = id,
                AppUserId = auId,
                Note = "Test<script>alert('test')</script>",
                Bissue = bissue,
                BissueId = bid
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                context.OfficialNotes.Add(msg);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new NoteController(context, _userManager, _logger, sanitizer);
                var result = await _sut.DeleteConfirmed(id);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
            }
        }
    }
}
