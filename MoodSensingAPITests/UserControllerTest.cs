using MoodSensingApp.Controllers;
using MoodSensingApp.RequestModels;
using MoodSensingApp.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoodSensingAPITests
{
    [TestFixture]
    public class UserControllerTest
    {
        public Mock<IUserService> _userServiceMock;
       
        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        [Test]
        public async Task CheckPassworHaveEnoughCharacters_RegisterUser()
        {
            string userName = "ank";
            string password = "passw";
            RegisterModel rm = new RegisterModel()
            {
                Username = userName,
                Password = password
            };

            var controller = new UserController(_userServiceMock.Object);
            var op =  controller.Register(rm);
            Assert.IsNotNull(op);
        }
    }
}
