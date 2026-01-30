namespace ReservationSystem.Models
{
public class Cinema : Venue
{
    public List<Seat> Seats { get; set; } = new List<Seat>();
    public int ScreenNumber { get; set; }

    public Cinema()
    {
        VenueType = "Cinema";
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
        return $"Cinema: {Name} (Screen {ScreenNumber}) - Total seats: {GetTotalCapacity()}";
    }
}
}
