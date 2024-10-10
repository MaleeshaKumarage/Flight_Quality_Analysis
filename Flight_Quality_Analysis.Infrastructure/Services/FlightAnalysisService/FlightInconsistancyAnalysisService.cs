using Flight_Quality_Analysis.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Infrastructure.Services.FlightAnalysisService
{
    public class FlightInconsistancyAnalysisService : IFlightInconsistancyAnalysisService
    {
        public List<Flight> FindInconsistentFlights(List<Flight> flights)
        {

            var inconsistentFlights = new List<Flight>();
            //Flights are grouped based on its Registration Number
            var groupedFlights = flights.GroupBy(f => f.AircraftRegistrationNumber);

            foreach (var group in groupedFlights)
            {   //Each group Order by Departure Time
                //This will return List of flights ordered by departure time spesific to registrationNumber
                var orderedFlights = group.OrderBy(f => f.DepartureDateTime).ToList();

                for (int i = 1; i < orderedFlights.Count; i++)
                {//Check the adjacent flight's Arrival and Depature to identify is there any inconsitancy
                    var previousFlight = orderedFlights[i - 1];
                    var currentFlight = orderedFlights[i];

                    if (previousFlight.ArrivalAirport != currentFlight.DepartureAirport)
                    {
                        inconsistentFlights.Add(currentFlight);
                    }
                }
            }

            return inconsistentFlights;

        }
    }
}
