using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace test.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    partial class ReservationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"));

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SeatId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("VenueId")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.HasIndex("SeatId");

                    b.HasIndex("VenueId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Seat", b =>
                {
                    b.Property<int>("SeatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeatId"));

                    b.Property<int?>("AirplaneVenueId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BookedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int?>("CinemaVenueId")
                        .HasColumnType("int");

                    b.Property<bool>("IsBooked")
                        .HasColumnType("bit");

                    b.Property<int?>("RestaurantVenueId")
                        .HasColumnType("int");

                    b.Property<string>("SeatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VenueId")
                        .HasColumnType("int");

                    b.HasKey("SeatId");

                    b.HasIndex("AirplaneVenueId");

                    b.HasIndex("CinemaVenueId");

                    b.HasIndex("RestaurantVenueId");

                    b.HasIndex("VenueId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VenueId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VenueType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("VenueId");

                    b.ToTable("Venues");

                    b.HasDiscriminator<string>("VenueType").HasValue("Venue");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Airplane", b =>
                {
                    b.HasBaseType("Venue");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Airplane");
                });

            modelBuilder.Entity("Cinema", b =>
                {
                    b.HasBaseType("Venue");

                    b.Property<int>("ScreenNumber")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Cinema");
                });

            modelBuilder.Entity("Restaurant", b =>
                {
                    b.HasBaseType("Venue");

                    b.Property<string>("CuisineType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Restaurant");
                });

            modelBuilder.Entity("Reservation", b =>
                {
                    b.HasOne("Seat", "AssignedSeat")
                        .WithMany("Reservations")
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AssignedSeat");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("Seat", b =>
                {
                    b.HasOne("Airplane", null)
                        .WithMany("Seats")
                        .HasForeignKey("AirplaneVenueId");

                    b.HasOne("Cinema", null)
                        .WithMany("Seats")
                        .HasForeignKey("CinemaVenueId");

                    b.HasOne("Restaurant", null)
                        .WithMany("Tables")
                        .HasForeignKey("RestaurantVenueId");

                    b.HasOne("Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("Seat", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Airplane", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Cinema", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Restaurant", b =>
                {
                    b.Navigation("Tables");
                });
#pragma warning restore 612, 618
        }
    }
}
