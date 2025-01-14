using TrainerService.GRPC.Protos;

namespace NotificationService.API.GrpcServices;

public class TrainerGrpcService
{
    private readonly TrainerProtoService.TrainerProtoServiceClient _trainerProtoServiceClient;

    public TrainerGrpcService(TrainerProtoService.TrainerProtoServiceClient trainerProtoServiceClient)
    {
        _trainerProtoServiceClient = trainerProtoServiceClient ??
                                     throw new ArgumentNullException(nameof(trainerProtoServiceClient));
    }

    public async Task<GetTrainersResponse> GetTrainers(IEnumerable<string> ids)
    {
        var getTrainersRequest = new GetTrainersRequest();
        getTrainersRequest.Ids.AddRange(ids);
        return await _trainerProtoServiceClient.GetTrainersAsync(getTrainersRequest);
    }
}