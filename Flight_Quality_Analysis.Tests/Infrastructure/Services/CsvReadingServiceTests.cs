﻿using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Infrastructure.Services.FileReadingService;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
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
        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.SetupGet(c => c["CsvFilePath"]).Returns("fake/path/to/flights.csv");

            _csvReadingService = new CsvReadingService(_mockConfiguration.Object);

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

            var path = Path.GetTempFileName();
            await File.WriteAllTextAsync(path, csvData);

            // Act
            var result = await _csvReadingService.ReadFlightsFromCsvAsync(path);

            // Assert
            result.Should().HaveCount(1);
            result[0].AircraftRegistrationNumber.Should().Be("ZX-IKD");
            result[0].DepartureAirport.Should().Be("HEL");


            File.Delete(path);
        }
    }

}
