using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Bissues;
using Bissues.Controllers;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class ProjectsControllerTests
    {
        private ProjectsController _sut;
        private DbContextOptions<ApplicationDbContext> _options;
        public ProjectsControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Bissues")
                .Options;
        }

        [Fact]
        public async Task Index_ReturnsAView_WithListOfProjects()
        {
            // Arrange
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Project>>(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public async Task Details_ReturnsAView_WithProjectsDetailViewModel()
        {
            // Arrange
            
            // Act
            // Insert seed data into the database context
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project {Id = 1, Name = "Test Project 1", Description = "Test Project 1 Description"});
                context.SaveChanges();
            }
            
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Details(1,1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<ProjectsDetailViewModel>(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public async Task DetailsWithNullId_ReturnsNotFound()
        {
            // Arrange
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                int? id = null;
                int? index = 1;
                var result = await _sut.Details(id,index);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DetailsWithNullProject_ReturnsNotFound()
        {
            // Arrange
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                int? id = 9999;
                int? index = null;
                var result = await _sut.Details(id,index);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DetailsWithNullIndex_ReturnsView__WithProjectsDetailViewModel()
        {
            // Arrange
            
            // Act
            // Insert seed data into the database context
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project {
                    Id = 3, 
                    Name = "Test Project 2", 
                    Description = "Test Project 2 Description"
                });
                context.SaveChanges();
            }
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                int? id = 3;
                int? index = null;
                var result = await _sut.Details(id,index);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<ProjectsDetailViewModel>(viewResult.ViewData.Model);

            }
        }
        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = _sut.Create();
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Create_CreatesProject_ReturnsRedirect()
        {
            // Arrange
            int id = 100;
            var proj = new Project{Id = id};
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Create(proj);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.NotNull(await context.Projects.FirstOrDefaultAsync(p => p.Id == id));
            }
        }

        [Fact]
        public async Task Edit_ReturnsAView_WithAProjectModel()
        {
            // Arrange
            
            // Insert seed data into the database context
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project {Id = 4, Name = "Test Project 3", Description = "Test Project 3 Description"});
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Edit(4);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);
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
                _sut = new ProjectsController(context);
                var result = await _sut.Edit(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task EditWithNullProject_ReturnsNotFound()
        {
            // Arrange
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Edit(9999);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task EditPOST_WithMissMatchId_ReturnsNotFound()
        {
            // Arrange
            int id = 101;
            int badId = 102;
            Project proj = new Project{Id = id};
            
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Edit(badId, proj);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task EditPOST_ReturnsRedirect()
        {
            // Arrange
            int id = 103;
            Project proj = new Project
            {
                Id = id,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(proj);
                await context.SaveChangesAsync();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Edit(id, proj);
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
                _sut = new ProjectsController(context);
                var result = await _sut.Delete(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteWithNullProject_ReturnsNotFound()
        {
            // Arrange
            int? id = 999;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Delete(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_ReturnsAView_WithAProject()
        {
            // Arrange
            
            // Insert seed data into the database context
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(new Project {Id = 5, Name = "Test Project 4", Description = "Test Project 4 Description"});
                context.SaveChanges();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.Delete(5);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public async Task DeleteConfirm_WithNullProject_ReturnsNotFound()
        {
            // Arrange
            int id = 104;
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.DeleteConfirmed(id);
                var viewResult = Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteConfirm_DeletesProject_ReturnsRedirect()
        {
            // Arrange
            int id = 105;
            Project proj = new Project{ Id = id };
            using (var context = new ApplicationDbContext(_options))
            {
                context.Projects.Add(proj);
                await context.SaveChangesAsync();
            }
            // Act
            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                _sut = new ProjectsController(context);
                var result = await _sut.DeleteConfirmed(id);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var delProj = context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                Assert.Null(delProj.Result);
            }
        }
        
    }//END class ProjectsControllerTests
}