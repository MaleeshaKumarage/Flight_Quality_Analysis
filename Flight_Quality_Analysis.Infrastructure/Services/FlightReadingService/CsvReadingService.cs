using Flight_Quality_Analysis.Domain.Entity;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public CsvReadingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Flight>> ReadFlightsFromCsvAsync(string? filePath = null)
        {
            filePath = filePath ?? _configuration["CsvFilePath"];
            var flights = new List<Flight>();
            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);
                //Skip Header
                foreach (var line in lines.Skip(1))
                {
                    try
                    {//Skip Empty Lines
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var values = line.Split(',').Select(v => v.Trim()).ToArray();
                        //Validate Number of columns
                        if (values.Length != 8)
                            throw new FormatException("Invalid number of columns in CSV line.");

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
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing line: {line}. Error: {ex.Message}");
                        continue;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Unexpected error with line: {line}. Error: {ex.Message}");
                        continue;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {filePath}. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {filePath}. Error: {ex.Message}");
            }

            return flights;
        }


    }
}

