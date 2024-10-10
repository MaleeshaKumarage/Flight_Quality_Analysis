using Flight_Quality_Analysis.Domain.Entity;
using Flight_Quality_Analysis.Domain.Repository;
using Flight_Quality_Analysis.Infrastructure.Services.FileReadingService;
using Flight_Quality_Analysis.Infrastructure.Services.FlightAnalysisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Infrastructure.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ICsvReadingService _csvReadingService;
        private readonly IFlightInconsistancyAnalysisService _flightInconsistancyAnalysisService;

        public FlightRepository(ICsvReadingService csvReadingService, IFlightInconsistancyAnalysisService flightInconsistancyAnalysisService)
        {
            _csvReadingService = csvReadingService;
            _flightInconsistancyAnalysisService = flightInconsistancyAnalysisService;
        }

        public async Task<List<Flight>> GetAllFlightsAsync()
        {

            return await _csvReadingService.ReadFlightsFromCsvAsync();


        }

        public async Task<Dictionary<Flight, string>> GetInconsistentFlightsAsync()
        {
            var flights = await _csvReadingService.ReadFlightsFromCsvAsync();
            return _flightInconsistancyAnalysisService.FindInconsistentFlights(flights);
        }
    }
}
