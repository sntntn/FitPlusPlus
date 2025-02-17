using ReservationService.API.Entities;

namespace ReservationService.API.Repository;

public interface IReservationRepository
{
    Task<IEnumerable<IndividualReservation>> GetIndividualReservationsAsync(string? clientId, string? trainerId);
    Task<IEnumerable<GroupReservation>> GetGroupReservationsAsync(string? clientId, string? trainerId);
    Task<IndividualReservation> GetIndividualReservationByIdAsync(string id);
    Task<GroupReservation> GetGroupReservationByIdAsync(string id);
    Task CreateIndividualReservationAsync(IndividualReservation reservation);
    Task CreateGroupReservationAsync(GroupReservation reservation);
    Task UpdateIndividualReservationAsync(string id, IndividualReservation reservation);
    Task UpdateGroupReservationAsync(string id, GroupReservation reservation);
    Task DeleteIndividualReservationAsync(string id);
    Task DeleteGroupReservationAsync(string id);
    Task BookGroupReservationAsync(string reservationId, string clientId);
    Task CancelGroupReservationAsync(string reservationId, string clientId);
}