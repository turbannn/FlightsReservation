using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;

namespace FlightsReservation.BLL.Services.UtilityServices.Simulation
{
    public class RefreshService
    {
        private readonly IFlightsRepository _flightsRepository;
        private readonly ISeatsRepository _seatsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshService(IFlightsRepository flightsRepository, ISeatsRepository seatsRepository, IUsersRepository usersRepository, IUnitOfWork unitOfWork)
        {
            _flightsRepository = flightsRepository;
            _seatsRepository = seatsRepository;
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FlightReservationResult<int>> RefreshDatabaseAsync(string password, CancellationToken ct)
        {
            if (password != "qwer123")
                return FlightReservationResult<int>.Fail("Bad password", ResponseCodes.BadRequest);

            await _flightsRepository.DeleteAllAsync(ct);

            var random = new Random();
            var airports = new[] { "Warsaw", "Berlin", "Paris", "London", "Rome", "Prague" };
            var companies = new[] { "LOT Polish Airlines", "Wizz Air", "Air France", "Eurowings", "Ryanair", "British Airways" };

            int n = 70;

            for (int i = 0; i < n / 10; i++)
            {
                for (int m = 0; m < n / 7; m++)
                {
                    var departure = airports[random.Next(airports.Length)];
                    var arrival = airports.Except(new[] { departure }).OrderBy(_ => random.Next()).First();
                    var company = companies[random.Next(companies.Length)];

                    var depDay = DateTime.UtcNow.AddDays(i);

                    var createFlightDto = new Flight()
                    {
                        Departure = departure,
                        Arrival = arrival,
                        DepartureTime = depDay,
                        ArrivalTime = depDay.AddHours(random.Next(1, 4)),
                        AirplaneType = $"Boeing {random.Next(700, 799)}",
                        Price = random.Next(300, 900),
                        Currency = "PLN",
                        Company = company
                    };
                    var flightResult = await _flightsRepository.AddAsync(createFlightDto, ct);
                    if (!flightResult)
                        return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
                }
            }
            var flightsRead = await _flightsRepository.GetPageAsync(1, n, ct);
            if (!flightsRead.Any())
                return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);

            foreach (var f in flightsRead)
            {
                // 3. Seats
                var seatCount = random.Next(5, 10);
                for (int j = 1; j <= seatCount; j++)
                {
                    var seatDto = new Seat()
                    {
                        SeatNumber = $"{(char)('A' + j - 1)}{random.Next(1, 30)}",
                        FlightId = f.Id
                    };
                    await _seatsRepository.AddAsync(seatDto, ct);
                }
            }

            var users = await _usersRepository.GetAllAsync(ct);

            foreach (var u in users)
            {
                u.Money = random.Next(1000, 5000);
                _usersRepository.Update(u);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
        }
    }
}