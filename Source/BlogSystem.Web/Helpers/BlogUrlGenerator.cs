namespace BlogSystem.Web.Helpers
{
    using System;
    using System.Text;

    public class BlogUrlGenerator : IBlogUrlGenerator
    {
        public string GenerateUrl(int id, string title, DateTime createdOn)
        {
            return string.Format("/Blog/{0:0000}/{1:00}/{2}/{3}", createdOn.Year, createdOn.Month, this.ToUrl(title), id);
        }

        private string ToUrl(string uglyString)
        {
            var resultString = new StringBuilder(uglyString.Length);
            bool isLastCharacterDash = false;

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