using AnalyticsService.Common.DTOs;
using AnalyticsService.Common.Entities;
using AutoMapper;

namespace AnalyticsService.API.Mapper;

public class ClientTrainingProfile : Profile
{
    public ClientTrainingProfile()
    {
        CreateMap<ClientTrainingDTO, Training>().ReverseMap();
    }
}