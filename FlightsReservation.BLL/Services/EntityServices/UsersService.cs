using FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Interfaces.Dtos;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;
using FlightsReservation.BLL.Interfaces.Services;

namespace FlightsReservation.BLL.Services.EntityServices;

public class UsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<IUserDto> _validator;
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UsersService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<IUserDto> validator, IUsersRepository usersRepository, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<FlightReservationResult<UserReadDto?>> LoginAsync(string login, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(login))
        {
            Console.WriteLine("ERROR: Bad login");
            return FlightReservationResult<UserReadDto?>.Fail("Bad login", ResponseCodes.BadRequest);
        }

        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("ERROR: Bad password");
            return FlightReservationResult<UserReadDto?>.Fail("Bad password", ResponseCodes.BadRequest);
        }
        
        var user = await _usersRepository.GetByUsernameAsync(login, ct);
        if (user is null)
        {
            Console.WriteLine("User not found");
            return FlightReservationResult<UserReadDto?>.Fail("User not found", ResponseCodes.NotFound);
        }

        var verifyResult = _passwordHasher.VerifyPassword(password, user.Password);
        if (!verifyResult)
        {
            Console.WriteLine("Password hashes don`t match");
            return FlightReservationResult<UserReadDto?>.Fail("Password hashes don`t match", ResponseCodes.BadRequest);
        }

        var userReadDto = _mapper.Map<UserReadDto>(user);

        return FlightReservationResult<UserReadDto?>.Success(userReadDto, ResponseCodes.Success);
    }

    //Admin
    public async Task<FlightReservationResult<UserReadDto>> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<UserReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var user = await _usersRepository.GetByIdAsync(id, ct);
        if (user is null)
        {
            Console.WriteLine("User not found");
            return FlightReservationResult<UserReadDto>.Fail("User not found", ResponseCodes.NotFound);
        }

        var userReadDto = _mapper.Map<UserReadDto>(user);

        return FlightReservationResult<UserReadDto>.Success(userReadDto, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<TotalUserReadDto>> GetUserProfileByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<TotalUserReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var user = await _usersRepository.GetByIdAsync(id, ct);
        if (user is null)
        {
            Console.WriteLine("User not found");
            return FlightReservationResult<TotalUserReadDto>.Fail("User not found", ResponseCodes.NotFound);
        }

        var userProfileReadDto = _mapper.Map<TotalUserReadDto>(user);

        return FlightReservationResult<TotalUserReadDto>.Success(userProfileReadDto, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> AddUserAsync(UserCreateDto createDto, CancellationToken ct = default)
    {
        var existingUser = await _usersRepository.GetByUsernameAsync(createDto.Username, ct);
        if (existingUser is not null)
        {
            Console.WriteLine("User already exists");
            return FlightReservationResult<int>.Fail("User already exists", ResponseCodes.BadRequest);
        }

        var validationResult = await _validator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        createDto.Role = "User";
        createDto.Money = 0.0d;

        User user;

        try
        {
            user = _mapper.Map<User>(createDto);
            string hashedPassword = _passwordHasher.HashPassword(createDto.Password);
            user.Password = hashedPassword;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
            return FlightReservationResult<int>.Fail($"Internal server error: {ex.Message}", ResponseCodes.InternalServerError);
        }

        var res = await _usersRepository.AddAsync(user, ct);

        if (!res)
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> UpdateUserAsync(UserUpdateDto updateDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(updateDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        var user = await _usersRepository.GetByIdAsync(updateDto.Id, ct);
        if (user is null)
        {
            return FlightReservationResult<int>.Fail("User not found", ResponseCodes.NotFound);
        }

        try
        {
            _mapper.Map(updateDto, user);
            string hashedPassword = _passwordHasher.HashPassword(updateDto.Password);
            user.Password = hashedPassword;
        }
        catch
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        _usersRepository.Update(user);

        try
        {
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> UpdateUserMoneyAsync(Guid id, int amount, CancellationToken ct = default)
    {
        if(amount < -100000)
        {
            return FlightReservationResult<int>.Fail("Amount is too low", ResponseCodes.BadRequest);
        }


        var res = await _usersRepository.UpdateMoneyAsync(id, amount, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("User not updated", ResponseCodes.InternalServerError);
        }

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<string>> AddUserMoneyAsync(Guid id, double amount, CancellationToken ct = default)
    {
        if (amount < 0)
        {
            return FlightReservationResult<string>.Fail("Amount is below zero", ResponseCodes.BadRequest);
        }

        var res = await _usersRepository.AddMoneyAsync(id, amount/100, ct);
        if (!res)
        {
            return FlightReservationResult<string>.Fail("User not found", ResponseCodes.NotFound);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        var profileUrl = "http://localhost:63342/Front/profile.html";

        return FlightReservationResult<string>.Success(profileUrl, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> DeleteUser(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<int>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var res = await _usersRepository.DeleteAsync(id, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("User not found", ResponseCodes.NotFound);
        }
        await _unitOfWork.SaveChangesAsync(ct);
        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

}