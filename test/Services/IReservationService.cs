using ReservationSystem.DTOs;
using ReservationSystem.Models;

namespace ReservationSystem.Services
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllReservations();
        Task<Reservation?> GetReservationById(int reservationId);
        Task<List<Reservation>> GetReservationsByVenue(int venueId);
        Task<Reservation?> CreateReservation(int venueId, string customerName, int seatCount);
        Task<(bool success, string? errorMessage)> UpdateReservation(int id, UpdateReservationDto updateDto);
        Task<bool> DeleteReservation(int id);
        Task<bool> CancelReservation(int reservationId);
    }
}