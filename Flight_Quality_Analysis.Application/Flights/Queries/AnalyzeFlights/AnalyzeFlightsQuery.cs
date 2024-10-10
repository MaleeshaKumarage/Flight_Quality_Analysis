using Flight_Quality_Analysis.Application.Flights.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Application.Flights.Queries.AnalyzeFlights
{
    public class AnalyzeFlightsQuery : IRequest<List<FlightDto>>
    {
    }
}
