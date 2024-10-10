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

        [SetUp]
        public void SetUp()
        {
            _flightInconsistancyAnalysisService = new FlightInconsistancyAnalysisService();
        }

        [Test]
        public void FindInconsistentFlights_ShouldReturnAirportMismatchInconsistency()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR", DepartureDateTime = DateTime.Now.AddHours(-4), ArrivalDateTime = DateTime.Now.AddHours(-2) },
                new Flight { Id = 2, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "CDG", ArrivalAirport = "JFK", DepartureDateTime = DateTime.Now.AddHours(1), ArrivalDateTime = DateTime.Now.AddHours(4) }
            };

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[1]);
            result[flights[1]].Should().Contain("Arrival and departure airport mismatch.");
        }

        [Test]
        public void FindInconsistentFlights_ShouldReturnTimeOverlapInconsistency()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR", DepartureDateTime = DateTime.Now.AddHours(-4), ArrivalDateTime = DateTime.Now.AddHours(-2) },
                new Flight { Id = 2, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "LHR", ArrivalAirport = "JFK", DepartureDateTime = DateTime.Now.AddHours(-3), ArrivalDateTime = DateTime.Now.AddHours(2) }
            };

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[1]);
            result[flights[1]].Should().Contain("Time inconsistency: Departure before previous arrival or overlapping flights.");
        }

        [Test]
        public void FindInconsistentFlights_ShouldReturnNegativeDurationInconsistency()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "LHR", DepartureDateTime = DateTime.Now.AddHours(-4), ArrivalDateTime = DateTime.Now.AddHours(-5) }
            };

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[0]);
            result[flights[0]].Should().Contain("Negative duration: Arrival time is before departure time.");
        }

        [Test]
        public void FindInconsistentFlights_ShouldReturnBackToBackAirportMatchInconsistency()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "HEL", DepartureDateTime = DateTime.Now.AddHours(-4), ArrivalDateTime = DateTime.Now.AddHours(-2) }
            };

            // Act
            var result = _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);

            // Assert
            result.Should().ContainKey(flights[0]);
            result[flights[0]].Should().Contain("Back-to-back airport match: Departure and arrival airports are the same.");
        }


    }
}