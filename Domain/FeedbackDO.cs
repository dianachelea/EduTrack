using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class CategoryAttribute : Attribute
    {
        public string Value { get; }
        public CategoryAttribute(string value)
        {
            Value = value;
        }
    }

    public enum FeedbackCategory
    {
        [Category("Content Quality")]
        ContentQuality,
        [Category("User Experience")]
        UserExperience,
        [Category("Technical Performance")]
        TechnicalPerformance,
        [Category("Educational Tools")]
        EducationalTools,
        [Category("Assessment and Feedback")]
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
        public string stringCategory { get; set; }
        public DateTime? Date {  get; set; }
    }

    public static class EnumExtensions
    {
        public static string GetEnumString(this Enum enumValue) =>
                            enumValue.GetType()
                                     .GetMember(enumValue.ToString())
                                     .FirstOrDefault()?
                                     .GetCustomAttributes(typeof(CategoryAttribute), false)
                                     .Cast<CategoryAttribute>()
                                     .FirstOrDefault()?.Value ?? enumValue.ToString();
    
        public static TEnum GetEnumFromString<TEnum>(string value) where TEnum : Enum
        {
            foreach (var field in typeof(TEnum).GetFields())
            {
                var attribute = field.GetCustomAttributes(typeof(CategoryAttribute), false)
                    .Cast<CategoryAttribute>()
                    .FirstOrDefault();
                if (attribute != null && attribute.Value == value)
                {
                    return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException($"No matching enum value found for string '{value}'", nameof(value));
        }
    }
}
