using Microsoft.Extensions.Configuration;
using Moq;
using MoodSensingApp.Models;
using MoodSensingApp.Repositories;
using MoodSensingApp.Services;
using NUnit.Framework;
using System.Threading.Tasks;
using System;

namespace MoodSensingAPITests
{


    namespace MoodSensingApp.Tests.Services
    {
        [TestFixture]
        public class UserServiceTests
        {
            private Mock<IUserRepository> _userRepositoryMock;
            private Mock<IConfiguration> _configurationMock;
            private UserService _userService;

            [SetUp]
            public void Setup()
            {
                _userRepositoryMock = new Mock<IUserRepository>();
                _configurationMock = new Mock<IConfiguration>();
                _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object);
            }

            [Test]
            public async Task RegisterUser_ValidUsernameAndPassword_ReturnsTrue()
            {
                // Arrange
                var username = "testuser";
                var password = "password";

                _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
                    .Returns(Task.FromResult(true));

                // Act
                var result = await _userService.RegisterUser(username, password);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public async Task RegisterUser_ExceptionOccurs_ReturnsFalse()
            {
                // Arrange
                var username = "testuser";
                var password = "password";

                _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
                    .ThrowsAsync(new Exception("Failed to register user."));

                // Act
                var result = await _userService.RegisterUser(username, password);

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public async Task AuthenticateUser_ValidUsernameAndPassword_ReturnsTrue()
            {
                // Arrange
                var username = "testuser";
                var password = "password";

                byte[] salt = _userService.GenerateSalt();
                var passwordHash = _userService.HashPassword(password, salt);
                var passwordHashBase64 = Convert.ToBase64String(passwordHash);

                var user = new User
                {
                    Username = username,
                    Password = passwordHashBase64,
                    Salt = Convert.ToBase64String(salt)
                };

                _userRepositoryMock.Setup(x => x.GetByUsername(username))
                    .ReturnsAsync(user);

                // Act
                var result = await _userService.AuthenticateUser(username, password);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public async Task AuthenticateUser_InvalidUsername_ReturnsFalse()
            {
                // Arrange
                var username = "testuser";
                var password = "password";

                _userRepositoryMock.Setup(x => x.GetByUsername(username))
                    .ReturnsAsync((User)null);

                // Act
                var result = await _userService.AuthenticateUser(username, password);

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public async Task AuthenticateUser_InvalidPassword_ReturnsFalse()
            {
                // Arrange
                var username = "testuser";
                var password = "password";

                var salt = _userService.GenerateSalt();
                var passwordHash = _userService.HashPassword("wrong_password", salt);
                var passwordHashBase64 = Convert.ToBase64String(passwordHash);

                var user = new User
                {
                    Username = username,
                    Password = passwordHashBase64,
                    Salt = Convert.ToBase64String(salt)
                };

                _userRepositoryMock.Setup(x => x.GetByUsername(username))
                    .ReturnsAsync(user);

                // Act
                var result = await _userService.AuthenticateUser(username, password);

                // Assert
                Assert.IsFalse(result);
            }

          
            [Test]
            public void GenerateJSONWebToken_UserNotFound_ThrowsException()
            {
                // Arrange
                var userName = "testuser";

                _userRepositoryMock.Setup(x => x.GetByUsername(userName))
                    .ReturnsAsync((User)null);

                // Act & Assert
                Assert.ThrowsAsync<Exception>(async () => await _userService.GenerateJSONWebToken(userName));
            }
        }
    }

}
