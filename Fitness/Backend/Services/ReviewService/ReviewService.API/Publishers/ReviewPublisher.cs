using AutoMapper;
using MassTransit;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;

namespace ReviewService.API.Publishers;

public class ReviewPublisher : IReviewPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public ReviewPublisher(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task PublishReview(SubmitReviewDTO submittedReview, bool client)
    {
        var review = new ReviewEvent
        {
            ReservationId = submittedReview.ReservationId,
            UserId = client ? submittedReview.ClientId : submittedReview.TrainerId,
            Comment = client ? submittedReview.ClientComment : submittedReview.TrainerComment,
            Rating = client ? submittedReview.ClientRating : submittedReview.TrainerRating,
            EventType = client ? ReviewEventType.ClientReview : ReviewEventType.TrainerReview
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.ReviewEvent>(review);
        return _publishEndpoint.Publish(eventMessage);
    }
}