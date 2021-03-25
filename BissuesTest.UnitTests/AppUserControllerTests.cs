using System;
using System.Collections.Generic;
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
        public AppUserControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact(Skip = "Can't properly mock UserManager.")]
        public async Task Index_ReturnsAView()
        {
            // Arrange
            var name = "some name";

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier,name));

            var concontext = new ControllerContext(
                new ActionContext(
                    httpContext.Object, 
                    new Microsoft.AspNetCore.Routing.RouteData(), 
                    new ControllerActionDescriptor()
                    ));
            
            var store = new Mock<IUserStore<AppUser>>();
            store.Setup(x => x.FindByIdAsync(name, CancellationToken.None))
                .ReturnsAsync(new AppUser()
                {
                    Id = name
                });
            var mockUser = new Mock<UserManager<AppUser>>(store.Object, null, 
                null, null, null, null, null, null, null).Object;
            // mockUser.Setup( userManager => userManager.FindByIdAsync(It.IsAny<string>()))
            //     .ReturnsAsync(new AppUser { Id = name });
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new AppUserController(new NullLogger<AppUserController>(), context, mockUser){ControllerContext = concontext};
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

    }
}
