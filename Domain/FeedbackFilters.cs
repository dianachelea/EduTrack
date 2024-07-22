using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FeedbackFilters
    {
        public string? ByName { get; set; }
        public string? ByEmail { get; set; }
        public string? ByTitle { get; set; }
        public List<FeedbackCategory>? ByCategories { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MinStars { get; set; }
        public int? MaxStars { get; set; }
        public bool? IsAnonymus { get; set; }
    }
}
