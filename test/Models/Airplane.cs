namespace ReservationSystem.Models
{
public class Airplane : Venue
{
    public string FlightNumber { get; set; } = string.Empty;
    public List<Seat> Seats { get; set; } = new List<Seat>();

    public Airplane()
    {
        VenueType = "Airplane";
    }

    public override List<Seat> GetAvailableSeats()
    {
        return Seats.Where(s => !s.IsBooked).ToList();
    }

    public override int GetTotalCapacity()
    {
        return Seats.Count;
    }

    public string GetInfo()
    {
        return $"Flight: {FlightNumber} - Total seats: {GetTotalCapacity()}";
    }
}
}
