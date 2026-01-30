using System;
using System.Collections.Generic;
namespace ReservationSystem.Models
{
public class Seat
{
    public int SeatId { get; set; }
    public int VenueId { get; set; }
    public int Capacity { get; set; } 
    public bool IsBooked { get; set; } = false;
    public string SeatNumber { get; set; } = string.Empty; 
    public DateTime? BookedDate { get; set; }


    public Venue? Venue { get; set; }
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();

    public override string ToString()
    {
        return $"Seat {SeatNumber} (Capacity: {Capacity}, Booked: {IsBooked})";
    }
}
}