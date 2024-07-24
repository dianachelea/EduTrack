using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<bool> AddFeedback(FeedbackDO feedback);
        public Task<IEnumerable<FeedbackDO>> GetFeedback(FeedbackFilters? feedbackFilters = null);
    }
}
