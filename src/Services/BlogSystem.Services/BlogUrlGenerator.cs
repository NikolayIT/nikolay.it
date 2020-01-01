namespace BlogSystem.Services
{
    using System;
    using System.Text;

    public class BlogUrlGenerator : IBlogUrlGenerator
    {
        public string GenerateUrl(int id, string title, DateTime createdOn)
        {
            return $"/Blog/{createdOn.Year:0000}/{createdOn.Month:00}/{this.ToUrl(title)}/{id}";
        }

        private string ToUrl(string uglyString)
        {
            var resultString = new StringBuilder(uglyString.Length);
            var isLastCharacterDash = false;

            uglyString = uglyString.Replace("C#", "CSharp");
            uglyString = uglyString.Replace("C++", "CPlusPlus");

            foreach (var character in uglyString)
            {
                if (char.IsLetterOrDigit(character))
                {
                    resultString.Append(character);
                    isLastCharacterDash = false;
                }
                else if (!isLastCharacterDash)
                {
                    resultString.Append('-');
                    isLastCharacterDash = true;
                }
            }

            return resultString.ToString().Trim('-');
        }
    }
}
