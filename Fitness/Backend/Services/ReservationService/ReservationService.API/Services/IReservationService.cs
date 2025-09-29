using ReservationService.API.Entities;

namespace ReservationService.API.Services;

public interface IReservationService
{
    Task<IEnumerable<IndividualReservation>> GetIndividualReservations();

    Task<IEnumerable<GroupReservation>> GetGroupReservations();

    Task<IndividualReservation> GetIndividualReservation(string id);

    Task<GroupReservation> GetGroupReservation(string id);

    Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByClientId(string clientId);

    Task<IEnumerable<GroupReservation>> GetGroupReservationsByClientId(string clientId);

    Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByTrainerId(string trainerId);

    Task<IEnumerable<GroupReservation>> GetGroupReservationsByTrainerId(string trainerId);

    Task<bool> CreateIndividualReservation(IndividualReservation individualReservation);

    Task<bool> CreateGroupReservation(GroupReservation groupReservation);

    Task<bool> UpdateIndividualReservation(IndividualReservation individualReservation);

    Task<bool> UpdateGroupReservation(GroupReservation groupReservation);

    Task<bool> DeleteIndividualReservation(string id);

    Task<bool> DeleteGroupReservation(string id);

    Task<bool> BookGroupReservation(string id, string clientId);

    Task<bool> CancelGroupReservation(string id, string clientId);
}