﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    internal interface IFeedbackRepository
    {
        public Task<bool> AddFeedback(FeedbackDO feedback);
        public List<FeedbackDO> GetFeedback(List<FeedbackFilters> feedbackFilters);
    }
}