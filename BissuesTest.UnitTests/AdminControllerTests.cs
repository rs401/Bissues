using System;
using System.Collections.Generic;
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
        public async Task EditBissue_DbConcurrencyException_ReturnsNotFound()
        {
            // Arrange
            int id = 11;
            Bissue bissue = new Bissue
            {
                Id = id,
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
        public async Task EditBissue_ClosedWithNullClosedDate_SetsClosedDate_AndReturnsRedirect()
        {
            // Arrange
            int id = 12;
            Bissue bissue = new Bissue
            {
                Id = id,
                IsOpen = false,
                ClosedDate = null
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Bissues.Add(bissue);
                await context.SaveChangesAsync();
            }
            // Act
            // Assert
            Assert.Null(bissue.ClosedDate);
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.EditBissue(id, bissue);

                Assert.IsType<RedirectToActionResult>(result);
                var retBissue = await context.Bissues.FirstOrDefaultAsync(b => b.Id == id);
                Assert.NotNull(retBissue.ClosedDate);
            }
        }
        [Fact]
        public void Users_ReturnsAView_WithAModel()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = _sut.Users();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<AppUser>>(viewResult.ViewData.Model);
            }
        }
        [Fact]
        public async Task LockUser_WithNullString_ReturnsNotFound()
        {
            // Arrange
            string sid = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.LockUser(sid);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task LockUser_WithNullUser_ReturnsNotFound()
        {
            // Arrange
            string sid = "string";
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.LockUser(sid);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task LockUser_LocksTheUser()
        {
            // Arrange
            string sid = "userIDstring";
            AppUser unlockedUser = new AppUser
            {
                Id = sid,
                LockoutEnabled = false
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.AppUsers.Add(unlockedUser);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.LockUser(sid);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var user = await context.AppUsers.FirstOrDefaultAsync(u => u.Id == sid);
                Assert.True(user.LockoutEnabled);
            }
        }




        [Fact]
        public async Task UnLockUser_WithNullString_ReturnsNotFound()
        {
            // Arrange
            string sid = null;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.UnLockUser(sid);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task UnLockUser_WithNullUser_ReturnsNotFound()
        {
            // Arrange
            string sid = "stringUnlockUser";
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.UnLockUser(sid);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task UnLockUser_UnLocksTheUser()
        {
            // Arrange
            string sid = "UnLockuserIDstring";
            AppUser lockedUser = new AppUser
            {
                Id = sid,
                LockoutEnabled = true
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.AppUsers.Add(lockedUser);
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AdminController(new NullLogger<AdminController>(), context);
                var result = await _sut.UnLockUser(sid);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var user = await context.AppUsers.FirstOrDefaultAsync(u => u.Id == sid);
                Assert.False(user.LockoutEnabled);
            }
        }

    }
}
