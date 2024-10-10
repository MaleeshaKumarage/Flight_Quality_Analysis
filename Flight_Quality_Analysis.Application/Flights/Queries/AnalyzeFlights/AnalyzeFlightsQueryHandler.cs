using AutoMapper;
using Flight_Quality_Analysis.Application.Flights.Common;
using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Application.Flights.Queries.AnalyzeFlights
{
    public class AnalyzeFlightsQueryHandler : IRequestHandler<AnalyzeFlightsQuery, List<FlightDto>>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public AnalyzeFlightsQueryHandler(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<List<FlightDto>> Handle(AnalyzeFlightsQuery request, CancellationToken cancellationToken)
        {
            var inconsistentFlights = await _flightRepository.GetInconsistentFlightsAsync();
            return _mapper.Map<List<FlightDto>>(inconsistentFlights);
        }
    }
}
