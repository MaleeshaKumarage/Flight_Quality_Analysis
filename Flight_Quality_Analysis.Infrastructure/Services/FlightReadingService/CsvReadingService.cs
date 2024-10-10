using Flight_Quality_Analysis.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Infrastructure.Services.FileReadingService
{
    public class CsvReadingService : ICsvReadingService
    {
        private const string CsvFilePath = "C:\\Users\\malee\\source\\repos\\Flight_Quality_Analysis\\Flight_Quality_Analysis.Infrastructure\\Data\\flights.csv";

        public async Task<List<Flight>> ReadFlightsFromCsvAsync(string? filePath = null)
        {
            filePath = filePath ?? CsvFilePath;
            var flights = new List<Flight>();
            var lines = await File.ReadAllLinesAsync(filePath);

            foreach (var line in lines.Skip(1)) // Skip header row
            {
                var values = line.Split(',');
                var flight = new Flight
                {
                    Id = int.Parse(values[0]),
                    AircraftRegistrationNumber = values[1],
                    AircraftType = values[2],
                    FlightNumber = values[3],
                    DepartureAirport = values[4],
                    DepartureDateTime = DateTime.ParseExact(values[5], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    ArrivalAirport = values[6],
                    ArrivalDateTime = DateTime.ParseExact(values[7], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };
                flights.Add(flight);
            }

            return flights;
        }
    }
}

