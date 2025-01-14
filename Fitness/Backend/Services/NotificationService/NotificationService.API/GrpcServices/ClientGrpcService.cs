using ClientService.GRPC.Protos;
using Google.Protobuf.Collections;

namespace NotificationService.API.GrpcServices;

public class ClientGrpcService
{
    private readonly ClientProtoService.ClientProtoServiceClient _clientProtoServiceClient;

    public ClientGrpcService(ClientProtoService.ClientProtoServiceClient clientProtoServiceClient)
    {
        _clientProtoServiceClient = clientProtoServiceClient ??
                                    throw new ArgumentNullException(nameof(clientProtoServiceClient));
    }

    public async Task<GetClientsResponse> GetClients(IEnumerable<string> ids)
    {
        var getClientsRequest = new GetClientsRequest();
        getClientsRequest.Ids.AddRange(ids);
        return await _clientProtoServiceClient.GetClientsAsync(getClientsRequest);
    }
}