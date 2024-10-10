using AutoMapper;
using Flight_Quality_Analysis.Application.Flights.Common;
using Flight_Quality_Analysis.Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Application.Flights.Queries.GetFlights
{
    public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, List<FlightInconsistancyDto>>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public GetFlightsQueryHandler(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }
        public async Task<List<FlightInconsistancyDto>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
        {
            var flights = await _flightRepository.GetAllFlightsAsync();
            return _mapper.Map<List<FlightInconsistancyDto>>(flights);


        }
    }
}
