using AutoMapper;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Services.UtilityServices.FlightsApi;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using System;

namespace FlightsReservation.BLL.Services.UtilityServices.Simulation
{
    public class RefreshService
    {
        private readonly AviationStackService _aviationStackService;
        
        private readonly IFlightsRepository _flightsRepository;
        private readonly ISeatsRepository _seatsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RefreshService(IFlightsRepository flightsRepository,
            ISeatsRepository seatsRepository,
            IUnitOfWork unitOfWork,
            AviationStackService aviationStackService,
            IMapper mapper)
        {
            _flightsRepository = flightsRepository;
            _seatsRepository = seatsRepository;
            _unitOfWork = unitOfWork;
            _aviationStackService = aviationStackService;
            _mapper = mapper;
        }

        public async Task<FlightReservationResult<int>> RefreshDatabaseAsync(string password, CancellationToken ct)
        {
            if (password != "qwer123")
                return FlightReservationResult<int>.Fail("Bad password", ResponseCodes.BadRequest);

            //await _flightsRepository.DeleteAllAsync(ct);

            var futureDate = DateTime.UtcNow.Date.AddDays(10);

            /*
            Example airport IATA codes:
            var airports = new[]
            {
                "WAW",  // Warsaw
                "BER",  // Berlin
                "CDG",  // Paris
                "LHR",  // London
                "FCO",  // Rome
                "PRG"   // Prague
            };
            */
            int added = 0;

            var res = await _aviationStackService.GetFutureFlightsAsync("WAW", "departure", futureDate);

            if (!res.IsSuccess)
                return FlightReservationResult<int>.Fail(res.ErrorMessage!, ResponseCodes.InternalServerError);

            List<Flight> flightsToAdd;

            flightsToAdd = _mapper.Map<List<Flight>>(res.Value);

            foreach (var f in flightsToAdd)
            {
                await _flightsRepository.AddAsync(f, ct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
                added += flightsToAdd.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return FlightReservationResult<int>.Fail(ex.Message, ResponseCodes.InternalServerError);
            }
            
            var n = await _flightsRepository.GetTotalCountAsync(ct);

            var flightsRead = await _flightsRepository.GetPageAsync(1, n, ct);
            if (!flightsRead.Any())
                return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);

            char[] seatLetters = { 'A', 'B', 'C', 'D', 'E', 'F' };

            foreach (var f in flightsRead)
            {
                for (int j = 1; j <= 6; j++)
                {
                    foreach (char letter in seatLetters)
                    {
                        var seatDto = new Seat()
                        {
                            SeatNumber = $"{letter}{j}",
                            FlightId = f.Id
                        };
                        await _seatsRepository.AddAsync(seatDto, ct);
                    }
                }
            }
            
            await _unitOfWork.SaveChangesAsync(ct);

            Console.WriteLine($"Successfully imported {added} flights from AviationStack API");
            return FlightReservationResult<int>.Success(added, ResponseCodes.Success);
        }
    }
}