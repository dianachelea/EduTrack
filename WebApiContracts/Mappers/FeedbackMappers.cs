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
                Category = feedbackContract.Category
            };
        }

        public static FeedbackFilters MapToFeedbackFilters(this FeedbackFiltersContract feedbackFiltersContract)
        {
            return new FeedbackFilters
            {
                ByName = feedbackFiltersContract.ByName,
                ByEmail = feedbackFiltersContract.ByEmail,
                ByTitle = feedbackFiltersContract.ByTitle,
                ByCategories = feedbackFiltersContract.ByCategories,
                StartDate = feedbackFiltersContract.StartDate,
                EndDate = feedbackFiltersContract.EndDate,
                Stars = feedbackFiltersContract.Stars,
                IsAnonymus = feedbackFiltersContract.IsAnonymus
            };
        }
    }
}
