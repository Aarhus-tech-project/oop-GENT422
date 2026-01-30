using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.DTOs
{
    
    public class CreateVenueDto
    {
        [Required(ErrorMessage = "Venue navn er påkrævet")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Navn skal være mellem 2-100 tegn")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue type er påkrævet")]
        [RegularExpression("^(Restaurant|Cinema|Airplane)$",
            ErrorMessage = "Venue type skal være 'Restaurant', 'Cinema' eller 'Airplane'")]
        public string VenueType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kapacitet er påkrævet")]
        [Range(1, 10000, ErrorMessage = "Kapacitet skal være mellem 1-10000")]
        public int Capacity { get; set; }
    }

    public class UpdateVenueDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Navn skal være mellem 2-100 tegn")]
        public string? Name { get; set; }

        [Range(1, 10000, ErrorMessage = "Kapacitet skal være mellem 1-10000")]
        public int? Capacity { get; set; }
    }

    public class VenueResponseDto
    {
        public int VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VenueType { get; set; } = string.Empty;
        public int TotalCapacity { get; set; }
        public int AvailableSeats { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
