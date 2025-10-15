using ReservationService.API.Entities;

namespace ReservationService.API.Publishers;

public interface IGroupReservationPublisher
{
    Task PublishAdded(GroupReservation groupReservation);
    
    Task PublishRemoved(GroupReservation groupReservation);

    Task PublishBooked(GroupReservation groupReservation, string clientId);
    
    Task PublishCancelled(GroupReservation groupReservation, string clientId);  
}