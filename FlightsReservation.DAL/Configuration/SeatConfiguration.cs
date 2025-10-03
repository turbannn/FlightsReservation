using FlightsReservation.DAL.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightsReservation.DAL.Configuration
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.Property(s => s.SeatNumber).IsRequired().HasMaxLength(4);
        }
    }
}
