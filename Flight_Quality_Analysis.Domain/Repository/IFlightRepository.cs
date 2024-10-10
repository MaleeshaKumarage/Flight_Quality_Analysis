using Flight_Quality_Analysis.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Domain.Repository
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllFlightsAsync();
        Task<Dictionary<Flight, string>> GetInconsistentFlightsAsync();
    }
}
