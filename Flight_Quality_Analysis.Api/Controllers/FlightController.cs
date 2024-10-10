using Flight_Quality_Analysis.Application.Flights.Queries.AnalyzeFlights;
using Flight_Quality_Analysis.Application.Flights.Queries.GetFlights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight_Quality_Analysis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ApiController
    {
        [HttpGet("allflights")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var flights = await Mediator.Send(new GetFlightsQuery());
                if (flights == null || flights.Count == 0)
                {
                    return NotFound(new { Message = "No flights found." });
                }
                return Ok(flights);
            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching flights.", Details = ex.Message });
            }
        }

        [HttpGet("inconsistentflights")]
        public async Task<IActionResult> GetInconsitantFlightsAsync()
        {
            try
            {
                var inconsistantFlights = await Mediator.Send(new AnalyzeFlightsQuery());
                if (inconsistantFlights == null || inconsistantFlights.Count == 0)
                {
                    return NotFound(new { Message = "No inconsitanat flights found." });
                }
                return Ok(inconsistantFlights);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching inconsitanat flights.", Details = ex.Message });
            }
        }
    }
}
