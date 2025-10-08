using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Constants
{
    public static class EventBusConstants
    {
        public const string NotificationQueue = "notification-queue";
        public const string IndividualReservationQueue = "individual-reservation-queue";
        public const string GroupReservationQueue = "group-reservation-queue";
        public const string ReviewQueue = "review-queue";
    }
}
