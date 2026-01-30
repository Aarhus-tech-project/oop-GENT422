using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test.Migrations
{
    
    public partial class InitialCreate : Migration
    {
       
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VenueType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenNumber = table.Column<int>(type: "int", nullable: true),
                    CuisineType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AirplaneVenueId = table.Column<int>(type: "int", nullable: true),
                    CinemaVenueId = table.Column<int>(type: "int", nullable: true),
                    RestaurantVenueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_Venues_AirplaneVenueId",
                        column: x => x.AirplaneVenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                    table.ForeignKey(
                        name: "FK_Seats_Venues_CinemaVenueId",
                        column: x => x.CinemaVenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                    table.ForeignKey(
                        name: "FK_Seats_Venues_RestaurantVenueId",
                        column: x => x.RestaurantVenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                    table.ForeignKey(
                        name: "FK_Seats_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: true),
                    PersonCount = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "SeatId");
                    table.ForeignKey(
                        name: "FK_Reservations_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VenueId",
                table: "Reservations",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_AirplaneVenueId",
                table: "Seats",
                column: "AirplaneVenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CinemaVenueId",
                table: "Seats",
                column: "CinemaVenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_RestaurantVenueId",
                table: "Seats",
                column: "RestaurantVenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_VenueId",
                table: "Seats",
                column: "VenueId");
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
