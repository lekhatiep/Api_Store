using System.Globalization;
using System.Text;

namespace DoAn3API.Extensions
{
    public static class StringExtension
    {
        //Ex: Nũm => Num
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string withoutDiacritics = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Đ và đ
            withoutDiacritics = withoutDiacritics.Replace("Đ", "D");
            withoutDiacritics = withoutDiacritics.Replace("đ", "d");

            return withoutDiacritics;
        }
    }
}
