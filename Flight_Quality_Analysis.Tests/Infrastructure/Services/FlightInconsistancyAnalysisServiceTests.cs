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
        private Mock<ICsvReadingService> _mockCsvReadingService;

        [SetUp]
        public void SetUp()
        {
            _flightInconsistancyAnalysisService = new FlightInconsistancyAnalysisService();
            _mockCsvReadingService = new Mock<ICsvReadingService>();
        }

        [Test]
        public async Task GetInconsistentFlightsAsync_ShouldReturnInconsistentFlights_WhenNegativeDurationExists()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR", DepartureDateTime = new DateTime(2024, 10, 10, 12, 00, 00), ArrivalDateTime = new DateTime(2024, 10, 10, 11, 30, 00) }, // Negative duration
            };
            _mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync(null)).ReturnsAsync(flights);

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[0]);
            result[flights[0]].Should().Contain("Negative duration");
        }

        [Test]
        public async Task GetInconsistentFlightsAsync_ShouldReturnInconsistentFlights_WhenAirportMismatchExists()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR", DepartureDateTime = new DateTime(2024, 10, 10, 12, 00, 00), ArrivalDateTime = new DateTime(2024, 10, 10, 14, 00, 00) },
                new Flight { Id = 2, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "CDG", ArrivalAirport = "JFK", DepartureDateTime = new DateTime(2024, 10, 10, 16, 00, 00), ArrivalDateTime = new DateTime(2024, 10, 10, 18, 00, 00) }, // Airport mismatch (LHR -> CDG)
            };
            _mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync(null)).ReturnsAsync(flights);

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[1]);
            result[flights[1]].Should().Contain("Arrival and departure airport mismatch");
        }

        [Test]
        public async Task GetInconsistentFlightsAsync_ShouldReturnInconsistentFlights_WhenTimeOverlapExists()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR", DepartureDateTime = new DateTime(2024, 10, 10, 12, 00, 00), ArrivalDateTime = new DateTime(2024, 10, 10, 14, 00, 00) },
                new Flight { Id = 2, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "LHR", ArrivalAirport = "JFK", DepartureDateTime = new DateTime(2024, 10, 10, 13, 30, 00), ArrivalDateTime = new DateTime(2024, 10, 10, 17, 00, 00) }, // Time overlap
            };
            _mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync(null)).ReturnsAsync(flights);

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[1]);
            result[flights[1]].Should().Contain("Time inconsistency");
        }

        [Test]
        public async Task GetInconsistentFlightsAsync_ShouldReturnInconsistentFlights_WhenBackToBackAirportMatchExists()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "HEL", DepartureDateTime = new DateTime(2024, 10, 10, 12, 00, 00), ArrivalDateTime = new DateTime(2024, 10, 10, 14, 00, 00) }, // Back-to-back airport match
            };
            _mockCsvReadingService.Setup(s => s.ReadFlightsFromCsvAsync(null)).ReturnsAsync(flights);

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[0]);
            result[flights[0]].Should().Contain("Back-to-back airport match");
        }
    }
}
