using ReservationService.API.Entities;

namespace ReservationService.API.Services;

public interface IReservationService
{
    Task<IEnumerable<IndividualReservation>> GetIndividualReservationsAsync();

    Task<IEnumerable<GroupReservation>> GetGroupReservationsAsync();

    Task<IndividualReservation> GetIndividualReservationAsync(string id);

    Task<GroupReservation> GetGroupReservationAsync(string id);

    Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByClientIdAsync(string clientId);

    Task<IEnumerable<GroupReservation>> GetGroupReservationsByClientIdAsync(string clientId);

    Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByTrainerIdAsync(string trainerId);

    Task<IEnumerable<GroupReservation>> GetGroupReservationsByTrainerIdAsync(string trainerId);

    Task<bool> CreateIndividualReservationAsync(IndividualReservation individualReservation);

    Task<bool> CreateGroupReservationAsync(GroupReservation groupReservation);

    Task<bool> DeleteGroupReservationAsync(string id);

    Task<bool> BookGroupReservationAsync(string id, string clientId);

    Task<bool> CancelGroupReservationAsync(string id, string clientId);
    
    Task<bool> ClientCancelIndividualReservationAsync(string id);
    
    Task<bool> TrainerCancelIndividualReservationAsync(string id);
}