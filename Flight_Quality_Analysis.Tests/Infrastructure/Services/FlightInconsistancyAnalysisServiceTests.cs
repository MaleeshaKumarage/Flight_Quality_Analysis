using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Infrastructure.Services.FileReadingService;
using Flight_Quality_Analysis.Infrastructure.Services.FlightAnalysisService;
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
    public class FlightInconsistancyAnalysisServiceTests
    {
        private FlightInconsistancyAnalysisService _flightInconsistancyAnalysisService;
        private CsvReadingService _csvReadingService;
        private Mock<ICsvReadingService> _mockCsvReadingService;
        [SetUp]
        public void SetUp()
        {
            _flightInconsistancyAnalysisService = new FlightInconsistancyAnalysisService();
            _csvReadingService = new CsvReadingService();
            _mockCsvReadingService = new Mock<ICsvReadingService>();
        }
        [Test]
        public async Task GetInconsistentFlightsAsync_ShouldReturnInconsistentFlights()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR" },
                new Flight { Id = 2, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "CDG", ArrivalAirport = "JFK" }
            };
            _mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync()).ReturnsAsync(flights);

            // Act

            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().HaveCount(1);
            result[0].FlightNumber.Should().Be("JFK");
        }
    }
}
