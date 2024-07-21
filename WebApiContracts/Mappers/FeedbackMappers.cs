using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts.Mappers
{
    public static class FeedbackMappers
    {
        public static FeedbackDO MapToFeedback(this FeedbackContract feedbackContract)
        {
            return new FeedbackDO
            {
                Name = feedbackContract.Name,
                Email = feedbackContract.Email,
                Title = feedbackContract.Title,
                Content = feedbackContract.Content,
                Stars = feedbackContract.Stars,
                IsAnonymus = feedbackContract.IsAnonymus,
                Category = feedbackContract.Category,
                Date = feedbackContract.Date,
            };
        }
    }
}
