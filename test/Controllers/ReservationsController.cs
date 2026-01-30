using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services;
using ReservationSystem.DTOs;

namespace ReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDto>>> GetAllReservations()
        {
            try  
            {
                var reservations = await _reservationService.GetAllReservations();
                var reservationDtos = reservations.Select(r => new ReservationDto
                {
                    ReservationId = r.ReservationId,
                    VenueId = r.VenueId,
                    CustomerName = r.CustomerName,
                    ReservationDate = r.ReservationDate,
                    VenueName = r.Venue?.Name ?? "N/A",
                    SeatCount = r.PersonCount,
                }).ToList();
               Console.WriteLine(reservationDtos.FirstOrDefault());
                return Ok(reservationDtos);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Business logic fejl: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fejl ved hentning af reservationer: {ex.Message}");
                return StatusCode(500, new { error = "Kunne ikke hente reservationer", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservationById(int id)
        {
            try
            {
                var reservations = await _reservationService.GetAllReservations();
                var reservation = reservations.FirstOrDefault(r => r.ReservationId == id);

                if (reservation == null)
                {
                    return NotFound(new { error = $"Reservation med ID {id} findes ikke" });
                }

                var reservationDto = new ReservationDto
                {
                    ReservationId = reservation.ReservationId,
                    VenueId = reservation.VenueId,
                    CustomerName = reservation.CustomerName,
                    ReservationDate = reservation.ReservationDate,
                
                    VenueName = reservation.Venue?.Name ?? "N/A",
                  
                };

                return Ok(reservationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fejl ved hentning af reservation {id}: {ex.Message}");
                return StatusCode(500, new { error = "Kunne ikke hente reservation", details = ex.Message });
            }
        }

        [HttpGet("venue/{venueId}")]
        public async Task<ActionResult<List<ReservationDto>>> GetReservationsByVenue(int venueId)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByVenue(venueId);
                var reservationDtos = reservations.Select(r => new ReservationDto
                {
                    ReservationId = r.ReservationId,
                    VenueId = r.VenueId,
                    CustomerName = r.CustomerName,
                    ReservationDate = r.ReservationDate,
                    VenueName = r.Venue?.Name ?? "N/A",
                    
                }).ToList();

                return Ok(reservationDtos);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Business logic fejl: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fejl ved hentning af reservationer for venue {venueId}: {ex.Message}");
                return StatusCode(500, new { error = "Kunne ikke hente reservationer", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] CreateReservationDto createReservationDto)
        {
            try
            {
                if (createReservationDto.VenueId <= 0)
                {
                    return BadRequest(new { error = "Lokation ID er påkrævet" });
                }

                if (string.IsNullOrWhiteSpace(createReservationDto.CustomerName))
                {
                    return BadRequest(new { error = "Kundenavn er påkrævet" });
                }

                if (createReservationDto.SeatCount <= 0)
                {
                    return BadRequest(new { error = "Antal pladser skal være større end 0" });
                }

                var reservation = await _reservationService.CreateReservation(
                    createReservationDto.VenueId,
                    createReservationDto.CustomerName,
                    createReservationDto.SeatCount
                );

                var reservationDto = new ReservationDto
                {
                    ReservationId = reservation.ReservationId,
                    VenueId = reservation.VenueId,
                    CustomerName = reservation.CustomerName,
                    ReservationDate = reservation.ReservationDate,
                    VenueName = reservation.Venue?.Name ?? "N/A",
                    SeatCount = reservation.PersonCount,
                   
                };

                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.ReservationId }, reservationDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validerings fejl: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Business logic fejl: {ex.Message}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fejl ved oprettelse af reservation: {ex.Message}");
                return StatusCode(500, new { error = "Kunne ikke oprette reservation", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                var result = await _reservationService.DeleteReservation(id);
                if (!result)
                {
                    return NotFound(new { error = $"Reservation med ID {id} findes ikke" });
                }

                return Ok(new { message = "Reservation slettet med succes" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fejl ved sletning af reservation {id}: {ex.Message}");
                return StatusCode(500, new { error = "Kunne ikke slette reservation", details = ex.Message });
            }
        }
    }
}
