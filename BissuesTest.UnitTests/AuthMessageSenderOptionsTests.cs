using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bissues.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BissuesTest.UnitTests
{
    public class AuthMessageSenderOptionsTests
    {
        private AuthMessageSenderOptions _sut;
        public AuthMessageSenderOptionsTests()
        {
            _sut = new AuthMessageSenderOptions();
        }

        [Fact]
        public void AuthMessageSenderOptions_SendGridUser_ReturnsAString()
        {
            // Arrange
            // Act
            var result = _sut.SendGridUser;
            // Assert
            Assert.IsType<string>(result);
        }
        [Fact]
        public void AuthMessageSenderOptions_SendGridEmail_ReturnsAString()
        {
            // Arrange
            // Act
            var result = _sut.SendGridEmail;
            // Assert
            Assert.IsType<string>(result);
        }
        [Fact]
        public void AuthMessageSenderOptions_SendGridKey_ReturnsAString()
        {
            // Arrange
            // Act
            var result = _sut.SendGridKey;
            // Assert
            Assert.IsType<string>(result);
        }

    }
}
