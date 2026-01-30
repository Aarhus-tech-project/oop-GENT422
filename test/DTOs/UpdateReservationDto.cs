namespace ReservationSystem.DTOs
{
    public class UpdateReservationDto
    {
        public int? PersonCount { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
    }
}