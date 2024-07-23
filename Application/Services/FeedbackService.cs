﻿using Application.Interfaces;
using Domain;
using System;
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
            // TODO - data validations
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
            // TODO - data validations
            var fileds = feedbackFilters.GetType().GetFields().All(field => field.GetValue(feedbackFilters) == null);
            var properties = feedbackFilters.GetType().GetProperties().All(property => property.GetValue(feedbackFilters) == null);
            if (fileds && properties)
                return await this._feedbackRepository.GetFeedback();

            var fbacks = await this._feedbackRepository.GetFeedback(feedbackFilters);

            return fbacks;
        }
    }
}