using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Villafane.WebSite.Helpers
{
    public static class Extensions
    {
        public static string GetUpn(this IIdentity i)
        {
            if (i == null)
            {
                return null;
            }
            if (i is not ClaimsIdentity c_i)
            {
                return null;
            }
            var claim_upn = c_i.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Upn);
            if (claim_upn == null)
            {
                return null;
            }
            return claim_upn.Value;
        }

        public static SelectListItem[] ForSelect<T>(this IEnumerable<T> s, Func<T, string> textSelector, Func<T, string> valueSelector,
            Func<T, bool>? selectedSelector = null)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            if (textSelector == null)
            {
                throw new ArgumentNullException(nameof(textSelector));
            }
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }
            var result = s
                .Select(i =>
                {
                    var result = new SelectListItem { Text = textSelector(i), Value = valueSelector(i) };
                    if (selectedSelector != null)
                    {
                        result.Selected = selectedSelector(i);
                    }
                    return result;
                })
                .ToArray();
            return result;
        }

        public static async Task<SelectListItem[]> ForSelectAsync<T>(this Task<List<T>> s, Func<T, string> textSelector, Func<T, string> valueSelector,
            Func<T, bool>? selectedSelector = null)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            if (textSelector == null)
            {
                throw new ArgumentNullException(nameof(textSelector));
            }
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }
            var data = await s;
            var result = data
                .Select(i =>
                {
                    var result = new SelectListItem
                    {
                        Text = textSelector(i),
                        Value = valueSelector(i),
                        Selected = selectedSelector != null && selectedSelector(i)
                    };
                    return result;
                })
                .ToArray();
            return result;
        }

        public static List<SelectListItem> ForOptionalSelect<T>(this IEnumerable<T> s, Func<T, string> textSelector, Func<T, string> valueSelector,
            string nullItemText, Func<T, bool>? selectedSelector = null)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            if (textSelector == null)
            {
                throw new ArgumentNullException(nameof(textSelector));
            }
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }
            if (string.IsNullOrEmpty(nullItemText))
            {
                throw new ArgumentNullException(nameof(nullItemText));
            }
            var result = s
                .Select(i =>
                {
                    var result = new SelectListItem { Text = textSelector(i), Value = valueSelector(i) };
                    if (selectedSelector != null)
                    {
                        result.Selected = selectedSelector(i);
                    }
                    return result;
                })
                .ToList();
            result.Insert(0, new SelectListItem { Text = nullItemText, Value = "", Selected = selectedSelector == null });
            return result;
        }

        public static async Task<List<SelectListItem>> ForOptionalSelectAsync<T>(this Task<List<T>> s, Func<T, string> textSelector, 
            Func<T, string> valueSelector, string nullItemText, Func<T, bool>? selectedSelector = null)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            if (textSelector == null)
            {
                throw new ArgumentNullException(nameof(textSelector));
            }
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }
            if (string.IsNullOrEmpty(nullItemText))
            {
                throw new ArgumentNullException(nameof(nullItemText));
            }
            var data = await s;
            var result = data
                .Select(i =>
                {
                    var result = new SelectListItem 
                    { 
                        Text = textSelector(i), 
                        Value = valueSelector(i),
                        Selected = selectedSelector != null && selectedSelector(i)
                    };
                    return result;
                })
                .ToList();
            result.Insert(0, new SelectListItem { Text = nullItemText, Value = "", Selected = selectedSelector == null });
            return result;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static IHtmlContent BoolToIcon(this IHtmlHelper html, bool value)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var data = value ? "checkmark" : "cross";
            var result = string.Format("<i class=\"dripicons-{0}\"></i>", data);
            return new HtmlString(result);
        }

        public static string TruncateAt(this string s, int length, bool appendEllipsis = true)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            if (s.Length < length)
            {
                return s;
            }
            var result = s[..length];
            if (appendEllipsis)
            {
                return result + "...";
            }
            else
            {
                return result;
            }
        }

        public static string NormalizeForComparison(this string text)
        {
            var normalized_string = text.Trim().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();
            for (int i = 0; i < normalized_string.Length; i++)
            {
                char c = normalized_string[i];
                var unicode_category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicode_category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(char.ToUpper(c));
                }
            }
            var result =  builder.ToString().Normalize(NormalizationForm.FormC);
            return result;
        }

        public static async Task<byte[]> GetFormFileContentAsync(this IFormFile f)
        {
            if (f == null)
            {
                return null;
            }
            byte[] result;
            using (var m = new MemoryStream())
            using (var s = f.OpenReadStream())
            {
                await s.CopyToAsync(m);
                result = m.ToArray();
            }
            return result;
        }

        public static HtmlString FormatNullOrEmpty<TModel>(this IHtmlHelper<TModel> helper, string? valor)
        {
            if (helper == null)
            {
                throw new NullReferenceException();
            }
            if (string.IsNullOrEmpty(valor))
            {
                return new HtmlString("&nbsp;");
            }
            return new HtmlString(valor);
        }

        public static HtmlString FormatNullOrEmpty<TModel>(this IHtmlHelper<TModel> helper, int? valor, string formatString = "D0")
        {
            if (helper == null)
            {
                throw new NullReferenceException();
            }
            if (!valor.HasValue)
            {
                return new HtmlString("&nbsp;");
            }
            return new HtmlString(valor.Value.ToString(formatString));
        }

       
    }
}
