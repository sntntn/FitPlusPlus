using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using TrainerService.Common.Entities;
using TrainerService.Common.Repositories;

namespace TrainerService.API.EventBusConsumers
{
    public class BookTrainingConsumer : IConsumer<BookTrainingEvent>
    {
        private readonly ITrainerRepository _repository;
        private readonly IMapper _mapper;

        public BookTrainingConsumer(ITrainerRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<BookTrainingEvent> context)
        {
            var bti = _mapper.Map<BookTrainingInformation>(context.Message);
            await _repository.BookTraining(bti);
        }
    }
}
