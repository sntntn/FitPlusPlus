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