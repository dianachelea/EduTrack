using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStatistics
    {
        public Task<StatisticsDO> GetStudentStats(string email);
        public Task<StatisticsDO> GetTeacherStats(string email);
    }
}
