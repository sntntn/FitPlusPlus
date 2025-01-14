using AutoMapper;
using ClientService.Common.Repositories;
using Grpc.Core;
using ClientService.GRPC.Protos;
using static ClientService.GRPC.Protos.GetClientsResponse.Types;

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

    public override async Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
    {
        var clients = await _repository.GetClientsByIds(request.Ids);
        var clientsList = new GetClientsResponse();
        clientsList.Clients.AddRange(_mapper.Map<IEnumerable<ClientReply>>(clients));
        return clientsList;
    }
}