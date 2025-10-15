using System.Text.Json;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using EventBus.Messages.Events;
using MassTransit;

namespace AnalyticsService.API.EventBusConsumers;

public class ReviewConsumer : IConsumer<ReviewEvent>
{
    private readonly IAnalyticsRepository _repository;

    public ReviewConsumer(IAnalyticsRepository repository)
    {
        _repository = repository ??  throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Method for processing <c>ReviewEvent</c> object based on the review type.
    /// Behavior depends both on the sender of the review (i.e., trainer or client) and
    /// on the type of the training that is reviewed (i.e., individual or group)
    /// </summary>
    /// <param name="context"><c>ConsumeContext</c> object representing the send message through the bus</param>
    public async Task Consume(ConsumeContext<ReviewEvent> context)
    {
        ReviewEvent review = context.Message;
        Review analyticsReview = new Review
        {
            UserId = review.UserId,
            Rating = review.Rating,
            Comment = review.Comment
        };
        var individualTraining = await _repository.GetIndividualTrainingByReservationId(review.ReservationId);
        var groupTraining = await _repository.GetGroupTrainingByReservationId(review.ReservationId);
        switch (review.EventType)
        {
            case ReviewEventType.ClientReview:
            {
                if (individualTraining != null)
                {
                    individualTraining.ClientReview = analyticsReview;
                    await _repository.UpdateIndividualTraining(individualTraining);
                }
                else if (groupTraining != null)
                {
                    groupTraining.ClientReviews ??= [];
                    groupTraining.ClientReviews.Add(analyticsReview);
                    await _repository.UpdateGroupTraining(groupTraining);
                }
                break;
            }
            case ReviewEventType.TrainerReview:
            {
                if (individualTraining != null)
                {
                    individualTraining.TrainerReview = analyticsReview;
                    await _repository.UpdateIndividualTraining(individualTraining);
                }
                else if (groupTraining != null)
                {
                    groupTraining.TrainerReview = analyticsReview;
                    await _repository.UpdateGroupTraining(groupTraining);
                }
                break;
            }
        }
    }
}