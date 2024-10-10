using Flight_Quality_Analysis.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Infrastructure.Services.FileReadingService
{
    public interface ICsvReadingService
    {
        Task<List<Flight>> ReadFlightsFromCsvAsync();
    }
}
