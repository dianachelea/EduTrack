using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Dapper;

namespace Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public FeedbackRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<bool> AddFeedback(FeedbackDO feedback)
        {
            var query = @"
                    INSERT INTO [SummerPractice].[Feedback] (
                        [Feedback_name],
                        [Feedback_email],
                        [Feedback_description],
                        [Feedback_title],
                        [Feedback_anonymus],
                        [Feedback_stars],
                        [Feedback_category]
                    )
                    VALUES (
                        @FeedbackName,
                        @FeedbackEmail,
                        @FeedbackDescription,
                        @FeedbackTitle,
                        @FeedbackAnonymus,
                        @FeedbackStars,
                        @FeedbackCategory
                    )";
            var parameters = new DynamicParameters();
            parameters.Add("FeedbackName", feedback.Name);
            parameters.Add("FeedbackEmail", feedback.Email);
            parameters.Add("FeedbackDescription", feedback.Content);
            parameters.Add("FeedbackTitle", feedback.Title);
            parameters.Add("FeedbackAnonymus", feedback.IsAnonymus, DbType.Boolean);
            parameters.Add("FeedbackStars", feedback.Stars, DbType.Int32);
            parameters.Add("FeedbackCategory", feedback.Category.GetEnumString());
            
            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public List<FeedbackDO> GetFeedback(List<FeedbackFilters> feedbackFilters)
        {
            return null;
        }
    }
}
