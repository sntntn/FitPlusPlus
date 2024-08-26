using AutoMapper;
using ClientService.API.Entities;
using ClientService.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;

namespace ClientService.API.EventBusConsumers
{
    public class CancelTrainingConsumer : IConsumer<TrainerCancellingTrainingEvent>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CancelTrainingConsumer(IRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<TrainerCancellingTrainingEvent> context)
        {
            var cti = _mapper.Map<CancelTrainingInformation>(context.Message);
            await _repository.CancelledTrainingByTrainer(cti);
        }
    }
}
