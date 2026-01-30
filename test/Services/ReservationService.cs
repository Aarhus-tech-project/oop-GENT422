using Microsoft.EntityFrameworkCore;
using ReservationSystem.DTOs;
using ReservationSystem.Models;

namespace ReservationSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationDbContext _context;
        private readonly Random _random = new Random();

        public ReservationService(ReservationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            return await _context.Reservations
                .Include(r => r.Venue)
              
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationById(int reservationId)
        {
            return await _context.Reservations
                .Include(r => r.Venue)
               
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }

        public async Task<List<Reservation>> GetReservationsByVenue(int venueId)
        {
            return await _context.Reservations
                .Where(r => r.VenueId == venueId)
                .Include(r => r.Venue)
             
                .ToListAsync();
        }

        public async Task<Reservation?> CreateReservation(int venueId, string customerName, int seatCount)
        {
            try
            {
                var venue = await _context.Venues.FindAsync(venueId);
                if (venue == null)
                    throw new InvalidOperationException("Venue ikke fundet");

                var reservation = new Reservation
                {
                    VenueId = venueId,
                    CustomerName = customerName,
                    ReservationDate = DateTime.Now,
                    PersonCount = seatCount,
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return reservation;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Fejl ved oprettelse: {ex.Message}");
            }
        }

        public async Task<(bool success, string? errorMessage)> UpdateReservation(int id, UpdateReservationDto updateDto)
        {
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == id);
                if (reservation == null)
                    return (false, $"Reservation med ID {id} blev ikke fundet");

                if (!string.IsNullOrWhiteSpace(updateDto.CustomerName))
                    reservation.CustomerName = updateDto.CustomerName;

                if (updateDto.PersonCount.HasValue && updateDto.PersonCount > 0)
                    reservation.PersonCount = updateDto.PersonCount.Value;

                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Databasefejl: {ex.Message}");
            }
        }

        public async Task<bool> DeleteReservation(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                    return false;

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelReservation(int reservationId)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(reservationId);
                if (reservation == null)
                    return false;

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Seat?> FindBestSeat(int venueId, int personCount)
        {
            var availableSeats = await _context.Seats
                .Where(s => s.VenueId == venueId && !s.IsBooked && s.Capacity >= personCount)
                .ToListAsync();

            if (!availableSeats.Any())
                return null;

            var randomIndex = _random.Next(availableSeats.Count);
            return availableSeats[randomIndex];
        }
    }
}
