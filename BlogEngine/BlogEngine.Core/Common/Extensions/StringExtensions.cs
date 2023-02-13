using BlogEngine.Shared.Helpers;
using System.Text.RegularExpressions;

namespace BlogEngine.Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static string StripHtmlTagsWithRegex(this string rawHtmlContent)
        {
            Preconditions.NotNullOrWhiteSpace(rawHtmlContent, nameof(rawHtmlContent));

            Regex regex = new Regex("\\<[^\\>]*\\>");
            return regex.Replace(rawHtmlContent, string.Empty);
        }
    }
}