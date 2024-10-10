using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Infrastructure.Services.FileReadingService;
using FluentAssertions;
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
        private Mock<ICsvReadingService> _mockCsvReadingService;
        [SetUp]
        public void SetUp()
        {
            _csvReadingService = new CsvReadingService();
            _mockCsvReadingService = new Mock<ICsvReadingService>();
        }

        [Test]
        public async Task GetAllFlightsAsync_ShouldReturnFlightsFromCsvService()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "DXB" }
            };
            _mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync()).ReturnsAsync(flights);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().BeEquivalentTo(flights);
        }

        [Test]
        public async Task ReadFlightsFromCsvAsync_ShouldReturnCorrectFlightData()
        {
            // Arrange
            var csvData = @"id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime
1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27";

            var path = Path.GetTempFileName();
            await File.WriteAllTextAsync(path, csvData);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync();

            // Assert
            result.Should().HaveCount(1);
            result[0].AircraftRegistrationNumber.Should().Be("ZX-IKD");
            result[0].DepartureAirport.Should().Be("HEL");


            File.Delete(path);
        }
    }

}
