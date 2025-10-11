using ReservationService.API.Entities;

namespace ReservationService.API.Publishers;

public interface IIndividualReservationPublisher
{
    Task PublishBooked(IndividualReservation individualReservation);

    Task PublishClientCancelled(IndividualReservation individualReservation);
    
    Task PublishTrainerCancelled(IndividualReservation individualReservation);
}