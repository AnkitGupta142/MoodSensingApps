using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoodSensingApp.Controllers;
using MoodSensingApp.Models;
using MoodSensingApp.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoodSensingAPITests
{


    namespace MoodSensingApp.Tests.Controllers
    {
        [TestFixture]
        public class LocationsControllerTests
        {
            [Test]
            public async Task GetClosestHappyLocation_ValidUser_ReturnsClosestHappyLocation()
            {
                var locationServiceMock = new Mock<ILocationService>();
                var moodServiceMock = new Mock<IMoodService>();
                var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

                var controller = new LocationsController(locationServiceMock.Object, httpContextAccessorMock.Object, moodServiceMock.Object);

               
                var httpContextMock = new Mock<HttpContext>();
                httpContextAccessorMock.Setup(m => m.HttpContext).Returns(httpContextMock.Object);

                
                var connectionInfoMock = new Mock<ConnectionInfo>();
                connectionInfoMock.Setup(c => c.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1"));

                
                httpContextMock.Setup(c => c.Connection).Returns(connectionInfoMock.Object);

               
                var userCurrentLocation = new Location
                {
                    Latitude = 28.987509,
                    Longitude = 79.414124
                };
                locationServiceMock.Setup(l => l.GetUserLocation("127.0.0.1")).ReturnsAsync(userCurrentLocation);

                
                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                     new Claim("UserId", "1"),
                  
                }));

               
                httpContextMock.Setup(c => c.User).Returns(user);

             
                var happyMoodCaptures = new List<MoodCapture>
            {
                new MoodCapture
                {
                    Location = new Location
                    {
                        Latitude = 28.987509,
                        Longitude = 79.414124,
                        City = "City A"
                    },
                    Mood = "Joy"
                },
                new MoodCapture
                {
                    Location = new Location
                    {
                        Latitude = 28.987511,
                        Longitude = 79.414126,
                        City = "City B"
                    },
                    Mood = "Joy"
                }
                };
                moodServiceMock.Setup(m => m.GetMoodCaptures(It.IsAny<int>(), "Joy")).ReturnsAsync(happyMoodCaptures);
                var controllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                };
                controller.ControllerContext = controllerContext;
                // Act
                var result = await controller.GetClosestHappyLocation();

                // Assert
                var okResult = (OkObjectResult)result;
                
                var response = okResult.Value;
                Assert.IsNotNull(response);
            }


            [Test]
            public async Task GetClosestHappyLocation_NoHappyMoodCaptures_ReturnsNoContent()
            {
                var locationServiceMock = new Mock<ILocationService>();
                var moodServiceMock = new Mock<IMoodService>();
                var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

                var controller = new LocationsController(locationServiceMock.Object, httpContextAccessorMock.Object, moodServiceMock.Object);


                var httpContextMock = new Mock<HttpContext>();
                httpContextAccessorMock.Setup(m => m.HttpContext).Returns(httpContextMock.Object);


                var connectionInfoMock = new Mock<ConnectionInfo>();
                connectionInfoMock.Setup(c => c.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1"));


                httpContextMock.Setup(c => c.Connection).Returns(connectionInfoMock.Object);


                var userCurrentLocation = new Location
                {
                    Latitude = 28.987509,
                    Longitude = 79.414124,
                    City = "City"
                };
                locationServiceMock.Setup(l => l.GetUserLocation("127.0.0.1")).ReturnsAsync(userCurrentLocation);


                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                     new Claim("UserId", "1"),

                }));


                httpContextMock.Setup(c => c.User).Returns(user);
                var controllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                };
                controller.ControllerContext = controllerContext;
                var happyMoodCaptures = new List<MoodCapture>();
                moodServiceMock.Setup(m => m.GetMoodCaptures(It.IsAny<int>(), "Joy")).ReturnsAsync(happyMoodCaptures);

                // Act
                var result = await controller.GetClosestHappyLocation();

                // Assert
                Assert.IsInstanceOf<NotFoundResult>(result);
            }  
        }
    }

}
