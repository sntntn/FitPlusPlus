using AutoMapper;
using Grpc.Core;
using TrainerService.Common.Repositories;
using TrainerService.GRPC.Protos;
using static TrainerService.GRPC.Protos.GetTrainersResponse.Types;

namespace TrainerService.GRPC.Services;

public class TrainerServiceGrpc : TrainerProtoService.TrainerProtoServiceBase
{
    private readonly ITrainerRepository _repository;
    private readonly IMapper _mapper;

    public TrainerServiceGrpc(ITrainerRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<GetTrainersResponse> GetTrainers(GetTrainersRequest request, ServerCallContext context)
    {
        var trainers = await _repository.GetTrainersByIds(request.Ids);
        var trainersList = new GetTrainersResponse();
        trainersList.Trainers.AddRange(_mapper.Map<IEnumerable<TrainerReply>>(trainers));
        return trainersList;
    }
}