using System;
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
        public async Task Edit_ReturnsAView()
        {
            // Arrange
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppRolesController(_roleManager, _userManager);
                var result = await _sut.Edit("");

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

    }
}
