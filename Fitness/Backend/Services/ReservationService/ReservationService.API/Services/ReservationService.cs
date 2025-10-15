using MassTransit;
using ReservationService.API.Entities;
using ReservationService.API.Publishers;
using ReservationService.API.Repository;

namespace ReservationService.API.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly INotificationPublisher _notificationPublisher;
    private readonly IIndividualReservationPublisher _individualReservationPublisher;
    private readonly IGroupReservationPublisher _groupReservationPublisher;
    public ReservationService(IReservationRepository reservationRepository, INotificationPublisher notificationPublisher,
        IIndividualReservationPublisher individualReservationPublisher, IGroupReservationPublisher groupReservationPublisher)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
        _individualReservationPublisher = individualReservationPublisher ?? throw new ArgumentNullException(nameof(individualReservationPublisher));
        _groupReservationPublisher = groupReservationPublisher ?? throw new ArgumentNullException(nameof(groupReservationPublisher));
    }
    
    public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsAsync()
    {
        var reservations = await _reservationRepository.GetIndividualReservationsAsync();
        return reservations;
    }

    public async Task<IEnumerable<GroupReservation>> GetGroupReservationsAsync()
    {
        var reservations = await _reservationRepository.GetGroupReservationsAsync();
        return reservations;
    }

    public async Task<IndividualReservation> GetIndividualReservationAsync(string id)
    {
        var reservation = await _reservationRepository.GetIndividualReservationByIdAsync(id);
        return reservation;
    }

    public async Task<GroupReservation> GetGroupReservationAsync(string id)
    {
        var reservation = await _reservationRepository.GetGroupReservationByIdAsync(id);
        return reservation;
    }

    public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByClientIdAsync(string clientId)
    {
        var reservations = await _reservationRepository.GetIndividualReservationsByClientIdAsync(clientId);
        return reservations;
    }

    public async Task<IEnumerable<GroupReservation>> GetGroupReservationsByClientIdAsync(string clientId)
    {
        var reservations = await _reservationRepository.GetGroupReservationsByClientIdAsync(clientId);
        return reservations;
    }

    public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByTrainerIdAsync(string trainerId)
    {
        var reservations = await _reservationRepository.GetIndividualReservationsByTrainerIdAsync(trainerId);
        return reservations;
    }

    public async Task<IEnumerable<GroupReservation>> GetGroupReservationsByTrainerIdAsync(string trainerId)
    {
        var reservations = await _reservationRepository.GetGroupReservationsByTrainerIdAsync(trainerId);
        return reservations;
    }

    public async Task<bool> CreateIndividualReservationAsync(IndividualReservation individualReservation)
    {
        if (await IsClientFree(individualReservation.ClientId, individualReservation.Date,
                individualReservation.StartTime, individualReservation.EndTime)
            && await IsTrainerFree(individualReservation.TrainerId, individualReservation.Date,
                individualReservation.StartTime, individualReservation.EndTime))
        {
            await _reservationRepository.CreateIndividualReservationAsync(individualReservation);
            var users = new Dictionary<string, string>
            {
                { individualReservation.ClientId, "Client" },
                { individualReservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation created", individualReservation.ToString(), "Information", true, users);
            await _individualReservationPublisher.PublishBooked(individualReservation);
            
            return true;
        }

        return false;
    }

    public async Task<bool> CreateGroupReservationAsync(GroupReservation groupReservation)
    {
        if (await IsTrainerFree(groupReservation.TrainerId, groupReservation.Date,
                groupReservation.StartTime, groupReservation.EndTime))
        {
            await _reservationRepository.CreateGroupReservationAsync(groupReservation);
            var users = new Dictionary<string, string> { { groupReservation.TrainerId, "Trainer" } };
            await _notificationPublisher.PublishNotification("Training reservation created", groupReservation.ToString(), "Information", true, users);
            await _groupReservationPublisher.PublishAdded(groupReservation);
            
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteGroupReservationAsync(string id)
    {
        var reservation = await _reservationRepository.GetGroupReservationByIdAsync(id);
        var deleted = await _reservationRepository.DeleteGroupReservationAsync(id);

        if (deleted)
        {
            var users = new Dictionary<string, string>();
            foreach (var clientId in reservation.ClientIds)
            {
                users.Add(clientId, "Client");
            }
            users.Add(reservation.TrainerId, "Trainer");
            await _notificationPublisher.PublishNotification("Training reservation cancelled", reservation.ToString(),
                "Information", true, users);
            await _groupReservationPublisher.PublishRemoved(reservation);
        }

        return deleted;
    }

    public async Task<bool> BookGroupReservationAsync(string id, string clientId)
    {
        var groupReservation = await _reservationRepository.GetGroupReservationByIdAsync(id); 
        if (await IsClientFree(clientId, groupReservation.Date, groupReservation.StartTime, groupReservation.EndTime))
        {
            var booked = await _reservationRepository.BookGroupReservationAsync(id, clientId);
            if (booked)
            {
                var users = new Dictionary<string, string>
                {
                    { clientId, "Client" },
                    { groupReservation.TrainerId, "Trainer" }
                };
                await _notificationPublisher.PublishNotification("Training reservation booked", groupReservation.ToString(),
                    "Information", true, users);
                await _groupReservationPublisher.PublishBooked(groupReservation, clientId);
            }

            return booked;
        }

        return false;
    }

    public async Task<bool> CancelGroupReservationAsync(string id, string clientId)
    {
        var groupReservation = await _reservationRepository.GetGroupReservationByIdAsync(id);
        var cancelled = await _reservationRepository.CancelGroupReservationAsync(id, clientId);

        if (cancelled)
        {
            var users = new Dictionary<string, string>
            {
                { clientId, "Client" },
                { groupReservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation cancelled", groupReservation.ToString(),
                "Information", true, users);
            await _groupReservationPublisher.PublishCancelled(groupReservation, clientId);
        }
        
        return cancelled;
    }

    public async Task<bool> ClientCancelIndividualReservationAsync(string id)
    {
        var reservation = await _reservationRepository.GetIndividualReservationByIdAsync(id);
        reservation.Status = IndividualReservationStatus.ClientCancelled;
        var cancelled = await _reservationRepository.UpdateIndividualReservationAsync(reservation);
        
        if (cancelled)
        {
            var users = new Dictionary<string, string>
            {
                { reservation.ClientId, "Client" },
                { reservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation cancelled by client", reservation.ToString(),
                "Information", true, users);
            await _individualReservationPublisher.PublishClientCancelled(reservation);
        }
        
        return cancelled;
    }

    public async Task<bool> TrainerCancelIndividualReservationAsync(string id)
    {
        var reservation = await _reservationRepository.GetIndividualReservationByIdAsync(id);
        reservation.Status = IndividualReservationStatus.TrainerCancelled;
        var cancelled = await _reservationRepository.UpdateIndividualReservationAsync(reservation);
        
        if (cancelled)
        {
            var users = new Dictionary<string, string>
            {
                { reservation.ClientId, "Client" },
                { reservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation cancelled by trainer", reservation.ToString(),
                "Information", true, users);
            await _individualReservationPublisher.PublishTrainerCancelled(reservation);       
        }
        
        return cancelled;
    }

    private async Task<bool> IsClientFree(string clientId, DateOnly date, TimeOnly start, TimeOnly end)
    {
        var individualReservations = await _reservationRepository.GetIndividualReservationsByClientIdAsync(clientId);
        var groupReservations = await _reservationRepository.GetGroupReservationsByClientIdAsync(clientId);

        foreach (var individualReservation in individualReservations)
        {
            if (individualReservation.Status == IndividualReservationStatus.Active && individualReservation.Date == date && IntervalsOverlap(individualReservation.StartTime,
                    individualReservation.EndTime, start, end))
            {
                return false;
            }
        }
        
        foreach (var groupReservation in groupReservations)
        {
            if (groupReservation.Date == date && IntervalsOverlap(groupReservation.StartTime,
                    groupReservation.EndTime, start, end))
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> IsTrainerFree(string trainerId, DateOnly date, TimeOnly start, TimeOnly end)
    {
        var individualReservations = await _reservationRepository.GetIndividualReservationsByTrainerIdAsync(trainerId);
        var groupReservations = await _reservationRepository.GetGroupReservationsByTrainerIdAsync(trainerId);

        foreach (var individualReservation in individualReservations)
        {
            if (individualReservation.Status == IndividualReservationStatus.Active && individualReservation.Date == date && IntervalsOverlap(individualReservation.StartTime,
                    individualReservation.EndTime, start, end))
            {
                return false;
            }
        }
        
        foreach (var groupReservation in groupReservations)
        {
            if (groupReservation.Date == date && IntervalsOverlap(groupReservation.StartTime,
                    groupReservation.EndTime, start, end))
            {
                return false;
            }
        }

        return true;
    }
    
    private bool IntervalsOverlap(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
    {
        var s1 = start1.ToTimeSpan();
        var e1 = end1.ToTimeSpan();
        var s2 = start2.ToTimeSpan();
        var e2 = end2.ToTimeSpan();
        
        if (e1 <= s1) e1 = e1.Add(TimeSpan.FromDays(1));
        if (e2 <= s2) e2 = e2.Add(TimeSpan.FromDays(1));

        return s1 < e2 && s2 < e1;
    }
}