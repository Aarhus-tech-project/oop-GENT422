namespace ReservationSystem.Models
{
public class Restaurant : Venue
{
    public string CuisineType { get; set; } = string.Empty;
    public List<Seat> Tables { get; set; } = new List<Seat>();

    public Restaurant()
    {
        VenueType = "Restaurant";
    }

    public override List<Seat> GetAvailableSeats()
    {
        return Tables.Where(t => !t.IsBooked).ToList();
    }

    public override int GetTotalCapacity()
    {
        return Tables.Sum(t => t.Capacity);
    }

    public string GetInfo()
    {
        return $"Restaurant: {Name} ({CuisineType}) - Total capacity: {GetTotalCapacity()}";
    }
}
}
