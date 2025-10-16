using FlightsReservation.DAL.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightsReservation.DAL.Configuration
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(f => f.Lock)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("TIMESTAMP '1970-01-01 00:00:00'");

            builder.Property(s => s.SeatNumber).IsRequired().HasMaxLength(4);
        }
    }
}
