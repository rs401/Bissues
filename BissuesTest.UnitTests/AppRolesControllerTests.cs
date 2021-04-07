using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
    public class AppRolesControllerTests
    {
        private AppRolesController _sut;
        private DbContextOptions<ApplicationDbContext> _options;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AppRolesControllerTests()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object,null,null,null,null).Object;
            var store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact]
        public void Index_ReturnsAView()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(_roleManager, _userManager);
                var result = _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public void Create_ReturnsAView()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(_roleManager, _userManager);
                var result = _sut.Create();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task CreatePOST_ReturnsAView()
        {
            // Arrange
            string tmpName = "Test";
            AppRole appRole = new AppRole{RoleName = tmpName};
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(_roleManager, _userManager);
                var result = await _sut.Create(appRole);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task Edit_ReturnsAView()
        {
            // Arrange
            // var userStore = new Mock<IUserStore<AppUser>>();

            // var userManager = new Mock<UserManager<AppUser>>(userStore.Object);
            // userManager.Setup(_ => _.Users).Returns(_users.AsQueryable);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(_roleManager, _userManager);
                var result = await _sut.Edit("");

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task Edit_WithUsers_ReturnsAView()
        {
            // while(!Debugger.IsAttached) Thread.Sleep(500);
            // Arrange
            string role = "User";
            string roleId = "1";
            var users = new List<AppUser>();
            var user1 = new AppUser{
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Istrator",
                DisplayName = "Admin",
                EmailConfirmed = true
            };
            var user2 = new AppUser{
                Email = "user@user.com",
                UserName = "user@user.com",
                FirstName = "user",
                LastName = "asdf",
                DisplayName = "user",
                EmailConfirmed = true
            };
            users.Add(user1);
            users.Add(user2);

            var mRole = new IdentityRole(role);
            mRole.Id = roleId;
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup( userManager => userManager.Users)
                .Returns(users.AsQueryable());
            mockUser.Setup(userManager =>  userManager.IsInRoleAsync(user2,role))
                .ReturnsAsync(true);
            
            var mRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mRoleManager = new Mock<RoleManager<IdentityRole>>(mRoleStore.Object,null,null,null,null);
            mRoleManager.Setup(rm => rm.FindByIdAsync(roleId))
                .ReturnsAsync(mRole);
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(mRoleManager.Object, mockUser.Object);
                var result = await _sut.Edit(roleId);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        [Fact]
        public async Task EditPOST_ReturnsARedirect()
        {
            // Arrange
            // Users
            string role = "User";
            string roleId = "1";
            var users = new List<AppUser>();
            var user1 = new AppUser{
                Id = "1",
                Email = "user1@user1.com",
                UserName = "user1@user1.com",
                FirstName = "user1",
                LastName = "asdf",
                DisplayName = "user1",
                EmailConfirmed = true
            };
            var user2 = new AppUser{
                Id = "2",
                Email = "user@user.com",
                UserName = "user@user.com",
                FirstName = "user",
                LastName = "asdf",
                DisplayName = "user",
                EmailConfirmed = true
            };
            users.Add(user1);
            users.Add(user2);
            // Role
            var mRole = new IdentityRole(role);
            mRole.Id = roleId;
            // Mock UserManager
            var store = new Mock<IUserStore<AppUser>>();
            // store.Setup(x => x.FindByNameAsync(name, CancellationToken.None))
            //     .ReturnsAsync(user);
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null);
            mockUser.Setup(userManager =>  userManager.FindByIdAsync("1")).ReturnsAsync(user1);
            mockUser.Setup(userManager =>  userManager.FindByIdAsync("2")).ReturnsAsync(user2);
            mockUser.Setup(userManager =>  userManager.AddToRoleAsync(user1, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mockUser.Setup(userManager =>  userManager.RemoveFromRoleAsync(user2, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            // Mock RoleManager
            var mRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mRoleManager = new Mock<RoleManager<IdentityRole>>(mRoleStore.Object,null,null,null,null);
            mRoleManager.Setup(rm => rm.FindByIdAsync(roleId))
                .ReturnsAsync(mRole);
            // RoleModification
            RoleModification roleMod = new RoleModification()
            {
                RoleName = role,
                RoleId = roleId,
                AddIds = new string[]{"1"},
                DeleteIds = new string[]{"2"}
            };
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(mRoleManager.Object, mockUser.Object);
                var result = await _sut.Edit(roleMod);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
            }
        }
        [Fact]
        public async Task EditPOST_AddsId_ReturnsARedirect()
        {
            // Arrange
            RoleModification roleMod = new RoleModification();
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(_roleManager, _userManager);
                var result = await _sut.Edit(roleMod);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
            }
        }

    }
}
