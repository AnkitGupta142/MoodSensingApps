using NUnit.Framework;
using MoodSensingApp.Models;
using MoodSensingApp.Repositories;
using MoodSensingApp.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MoodSensingAPITests
{
    [TestFixture]
    public class MoodServiceTests
    {
        private MoodService _moodService;
        private Mock<IMoodRepository> _moodRepositoryMock;
        private Mock<ILocationRepository> _locationRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _moodRepositoryMock = new Mock<IMoodRepository>();
            _locationRepositoryMock = new Mock<ILocationRepository>();
            _moodService = new MoodService(_moodRepositoryMock.Object, _locationRepositoryMock.Object);
        }

        [Test]
        public async Task GetMoodFrequencyDistribution_ValidUserId_ReturnsMoodFrequencyDistribution()
        {
            // Arrange
            int userId = 1;
            var moodCaptures = new List<MoodCapture>
            {
                new MoodCapture { Mood = "Joy" },
                new MoodCapture { Mood = "Sadness" },
                new MoodCapture { Mood = "Joy" },
                new MoodCapture { Mood = "Anger" }
            };

            _moodRepositoryMock.Setup(m => m.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<MoodCapture, bool>>>()))
                .ReturnsAsync(moodCaptures);

            // Act
            var result = await _moodService.GetMoodFrequencyDistribution(userId);

            // Assert
            Assert.AreEqual(2, result["Joy"]);
            Assert.AreEqual(1, result["Sadness"]);
            Assert.AreEqual(1, result["Anger"]);
        }

        [Test]
        public async Task InsertMoodCapture_ExistingLocation_LocationIsSet()
        {
            // Arrange
            var existingLocation = new Location { LocationId = 1 };
            var request = new MoodCapture { Location = existingLocation };
            _locationRepositoryMock.Setup(l => l.GetByCoordinates(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(existingLocation);

            // Act
            await _moodService.InsertMoodCapture(request);

            // Assert
            Assert.AreEqual(existingLocation, request.Location);
        }

        [Test]
        public async Task InsertMoodCapture_NewLocation_LocationIsAddedAndIdIsSet()
        {
            // Arrange
            var location = new Location
            {
                Name = "New York",
                Latitude = 40.7128,
                Longitude = -74.0060,
                City = "New York City"
            };

            _locationRepositoryMock.Setup(l => l.GetByCoordinates(location.Latitude, location.Longitude))
                .ReturnsAsync((double latitude, double longitude) => null);

            _locationRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Location>()))
                .Returns((Location newLocation) =>
                {
                    newLocation.LocationId = 1;
                    return Task.CompletedTask;
                });

            var moodCapture = new MoodCapture
            {
                Location = location
            };

            // Act
            await _moodService.InsertMoodCapture(moodCapture);

            // Assert
            Assert.AreEqual(1, moodCapture.LocationId);
          
        }


        [Test]
        public async Task GetMoodCaptures_ValidUserIdAndMood_ReturnsMatchingMoodCaptures()
        {
            // Arrange
            int userId = 1;
            string mood = "Joy";
            var moodCaptures = new List<MoodCapture>
              {
                  new MoodCapture { UserId = userId, Mood = "Joy" },
                  new MoodCapture { UserId = userId, Mood = "Joy" }
             };

            _moodRepositoryMock.Setup(m => m.GetAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<MoodCapture, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(moodCaptures);

            // Act
            var result = await _moodService.GetMoodCaptures(userId, mood);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(mood, result[0].Mood);
            Assert.AreEqual(mood, result[1].Mood);
        }

    }
}