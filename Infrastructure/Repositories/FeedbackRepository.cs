using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Dapper;
using System.Xml.Linq;

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

        public async Task<IEnumerable<FeedbackDO>> GetFeedback(FeedbackFilters? feedbackFilters = null)
        {
            var queryStart = @"
                    SELECT 
                        [Feedback_name] AS Name,
                        [Feedback_email] AS Email,
                        [Feedback_description] AS Content,
                        [Feedback_title] AS Title,
                        [Feedback_category] AS stringCategory,
                        [Feedback_anonymus] AS IsAnonymus,
                        [Feedback_stars] AS Stars,
                        [Feedback_date] AS Date
                    FROM [SummerPractice].[Feedback]";

            var queryBuilder = new StringBuilder(queryStart);
            var parameters = new DynamicParameters();

            if (feedbackFilters != null)
            {
                queryBuilder.Append(" WHERE 1=1 ");

                if (feedbackFilters.ByName != null && feedbackFilters.ByName.Any())
                {
                    queryBuilder.Append("AND [Feedback_name] IN (");
                    queryBuilder.Append(string.Join(", ", feedbackFilters
                                            .ByName
                                            .Select((_, index) => $"@ByName{index}")));
                    queryBuilder.Append(") ");
                    for (int i = 0; i < feedbackFilters.ByName.Count(); i++)
                    {
                        parameters.Add($"ByName{i}", feedbackFilters.ByName[i], DbType.String);
                    }
                }

                if (feedbackFilters.ByEmail != null && feedbackFilters.ByEmail.Any())
                {
                    queryBuilder.Append("AND [Feedback_email] IN (");
                    queryBuilder.Append(string.Join(", ", feedbackFilters
                                            .ByEmail
                                            .Select((_, index) => $"@ByEmail{index}")));
                    queryBuilder.Append(") ");
                    for (int i = 0; i < feedbackFilters.ByEmail.Count(); i++)
                    {
                        parameters.Add($"ByEmail{i}", feedbackFilters.ByEmail[i], DbType.String);
                    }
                }

                if (feedbackFilters.ByTitle != null && feedbackFilters.ByTitle.Any())
                {
                    queryBuilder.Append("AND [Feedback_title] IN (");
                    queryBuilder.Append(string.Join(", ", feedbackFilters
                                            .ByTitle
                                            .Select((_, index) => $"@ByTitle{index}")));
                    queryBuilder.Append(") ");
                    for (int i = 0; i < feedbackFilters.ByTitle.Count(); i++)
                    {
                        parameters.Add($"ByTitle{i}", feedbackFilters.ByTitle[i], DbType.String);
                    }
                }

                if (feedbackFilters.ByCategories != null && feedbackFilters.ByCategories.Any())
                {
                    // TODO
                    queryBuilder.Append("AND [Feedback_category] IN (");
                    queryBuilder.Append(string.Join(", ", feedbackFilters
                                            .ByCategories
                                            .Select((_, index) => $"@ByCategories{index}")));
                    queryBuilder.Append(") ");
                    for (int i = 0; i < feedbackFilters.ByCategories.Count(); i++)
                    {
                        parameters.Add($"ByCategories{i}", feedbackFilters.ByCategories[i].GetEnumString(), DbType.String);
                    }
                }

                if (feedbackFilters.StartDate.HasValue)
                {
                    queryBuilder.Append("AND [Feedback_date] >= @StartDate ");
                    parameters.Add("StartDate", feedbackFilters.StartDate);
                }

                if (feedbackFilters.EndDate.HasValue)
                {
                    queryBuilder.Append("AND [Feedback_date] <= @EndDate ");
                    parameters.Add("EndDate", feedbackFilters.EndDate);
                }

                if (feedbackFilters.Stars != null && feedbackFilters.Stars.Any())
                {
                    queryBuilder.Append("AND [Feedback_stars] IN (");
                    queryBuilder.Append(string.Join(", ", feedbackFilters
                                            .Stars
                                            .Select((_, index) => $"@Stars{index}")));
                    queryBuilder.Append(") ");
                    for (int i = 0; i < feedbackFilters.Stars.Count(); i++)
                    {
                        parameters.Add($"Stars{i}", feedbackFilters.Stars[i], DbType.Int32);
                    }
                }

                if (feedbackFilters.IsAnonymus.HasValue)
                {
                    queryBuilder.Append("AND [Feedback_anonymus] = @IsAnonymus ");
                    parameters.Add("IsAnonymus", feedbackFilters.IsAnonymus, DbType.Boolean);
                }
            }
            
            var query = queryBuilder.ToString();
            var connection = _databaseContext.GetDbConnection();
            var fbacks = await connection.QueryAsync<FeedbackDO>(query, parameters, _databaseContext.GetDbTransaction());

            var feedbackDOs = fbacks.Select(feedback => new FeedbackDO
            {
                Name = feedback.Name,
                Email = feedback.Email,
                Content = feedback.Content,
                Title = feedback.Title,
                Stars = feedback.Stars,
                IsAnonymus = feedback.IsAnonymus,
                Category = EnumExtensions.GetEnumFromString<FeedbackCategory>(feedback.stringCategory),
                Date = feedback.Date
            }).ToList();

            return feedbackDOs.ToList();
        }
    }
}
