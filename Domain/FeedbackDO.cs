using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum FeedbackCategory
    {
        ContentQuality,
        UserExperience,
        TechnicalPerformance,
        EducationalTools,
        AssessmentAndFeedback
    }
    public class FeedbackDO
    {
        public string Name {  get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } //in db is Description
        public int Stars { get; set; }
        public bool IsAnonymus { get; set; }
        public FeedbackCategory Category { get; set; }
        public DateTime Date {  get; set; }
    }
}
