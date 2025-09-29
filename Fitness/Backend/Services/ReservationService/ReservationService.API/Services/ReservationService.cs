using MassTransit;
using ReservationService.API.Entities;
using ReservationService.API.Publishers;
using ReservationService.API.Repository;

namespace ReservationService.API.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly INotificationPublisher _notificationPublisher;

    public ReservationService(IReservationRepository reservationRepository, INotificationPublisher notificationPublisher)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
    }
    
    public async Task<IEnumerable<IndividualReservation>> GetIndividualReservations()
    {
        var reservations = await _reservationRepository.GetIndividualReservationsAsync();
        return reservations;
    }

    public async Task<IEnumerable<GroupReservation>> GetGroupReservations()
    {
        var reservations = await _reservationRepository.GetGroupReservationsAsync();
        return reservations;
    }

    public async Task<IndividualReservation> GetIndividualReservation(string id)
    {
        var reservation = await _reservationRepository.GetIndividualReservationByIdAsync(id);
        return reservation;
    }

    public async Task<GroupReservation> GetGroupReservation(string id)
    {
        var reservation = await _reservationRepository.GetGroupReservationByIdAsync(id);
        return reservation;
    }

    public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByClientId(string clientId)
    {
        var reservations = await _reservationRepository.GetIndividualReservationsByClientIdAsync(clientId);
        return reservations;
    }

    public async Task<IEnumerable<GroupReservation>> GetGroupReservationsByClientId(string clientId)
    {
        var reservations = await _reservationRepository.GetGroupReservationsByClientIdAsync(clientId);
        return reservations;
    }

    public async Task<IEnumerable<IndividualReservation>> GetIndividualReservationsByTrainerId(string trainerId)
    {
        var reservations = await _reservationRepository.GetIndividualReservationsByTrainerIdAsync(trainerId);
        return reservations;
    }

    public async Task<IEnumerable<GroupReservation>> GetGroupReservationsByTrainerId(string trainerId)
    {
        var reservations = await _reservationRepository.GetGroupReservationsByTrainerIdAsync(trainerId);
        return reservations;
    }

    public async Task<bool> CreateIndividualReservation(IndividualReservation individualReservation)
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

            return true;
        }

        return false;
    }

    public async Task<bool> CreateGroupReservation(GroupReservation groupReservation)
    {
        if (await IsTrainerFree(groupReservation.TrainerId, groupReservation.Date,
                groupReservation.StartTime, groupReservation.EndTime))
        {
            await _reservationRepository.CreateGroupReservationAsync(groupReservation);
            var users = new Dictionary<string, string> { { groupReservation.TrainerId, "Trainer" } };
            await _notificationPublisher.PublishNotification("Training reservation created", groupReservation.ToString(), "Information", true, users);

            return true;
        }

        return false;
    }

    public async Task<bool> UpdateIndividualReservation(IndividualReservation individualReservation)
    {
        var updated = await _reservationRepository.UpdateIndividualReservationAsync(individualReservation);

        if (updated)
        {
            var users = new Dictionary<string, string>
            {
                { individualReservation.ClientId, "Client" },
                { individualReservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation updated", individualReservation.ToString(),
                "Information", true, users);
            
        }
        
        return updated;
    }

    public async Task<bool> UpdateGroupReservation(GroupReservation groupReservation)
    {
        var updated = await _reservationRepository.UpdateGroupReservationAsync(groupReservation);

        if (updated)
        {
            var users = new Dictionary<string, string>();
            foreach (var clientId in groupReservation.ClientIds)
            {
                users.Add(clientId, "Client");
            }
            users.Add(groupReservation.TrainerId, "Trainer");
            await _notificationPublisher.PublishNotification("Training reservation updated", groupReservation.ToString(),
                "Information", true, users);
        }

        return updated;
    }

    public async Task<bool> DeleteIndividualReservation(string id)
    {
        var reservation = await _reservationRepository.GetIndividualReservationByIdAsync(id);
        var deleted = await _reservationRepository.DeleteIndividualReservationAsync(id);

        if (deleted)
        {
            var users = new Dictionary<string, string>
            {
                { reservation.ClientId, "Client" },
                { reservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation cancelled", reservation.ToString(),
                "Information", true, users);
        }

        return deleted;
    }

    public async Task<bool> DeleteGroupReservation(string id)
    {
        var reservation = await _reservationRepository.GetGroupReservationByIdAsync(id);
        var deleted = await _reservationRepository.UpdateGroupReservationAsync(reservation);

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
        }

        return deleted;
    }

    public async Task<bool> BookGroupReservation(string id, string clientId)
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
            }

            return booked;
        }

        return false;
    }

    public async Task<bool> CancelGroupReservation(string id, string clientId)
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
        }
        
        return cancelled;
    }

    private async Task<bool> IsClientFree(string clientId, DateOnly date, TimeOnly start, TimeOnly end)
    {
        var individualReservations = await _reservationRepository.GetIndividualReservationsByClientIdAsync(clientId);
        var groupReservations = await _reservationRepository.GetGroupReservationsByClientIdAsync(clientId);

        foreach (var individualReservation in individualReservations)
        {
            if (individualReservation.Date == date && IntervalsOverlap(individualReservation.StartTime,
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
            if (individualReservation.Date == date && IntervalsOverlap(individualReservation.StartTime,
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