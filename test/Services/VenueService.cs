using Microsoft.EntityFrameworkCore;
using ReservationSystem.DTOs;
using ReservationSystem.Models;

using Venue = ReservationSystem.Models.Venue;
using Seat = ReservationSystem.Models.Seat;
using Restaurant = ReservationSystem.Models.Restaurant;
using Cinema = ReservationSystem.Models.Cinema;
using Airplane = ReservationSystem.Models.Airplane;

public interface IVenueService
{
    
    Task<Venue?> CreateVenue(string name, string venueType, int capacity);
    Task<List<Venue>> GetAllVenues();
    Task<Venue?> GetVenueById(int id);
    Task<List<Seat>> GetVenueSeats(int venueId);
    Task<bool> UpdateVenue(int id, UpdateVenueDto updateDto);
    Task<bool> DeleteVenue(int id);
}
public class VenueService : IVenueService
{
    private readonly ReservationDbContext _context;

    public VenueService(ReservationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Venue?> CreateVenue(string name, string venueType, int capacity)
    {
        try
        {
            Venue venue;

           
            venue = venueType.ToLower() switch
            {
                "restaurant" => new Restaurant { Name = name, CuisineType = "Danish" },
                "cinema" => new Cinema { Name = name, ScreenNumber = 1 },
                "airplane" => new Airplane { Name = name, FlightNumber = GenerateFlightNumber() },
                _ => throw new Exception("Unknown venue type")
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            await CreateRandomSeats(venue.VenueId, capacity, venueType);

            return venue;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating venue: {ex.Message}");
            return null;
        }
    }

    
    private async Task CreateRandomSeats(int venueId, int totalCapacity, string venueType)
    {
        var random = new Random();
        var seats = new List<Seat>();

        if (venueType.ToLower() == "restaurant")
        {
           
            for (int i = 0; i < totalCapacity; i++)
            {
                int tableCapacity = random.Next(1, 7); 
                seats.Add(new Seat
                {
                    VenueId = venueId,
                    SeatNumber = $"Table {i + 1}",
                    Capacity = tableCapacity,
                    IsBooked = false
                });
            }
        }
        else if (venueType.ToLower() == "cinema")
        {
            for (int i = 1; i <= totalCapacity; i++)
            {
                seats.Add(new Seat
                {
                    VenueId = venueId,
                    SeatNumber = $"Seat {i}",
                    Capacity = 1,
                    IsBooked = false
                });
            }
        }
        else if (venueType.ToLower() == "airplane")
        {
            int row = 1;
            for (int i = 0; i < totalCapacity; i++)
            {
                char col = (char)('A' + (i % 6));
                if (i > 0 && i % 6 == 0) row++;
                
                seats.Add(new Seat
                {
                    VenueId = venueId,
                    SeatNumber = $"{row}{col}",
                    Capacity = 1,
                    IsBooked = false
                });
            }
        }

        if (seats.Any())
        {
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
            Console.WriteLine($" Created {seats.Count} seats for venue {venueId}");
        }
    }

    private string GenerateFlightNumber()
    {
        var random = new Random();
        return $"SK{random.Next(1000, 9999)}";
    }

    public async Task<List<Venue>> GetAllVenues()
    {
        return await _context.Venues.ToListAsync();
    }

    public async Task<Venue?> GetVenueById(int id)
    {
        return await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == id);
    }

    public async Task<bool> UpdateVenue(int id, UpdateVenueDto updateDto)
    {
        try
        {
            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == id);
            if (venue == null) 
                return false;

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                venue.Name = updateDto.Name;

            if (updateDto.Capacity.HasValue && updateDto.Capacity > 0)
            {
                venue.Name = updateDto.Name ?? venue.Name;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteVenue(int id)
    {
        try
        {
            var venue = await GetVenueById(id);
            if (venue == null) return false;

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Seat>> GetVenueSeats(int venueId)
    {
        return await _context.Seats
            .Where(s => s.VenueId == venueId)
            .ToListAsync();
    }
}
