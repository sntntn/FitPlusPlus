using System.Text.Json;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;

namespace AnalyticsService.API.EventBusConsumers;

public class IndividualReservationConsumer : IConsumer<IndividualReservationEvent>
{
    private readonly IAnalyticsRepository _repository;

    public IndividualReservationConsumer(IAnalyticsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    /// <summary>
    /// Method for processing <c>IndividualReservationEvent</c> object based on the event type.
    /// In case of the booking event, a new individual training is registered, otherwise, the
    /// corresponding training is marked as cancelled.
    /// </summary>
    /// <param name="context"><c>ConsumeContext</c> object representing the send message through the bus</param>
    public async Task Consume(ConsumeContext<IndividualReservationEvent> context)
    {
        IndividualReservationEvent individualReservation = context.Message;
        switch (individualReservation.EventType)
        {
            case IndividualReservationEventType.Booked:
            {
                IndividualTraining individualTraining = new IndividualTraining
                {
                    ReservationId = individualReservation.ReservationId,
                    TrainerId = individualReservation.TrainerId,
                    ClientId = individualReservation.ClientId,
                    TrainingTypeId = individualReservation.TrainingTypeId,
                    StartTime = individualReservation.StartTime,
                    EndTime = individualReservation.EndTime,
                    Date = individualReservation.Date,
                    TrainerReview = null,
                    ClientReview = null,
                    Status = IndividualTrainingStatus.Active
                };
                await _repository.CreateIndividualTraining(individualTraining);
                break;
            }
            case IndividualReservationEventType.CancelledByClient:
            {
                IndividualTraining individualTraining =
                    await _repository.GetIndividualTrainingByReservationId(individualReservation.ReservationId);
                individualTraining.Status = IndividualTrainingStatus.ClientCancelled;
                await _repository.UpdateIndividualTraining(individualTraining);
                break;
            }
            case IndividualReservationEventType.CancelledByTrainer:
            {
                IndividualTraining individualTraining =
                    await _repository.GetIndividualTrainingByReservationId(individualReservation.ReservationId);
                individualTraining.Status = IndividualTrainingStatus.TrainerCancelled;
                await _repository.UpdateIndividualTraining(individualTraining);
                break;
            }
        }
    }
}