using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Common.DTOs
{
    public class BaseReviewDTO
    {
        // Potentially add training id?
        public string TrainerId { get; set; }
        public string ClientId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
