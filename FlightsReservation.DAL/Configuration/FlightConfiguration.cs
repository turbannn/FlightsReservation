using FlightsReservation.DAL.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightsReservation.DAL.Configuration;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.Property(f => f.FlightNumber).IsRequired().HasMaxLength(20);
        builder.Property(f => f.Departure).IsRequired().HasMaxLength(30);
        builder.Property(f => f.Arrival).IsRequired().HasMaxLength(30);
        builder.Property(f => f.AirplaneType).IsRequired().HasMaxLength(20);

        builder.Property(f => f.DepartureTime).IsRequired();
        builder.Property(f => f.ArrivalTime).IsRequired();
    }
}