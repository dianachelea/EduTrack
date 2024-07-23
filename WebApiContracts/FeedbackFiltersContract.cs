using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class FeedbackFiltersContract
    {
        public List<string>? ByName { get; set; }
        public List<string>? ByEmail { get; set; }
        public List<string>? ByTitle { get; set; }
        public List<FeedbackCategory>? ByCategories { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int>? Stars { get; set; }
        public bool? IsAnonymus { get; set; }
    }
}
