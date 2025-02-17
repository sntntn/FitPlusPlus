using MongoDB.Driver;
using ReservationService.API.Entities;

namespace ReservationService.API.Data;

public class Context : IContext
{
    public IMongoCollection<IndividualReservation> IndividualReservations { get; }
    public IMongoCollection<GroupReservation> GroupReservations { get; }

    public Context(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase("Reservations");
        
        IndividualReservations = database.GetCollection<IndividualReservation>("IndividualReservations");
        GroupReservations = database.GetCollection<GroupReservation>("GroupReservations");
    }
    
}