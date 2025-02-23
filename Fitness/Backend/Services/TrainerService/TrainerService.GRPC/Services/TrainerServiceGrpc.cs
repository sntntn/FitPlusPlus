using AutoMapper;
using Grpc.Core;
using TrainerService.Common.Repositories;
using TrainerService.GRPC.Protos;

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

    public override async Task<GetTrainerResponse> GetTrainer(GetTrainerRequest request, ServerCallContext context)
    {
        var trainer = await _repository.GetTrainer(request.Id);
        var trainerInfo = _mapper.Map<GetTrainerResponse>(trainer);
        return trainerInfo;
    }
}