using AutoMapper;
using ClientService.Common.Entities;
using EventBus.Messages.Events;

namespace ClientService.API.Mapper
{
    public class BookTrainingProfile : Profile
    {
        public BookTrainingProfile()
        {
            CreateMap<BookTrainingInformation, BookTrainingEvent>().ReverseMap();
        }
    }
}
