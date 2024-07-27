using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StatisticsService
    {
        private IStatistics _statistics;

        public StatisticsService(IStatistics statistics)
        {
            _statistics = statistics; 
        }

        public async Task<StatisticsDO> GetStudentStats(string email)
        {
            if (email == null) { throw new ArgumentNullException("email"); }
            if (!email.Contains('@')) { throw new ArgumentException("Email is invalid"); }

            var result = await _statistics.GetStudentStats(email);
            return result;
        }
        public async Task<StatisticsDO> GetTeacherStats(string email)
        {
            if (email == null) { throw new ArgumentNullException("email"); }
            if (!email.Contains('@')) { throw new ArgumentException("Email is invalid"); }

            var result = await _statistics.GetTeacherStats(email);
            return result;
        }
    }
}
