namespace ReservationSystem.DTOs
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int VenueId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public int SeatCount { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public List<SeatDto> Seats { get; set; } = new();
    }

    public class CreateReservationDto
    {
        public int VenueId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int SeatCount { get; set; }
    }

    public class SeatDto
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public bool IsReserved { get; set; }
    }
}
