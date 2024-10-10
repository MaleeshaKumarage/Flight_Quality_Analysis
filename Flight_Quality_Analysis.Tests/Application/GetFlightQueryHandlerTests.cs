using AutoMapper;
using Flight_Quality_Analysis.Application.Flights.Queries.GetFlights;
using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Domain.Repository;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Tests.Application
{
    public class GetFlightQueryHandlerTests
    {
        private Mock<IFlightRepository> _mockFlightRepository;
        private Mock<IMapper> _mapper;
        private GetFlightsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockFlightRepository = new Mock<IFlightRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetFlightsQueryHandler(_mockFlightRepository.Object, _mapper.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnFlightList()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, AircraftRegistrationNumber = "ZX-IKD", DepartureAirport = "HEL", ArrivalAirport = "DXB" }
            };
            _mockFlightRepository.Setup(r => r.GetAllFlightsAsync()).ReturnsAsync(flights);

            // Act
            var result = await _handler.Handle(new GetFlightsQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(flights);
        }
    }
}
