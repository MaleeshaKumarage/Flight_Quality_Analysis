using Flight_Quality_Analysis.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Infrastructure.Services.FlightAnalysisService
{
    public interface IFlightInconsistancyAnalysisService
    {
        Dictionary<Flight, string> FindInconsistentFlights(List<Flight> flights);
    }
}
