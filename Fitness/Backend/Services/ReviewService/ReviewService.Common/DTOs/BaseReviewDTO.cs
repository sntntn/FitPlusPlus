using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Common.DTOs
{
    public class BaseReviewDTO
    {
        public string ReservationId { get; set; }
        public string TrainerId { get; set; }
        public string? TrainerComment { get; set; }
        public int? TrainerRating { get; set; }
        public string ClientId { get; set; }
        public string? ClientComment { get; set; }
        public int? ClientRating { get; set; }
    }
}
