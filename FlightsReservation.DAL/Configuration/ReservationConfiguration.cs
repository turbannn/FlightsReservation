using FlightsReservation.DAL.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightsReservation.DAL.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(r => r.ReservationNumber).IsRequired().HasMaxLength(30);

            builder.Property(r => r.ReservationDate).IsRequired();
        }
    }
}
