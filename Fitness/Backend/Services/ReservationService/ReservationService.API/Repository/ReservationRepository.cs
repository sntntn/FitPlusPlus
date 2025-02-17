using MongoDB.Driver;
using ReservationService.API.Data;
using ReservationService.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationService.API.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IContext _context;

        public ReservationRepository(IContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsAsync()
        {
            return await _context.IndividualReservations.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<GroupReservation>> GetGroupReservationsAsync()
        {
            return await _context.GroupReservations.Find(_ => true).ToListAsync();
        }

        public async Task<IndividualReservation> GetIndividualReservationByIdAsync(string id)
        {
            return await _context.IndividualReservations.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<GroupReservation> GetGroupReservationByIdAsync(string id)
        {
            return await _context.GroupReservations.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByClientIdAsync(string clientId)
        {
            return await _context.IndividualReservations.Find(r => r.ClientId == clientId).ToListAsync();
        }

        public async Task<IEnumerable<GroupReservation>> GetGroupReservationsByClientIdAsync(string clientId)
        {
            return await _context.GroupReservations.Find(r => r.Clients.Contains(clientId)).ToListAsync();
        }

        public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByTrainerIdAsync(string trainerId)
        {
            return await _context.IndividualReservations.Find(r => r.TrainerId == trainerId).ToListAsync();
        }

        public async Task<IEnumerable<GroupReservation>> GetGroupReservationsByTrainerIdAsync(string trainerId)
        {
            return await _context.GroupReservations.Find(r => r.TrainerId == trainerId).ToListAsync();
        }

        public async Task CreateIndividualReservationAsync(IndividualReservation reservation)
        {
            await _context.IndividualReservations.InsertOneAsync(reservation);
        }

        public async Task CreateGroupReservationAsync(GroupReservation reservation)
        {
            await _context.GroupReservations.InsertOneAsync(reservation);
        }

        public async Task<bool> UpdateIndividualReservationAsync(IndividualReservation reservation)
        {
            var result = await _context.IndividualReservations.ReplaceOneAsync(r => r.Id == reservation.Id, reservation);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateGroupReservationAsync(GroupReservation reservation)
        {
            var result = await _context.GroupReservations.ReplaceOneAsync(r => r.Id == reservation.Id, reservation);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteIndividualReservationAsync(string id)
        {
            var result = await _context.IndividualReservations.DeleteOneAsync(r => r.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteGroupReservationAsync(string id)
        {
            var result = await _context.GroupReservations.DeleteOneAsync(r => r.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> BookGroupReservationAsync(string id, string clientId)
        {
            var reservation = await GetGroupReservationByIdAsync(id);
            if (reservation.Capacity >= reservation.Clients.Count)
                return false;
            
            reservation.Clients.Add(clientId);
            var result = await _context.GroupReservations.ReplaceOneAsync(r => r.Id == id, reservation);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> CancelGroupReservationAsync(string id, string clientId)
        {
            var reservation = await GetGroupReservationByIdAsync(id);
            reservation.Clients.Remove(clientId);
            var result = await _context.GroupReservations.ReplaceOneAsync(r => r.Id == id, reservation);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}