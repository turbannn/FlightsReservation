using AutoMapper;
using FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserReadDto>();

        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        CreateMap<User, TotalUserReadDto>();
    }
}