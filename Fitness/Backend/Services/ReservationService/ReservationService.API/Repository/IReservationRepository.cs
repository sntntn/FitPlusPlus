using ReservationService.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationService.API.Repository
{
    public interface IReservationRepository
    {
        Task<IEnumerable<IndividualReservation>> GetIndividualReservationsAsync();
        Task<IEnumerable<GroupReservation>> GetGroupReservationsAsync();
        Task<IndividualReservation> GetIndividualReservationByIdAsync(string id);
        Task<GroupReservation> GetGroupReservationByIdAsync(string id);
        Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByClientIdAsync(string clientId);
        Task<IEnumerable<GroupReservation>> GetGroupReservationsByClientIdAsync(string clientId);
        Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByTrainerIdAsync(string trainerId);
        Task<IEnumerable<GroupReservation>> GetGroupReservationsByTrainerIdAsync(string trainerId);
        Task CreateIndividualReservationAsync(IndividualReservation reservation);
        Task CreateGroupReservationAsync(GroupReservation reservation);
        Task<bool> UpdateIndividualReservationAsync(IndividualReservation reservation);
        Task<bool> UpdateGroupReservationAsync(GroupReservation reservation);
        Task<bool> DeleteIndividualReservationAsync(string id);
        Task<bool> DeleteGroupReservationAsync(string id);
        Task<bool> BookGroupReservationAsync(string id, string clientId);
        Task<bool> CancelGroupReservationAsync(string id, string clientId);
    }
}