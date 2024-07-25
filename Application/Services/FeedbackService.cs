using Application.Interfaces;
using Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FeedbackService
    {
        private IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<bool> AddFeedback(FeedbackDO feedback)
        {
            if (feedback == null) throw new ArgumentNullException(nameof(feedback));

            if (!feedback.IsAnonymus)
            {
                if (feedback.Name == "-" && feedback.Email == "-")
                    feedback.IsAnonymus = true;
            }
            else
            {
                if (feedback.Name != "-" && feedback.Email != "-")
                {
                    feedback.Name = "-";
                    feedback.Email = "-";
                }
            }

            // Data validations
            if (!feedback.Email.Contains('@') && !feedback.IsAnonymus)
            {
                throw new Exception("Email invalid!");
            }
            if (feedback.Stars < 1 || feedback.Stars > 5)
            {
                throw new Exception("Number of starts is not valid!");
            }

            var result = await this._feedbackRepository.AddFeedback(new FeedbackDO
            {
                Name = feedback.Name,
                Email = feedback.Email,
                Title = feedback.Title,
                Content = feedback.Content,
                Stars = feedback.Stars,
                IsAnonymus = feedback.IsAnonymus,
                Category = feedback.Category,
            });

            return result;
        }

        public async Task<IEnumerable<FeedbackDO>> GetFeedback(FeedbackFilters feedbackFilters)
        {
            var fileds = feedbackFilters.GetType().GetFields().All(field => field.GetValue(feedbackFilters) == null);
            var properties = feedbackFilters.GetType().GetProperties().All(property => property.GetValue(feedbackFilters) == null);
            if (fileds && properties)
                return await this._feedbackRepository.GetFeedback();

            // TODO - data validations
            if (feedbackFilters.ByEmail != null)
            {
                foreach (var email in feedbackFilters.ByEmail)
                {
                    if (!email.Contains('@'))
                    {
                        throw new Exception("Email invalid!");
                    }
                }
            }
            
            if (feedbackFilters.Stars != null)
            {
                foreach (var stars in feedbackFilters.Stars)
                {   
                    if (stars < 1 || stars > 5)
                    {
                        throw new Exception("Number of starts is not valid!");
                    }
                }
            }
            

            var fbacks = await this._feedbackRepository.GetFeedback(feedbackFilters);

            return fbacks;
        }
    }
}
