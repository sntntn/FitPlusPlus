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

    public async Task<GetClientResponse> GetClient(string id)
    {
        var getClientsRequest = new GetClientRequest
        {
            Id = id
        };
        return await _clientProtoServiceClient.GetClientAsync(getClientsRequest);
    }
}