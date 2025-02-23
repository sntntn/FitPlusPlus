using AutoMapper;
using ClientService.Common.Repositories;
using Grpc.Core;
using ClientService.GRPC.Protos;

namespace ClientService.GRPC.Services;

public class ClientServiceGrpc : ClientProtoService.ClientProtoServiceBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    
    public ClientServiceGrpc(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<GetClientResponse> GetClient(GetClientRequest request, ServerCallContext context)
    {
        var client = await _repository.GetClientById(request.Id);
        var clientInfo = _mapper.Map<GetClientResponse>(client);
        return clientInfo;
    }
}