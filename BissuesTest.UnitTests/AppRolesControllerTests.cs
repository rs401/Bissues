using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AppRolesControllerTests
    {
        private AppRolesController _sut;
        private DbContextOptions<ApplicationDbContext> _options;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppUser admin = new AppUser{UserName = "admin@admin.com"};
        private AppUser user1 = new AppUser();
        private AppUser user2 = new AppUser();
        private List<AppUser> _users = new List<AppUser>();
        public AppRolesControllerTests()
        {
            _users.Add(admin);
            _users.Add(user1);
            _users.Add(user2);
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
        [Fact]//(Skip="Problems with mock usermanager")]
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
        public async Task EditPOST_ReturnsARedirect()
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
