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
        public Dictionary<Flight, string> FindInconsistentFlights(List<Flight> flights)
        {
            var inconsistentFlightsWithReason = new Dictionary<Flight, string>();

            // Group flights by aircraft registration number
            var groupedFlights = flights.GroupBy(f => f.AircraftRegistrationNumber);

            foreach (var group in groupedFlights)
            {
                // Order each group's flights by departure time
                var orderedFlights = group.OrderBy(f => f.DepartureDateTime).ToList();

                for (int i = 0; i < orderedFlights.Count; i++)
                {
                    var currentFlight = orderedFlights[i];

                    // Check negative duration for each flight
                    CheckNegativeDuration(currentFlight, inconsistentFlightsWithReason);

                    // Check for back-to-back airport matches
                    CheckBackToBackAirportMatch(currentFlight, inconsistentFlightsWithReason);

                    // Check for inconsistencies between consecutive flights
                    if (i > 0)
                    {
                        var previousFlight = orderedFlights[i - 1];

                        // Check for airport inconsistency
                        CheckAirportInconsistency(previousFlight, currentFlight, inconsistentFlightsWithReason);

                        // Check for time inconsistency and overlap
                        CheckTimeAndOverlapInconsistency(previousFlight, currentFlight, inconsistentFlightsWithReason);

                        // Check for back-to-back airport matches
                        CheckBackToBackAirportMatch(currentFlight, inconsistentFlightsWithReason);
                    }
                }
            }

            return inconsistentFlightsWithReason;
        }

        private void CheckAirportInconsistency(Flight previousFlight, Flight currentFlight, Dictionary<Flight, string> inconsistencies)
        {
            if (previousFlight.ArrivalAirport != currentFlight.DepartureAirport)
            {
                AddInconsistency(currentFlight, "Arrival and departure airport mismatch.", inconsistencies);
            }
        }

        private void CheckTimeAndOverlapInconsistency(Flight previousFlight, Flight currentFlight, Dictionary<Flight, string> inconsistencies)
        {
            if (currentFlight.DepartureDateTime < previousFlight.ArrivalDateTime)
            {
                AddInconsistency(currentFlight, "Time inconsistency: Departure before previous arrival or overlapping flights.", inconsistencies);
            }
        }

        private void CheckNegativeDuration(Flight currentFlight, Dictionary<Flight, string> inconsistencies)
        {
            if (currentFlight.ArrivalDateTime < currentFlight.DepartureDateTime)
            {
                AddInconsistency(currentFlight, "Negative duration: Arrival time is before departure time.", inconsistencies);
            }
        }

        private void CheckBackToBackAirportMatch(Flight currentFlight, Dictionary<Flight, string> inconsistencies)
        {
            if (currentFlight.DepartureAirport == currentFlight.ArrivalAirport)
            {
                AddInconsistency(currentFlight, "Back-to-back airport match: Departure and arrival airports are the same.", inconsistencies);
            }
        }

        private void AddInconsistency(Flight flight, string reason, Dictionary<Flight, string> inconsistencies)
        {
            if (!inconsistencies.ContainsKey(flight))
            {
                inconsistencies[flight] = reason;
            }
            else
            {
                inconsistencies[flight] += $" {reason}";
            }
        }
    }
}
