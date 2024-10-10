using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Infrastructure.Services.FileReadingService;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Tests.Infrastructure.Services
{
    [TestFixture]
    public class CsvReadingServiceTests
    {

        private CsvReadingService _csvReadingService;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IHostEnvironment> _mockHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.SetupGet(c => c["CsvFilePath"]).Returns("fake/path/to/flights.csv");

            _mockHostEnvironment = new Mock<IHostEnvironment>();
            _mockHostEnvironment.SetupGet(h => h.ContentRootPath).Returns("/root/path");

            _csvReadingService = new CsvReadingService(_mockConfiguration.Object, _mockHostEnvironment.Object);

        }

        [Test]
        public async Task GetAllFlightsAsync_ShouldReturnFlightsFromCsvService()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "DXB" }
            };
            var mockCsvReadingService = new Mock<ICsvReadingService>();
            mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync(null)).ReturnsAsync(flights);

            // Act
            var result = await mockCsvReadingService.Object.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().BeEquivalentTo(flights);
        }

        [Test]
        public async Task ReadFlightsFromCsvAsync_ShouldReturnCorrectFlightData()
        {
            // Arrange
            var csvData = @"id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime
1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27";

            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, csvData);

            _mockConfiguration.SetupGet(c => c["CsvFilePath"]).Returns(tempFilePath);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().HaveCount(1);
            result[0].AircraftRegistrationNumber.Should().Be("ZX-IKD");
            result[0].DepartureAirport.Should().Be("HEL");

            File.Delete(tempFilePath);
        }

        [Test]
        public async Task ReadFlightsFromCsvAsync_ShouldHandleFileNotFound()
        {
            // Arrange
            _mockConfiguration.SetupGet(c => c["CsvFilePath"]).Returns("non/existent/file.csv");

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task ReadFlightsFromCsvAsync_ShouldHandleInvalidCsvFormat()
        {
            // Arrange
            var csvData = @"id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime
1,ZX-IKD,350,M645,HEL,invalid_date,DXB,2024-01-03 02:31:27"; // Invalid date format

            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, csvData);

            _mockConfiguration.SetupGet(c => c["CsvFilePath"]).Returns(tempFilePath);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().BeEmpty();
            File.Delete(tempFilePath);
        }

        [Test]
        public async Task ReadFlightsFromCsvAsync_ShouldUseProvidedFilePath()
        {
            // Arrange
            var csvData = @"id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime
1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27";

            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, csvData);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync(tempFilePath);

            // Assert
            result.Should().HaveCount(1);
            result[0].AircraftRegistrationNumber.Should().Be("ZX-IKD");

            File.Delete(tempFilePath);
        }

        [Test]
        public async Task ReadFlightsFromCsvAsync_ShouldHandleEmptyLinesInCsv()
        {
            // Arrange
            var csvData = @"id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime

1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27
";

            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, csvData);

            _mockConfiguration.SetupGet(c => c["CsvFilePath"]).Returns(tempFilePath);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().HaveCount(1);
            result[0].AircraftRegistrationNumber.Should().Be("ZX-IKD");

            File.Delete(tempFilePath);
        }
    }

}
