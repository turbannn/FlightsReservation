using FlightsReservation.DAL.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightsReservation.DAL.Configuration
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(40);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(40);
            builder.Property(p => p.PassportNumber).IsRequired().HasMaxLength(15);
            builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(25);


            builder.HasOne(p => p.Seat)
                .WithOne(s => s.Passenger)
                .HasForeignKey<Passenger>(p => p.SeatId)
                .IsRequired();

            builder.HasIndex(p => p.SeatId).IsUnique();
        }
    }
}
