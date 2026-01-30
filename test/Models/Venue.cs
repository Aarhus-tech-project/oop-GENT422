namespace ReservationSystem.Models
{
public abstract class Venue
{
    public int VenueId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string VenueType { get; set; } = string.Empty; 
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public abstract List<Seat> GetAvailableSeats();

    public virtual int GetTotalCapacity()
    {
        return 0;
    }

    public override string ToString()
    {
        return $"[{VenueType}] {Name}";
    }
}
}
