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

    public async Task<GetTrainerResponse> GetTrainers(string id)
    {
        var getTrainerRequest = new GetTrainerRequest
        {
            Id = id
        };
        return await _trainerProtoServiceClient.GetTrainerAsync(getTrainerRequest);
    }
}