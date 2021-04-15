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
    public class ModelTests
    {
        public ModelTests()
        {
        }

        [Fact]
        public void Bissue_Model()
        {
            // Arrange
            var proj = new Project{Id = 1};
            var bis = new Bissue
            {
                Id = 1,
                Title = "Bissue",
                Description = "Bissue test.",
                IsOpen = true,
                Messages = new List<Message>(),
                ProjectId = 1,
                Project = proj,
                AssignedDeveloperId = "1"
            };
            
            // Act
            // Assert
            Assert.IsType<string>(bis.Title);
            Assert.IsType<List<Message>>(bis.Messages);
            Assert.IsType<Project>(bis.Project);
            Assert.IsType<string>(bis.AssignedDeveloperId);
        }

        [Fact]
        public void BaseEntity_Model()
        {
            // Arrange
            var appUser = new AppUser{Id = "1"};
            var bEntity = new BaseEntity
            {
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                AppUserId = "1",
                AppUser = appUser
            };
            
            // Act
            // Assert
            Assert.IsType<DateTime>(bEntity.CreatedDate);
            Assert.IsType<DateTime>(bEntity.ModifiedDate);
            Assert.IsType<string>(bEntity.AppUserId);
            Assert.IsType<AppUser>(bEntity.AppUser);
        }

        [Fact]
        public void AppRole_Model()
        {
            // Arrange
            var appRole = new AppRole
            {
                Id = 1,
                RoleName = "role"
            };
            
            // Act
            // Assert
            Assert.IsType<Int32>(appRole.Id);
            Assert.IsType<string>(appRole.RoleName);
        }

        [Fact]
        public void MeToo_Model()
        {
            // Arrange
            var bis = new Bissue{Id = 2};
            var meToo = new MeToo
            {
                Id = 1,
                Ip = "ip",
                BissueId = 2,
                Bissue = bis
            };
            
            // Act
            // Assert
            Assert.IsType<Int32>(meToo.Id);
            Assert.IsType<string>(meToo.Ip);
            Assert.IsType<Int32>(meToo.BissueId);
            Assert.IsType<Bissue>(meToo.Bissue);
        }
        
        [Fact]
        public void AppUser_Model()
        {
            // Arrange
            var bis = new List<Bissue>();
            var mes = new List<Message>();
            var not = new List<Notification>();
            var appUser = new AppUser
            {
                FirstName = "first",
                LastName = "last",
                DisplayName = "display",
                Bissues = bis,
                Messages = mes,
                Notifications = not
            };
            
            // Act
            // Assert
            Assert.IsType<string>(appUser.FirstName);
            Assert.IsType<string>(appUser.LastName);
            Assert.IsType<string>(appUser.DisplayName);
            Assert.IsType<List<Bissue>>(appUser.Bissues);
            Assert.IsType<List<Message>>(appUser.Messages);
            Assert.IsType<List<Notification>>(appUser.Notifications);
        }

        [Fact]
        public void Message_Model()
        {
            // Arrange
            var bis = new Bissue{Id = 3};
            var msg = new Message
            {
                Id = 4,
                Body = "message",
                BissueId = 3,
                Bissue = bis
            };
            
            // Act
            // Assert
            Assert.IsType<Int32>(msg.Id);
            Assert.IsType<string>(msg.Body);
            Assert.IsType<Int32>(msg.BissueId);
            Assert.IsType<Bissue>(msg.Bissue);
        }

        [Fact]
        public void Notification_Model()
        {
            // Arrange
            var bis = new Bissue{Id = 5};
            var not = new Notification
            {
                Id = 6,
                IsUnread = true,
                BissueId = 5,
                Bissue = bis
            };
            
            // Act
            // Assert
            Assert.IsType<Int32>(not.Id);
            Assert.IsType<bool>(not.IsUnread);
            Assert.IsType<Int32>(not.BissueId);
            Assert.IsType<Bissue>(not.Bissue);
        }

        [Fact]
        public void OfficialNote_Model()
        {
            // Arrange
            var bis = new Bissue{Id = 7};
            var not = new OfficialNote
            {
                Id = 8,
                BissueId = 7,
                Bissue = bis,
                CommitId = "cid",
                CommitURL = "curl",
                Note = "note"
            };
            
            // Act
            // Assert
            Assert.IsType<Int32>(not.Id);
            Assert.IsType<Int32>(not.BissueId);
            Assert.IsType<Bissue>(not.Bissue);
            Assert.IsType<string>(not.CommitId);
            Assert.IsType<string>(not.CommitURL);
            Assert.IsType<string>(not.Note);
        }

        [Fact]
        public void Project_Model()
        {
            // Arrange
            var bis = new List<Bissue>();
            var proj = new Project
            {
                Id = 9,
                Name = "Test proj",
                Description = "Project desc",
                Bissues = bis
            };
            
            // Act
            // Assert
            Assert.IsType<Int32>(proj.Id);
            Assert.IsType<string>(proj.Name);
            Assert.IsType<string>(proj.Description);
            Assert.IsType<List<Bissue>>(proj.Bissues);
        }

        [Fact]
        public void RoleEdit_Model()
        {
            // Arrange
            var role = new IdentityRole();
            var mems = new List<AppUser>();
            var nonMems = new List<AppUser>();
            var rEdit = new RoleEdit
            {
                Role = role,
                Members = mems,
                NonMembers = nonMems
            };
            
            // Act
            // Assert
            Assert.IsType<IdentityRole>(rEdit.Role);
            Assert.IsType<List<AppUser>>(rEdit.Members);
            Assert.IsType<List<AppUser>>(rEdit.NonMembers);
        }

        [Fact]
        public void RoleModification_Model()
        {
            // Arrange
            var addIds = new string[]{"123"};
            var delIds = new string[]{"123"};
            var rMod = new RoleModification
            {
                RoleName = "role",
                RoleId = "id",
                AddIds = addIds,
                DeleteIds = delIds
            };
            
            // Act
            // Assert
            Assert.IsType<string>(rMod.RoleName);
            Assert.IsType<string>(rMod.RoleId);
            Assert.IsType<string[]>(rMod.AddIds);
            Assert.IsType<string[]>(rMod.DeleteIds);
        }

        [Fact]
        public void ErrorView_Model()
        {
            // Arrange
            var evm = new ErrorViewModel
            {
                RequestId = "reqID"
            };
            
            // Act
            // Assert
            Assert.IsType<string>(evm.RequestId);
        }
    }//END Test Class
}
