using Microsoft.AspNetCore.Mvc;
using ReservationSystem.DTOs;
using ReservationSystem.Models;

[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;
    private readonly ILogger<VenuesController> _logger;

    public VenuesController(IVenueService venueService, ILogger<VenuesController> logger)
    {
        _venueService = venueService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Venue>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Venue>>> GetAllVenues()
    {
        try
        {
            var venues = await _venueService.GetAllVenues();
            _logger.LogInformation($"Retrieved {venues.Count} venues");
            return Ok(venues);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fejl ved hentning af venues: {ex.Message}");
            return StatusCode(500, new { message = "Serverfejl ved hentning af venues" });
        }
    }
     
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Venue))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Venue>> GetVenueById(int id)
    {
        try
        {
            var venue = await _venueService.GetVenueById(id);
            if (venue == null)
            {
                _logger.LogWarning($"Venue med ID {id} blev ikke fundet");
                return NotFound(new { message = $"Venue med ID {id} blev ikke fundet" });
            }

            _logger.LogInformation($"Retrieved venue {id}: {venue.Name}");
            return Ok(venue);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fejl ved hentning af venue: {ex.Message}");
            return StatusCode(500, new { message = "Serverfejl ved hentning af venue" });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Venue))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Venue>> CreateVenue([FromBody] CreateVenueDto request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Validerings fejl ved venue oprettelse");
            return BadRequest(ModelState);
        }

        try
        {
            var venue = await _venueService.CreateVenue(request.Name, request.VenueType, request.Capacity);
            
            if (venue == null)
            {
                _logger.LogError($"Kunne ikke oprette venue: {request.Name}");
                return BadRequest(new { message = "Kunne ikke oprette venue - prøv igen" });
            }

            _logger.LogInformation($"Venue oprettet: {venue.Name} ({venue.VenueType}) med kapacitet {request.Capacity}");
            return CreatedAtAction(nameof(GetVenueById), new { id = venue.VenueId }, venue);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fejl ved oprettelse af venue: {ex.Message}");
            return StatusCode(500, new { message = "Serverfejl ved oprettelse af venue" });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateVenue(int id, [FromBody] UpdateVenueDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Validerings fejl ved venue opdatering {id}");
            return BadRequest(ModelState);
        }

        try
        {
            var success = await _venueService.UpdateVenue(id, updateDto);
            
            if (!success)
            {
                _logger.LogWarning($"Venue med ID {id} blev ikke fundet");
                return NotFound(new { message = $"Venue med ID {id} blev ikke fundet" });
            }

            _logger.LogInformation($"Venue {id} blev opdateret");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fejl ved opdatering af venue: {ex.Message}");
            return StatusCode(500, new { message = "Serverfejl ved opdatering af venue" });
        }
    }

    [HttpGet("{id}/seats")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Seat>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Seat>>> GetVenueSeats(int id)
    {
        try
        {
            var seats = await _venueService.GetVenueSeats(id);
            _logger.LogInformation($"Retrieved {seats.Count} seats for venue {id}");
            return Ok(seats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fejl ved hentning af pladser: {ex.Message}");
            return StatusCode(500, new { message = "Serverfejl ved hentning af pladser" });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteVenue(int id)
    {
        try
        {
            var success = await _venueService.DeleteVenue(id);
            if (!success)
            {
                _logger.LogWarning($"Venue med ID {id} blev ikke fundet");
                return NotFound(new { message = $"Venue med ID {id} blev ikke fundet" });
            }

            _logger.LogInformation($"Venue {id} blev slettet");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fejl ved sletning af venue: {ex.Message}");
            return StatusCode(500, new { message = "Serverfejl ved sletning af venue" });
        }
    }
}
