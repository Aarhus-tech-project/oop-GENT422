namespace ReservationSystem.Models
{
public class Reservation
{
    public int ReservationId { get; set; }
    public int VenueId { get; set; }
    public int PersonCount { get; set; }
    public DateTime ReservationDate { get; set; } = DateTime.Now;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public Venue? Venue { get; set; }
 

    public override string ToString()
    {
        return $"Reservation: {CustomerName} ({PersonCount} persons) - {Status}";
    }
}
public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}
}
