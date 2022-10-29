using Ganss.XSS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;

namespace NMRAPIs.Core.ResponseMiddleware
{
    public static class StringExtension
    {
        /// <summary>
        /// An extension method to check if the string is a valid json or not.
        /// </summary>
        /// <param name="text">Input string.</param>
        /// <returns>A boolean indicating if the string is a valid json or not.</returns>
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();

            if ((!text.StartsWith("{") || !text.EndsWith("}")) && (!text.StartsWith("[") || !text.EndsWith("]")))
            {
                return false;
            }

            try
            {
                JToken.Parse(text);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// To decode HTML text before saving.
        /// </summary>
        /// <param name="text"> Text value.</param>
        /// <returns> Returns the decoded html string.</returns>
        public static string ToDecodeHTML(this string text)
        {
            string decodedString = string.Empty;

            if (!string.IsNullOrWhiteSpace(text))
            {
                decodedString = HttpUtility.HtmlDecode(text);
            }

            return decodedString;
        }

        /// <summary>
        /// To decode and sanitize HTML text before saving.
        /// </summary>
        /// <param name="text"> Text value.</param>
        /// <returns> Returns the decoded and sanitized html string.</returns>
        public static string ToSanitizeDecodeHTML(this string text)
        {
            string decodedString = string.Empty;

            if (!string.IsNullOrWhiteSpace(text))
            {
                HtmlSanitizer sanitizer = new HtmlSanitizer { AllowedSchemes = { "mailto" }, AllowedAttributes = { "class" }, AllowedTags = { "iframe" } };
                decodedString = HttpUtility.HtmlDecode(sanitizer.Sanitize(text));
            }

            return decodedString;
        }
    }
}
