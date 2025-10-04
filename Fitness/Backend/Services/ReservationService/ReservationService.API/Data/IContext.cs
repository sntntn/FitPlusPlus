using MongoDB.Driver;
using ReservationService.API.Entities;

namespace ReservationService.API.Data;

public interface IContext
{
    IMongoCollection<IndividualReservation> IndividualReservations { get; }
    IMongoCollection<GroupReservation> GroupReservations { get; }
}