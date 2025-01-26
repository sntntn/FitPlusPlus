using AnalyticsService.Common.Entities;
using AutoMapper;
using EventBus.Messages.Events;

namespace AnalyticsService.API.Mapper;

public class TrainingHeldProfile : Profile
{
    public TrainingHeldProfile()
    {
        CreateMap<TrainingHeldEvent, Training>().ReverseMap();
    }
}