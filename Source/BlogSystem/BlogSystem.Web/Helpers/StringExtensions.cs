namespace BlogSystem.Web.Helpers
{
    using System;
    using System.Text;

    public static class StringExtensions
    {
        public static string PascalCaseToText(this string input)
        {
            if (input == null)
            {
                return null;
            }

            const char WhiteSpace = ' ';

            var result = new StringBuilder();
            var currentWord = new StringBuilder();
            var abbreviation = new StringBuilder();

            char previous = WhiteSpace;
            bool inWord = false;
            bool isAbbreviation = false;

            for (int i = 0; i < input.Length; i++)
            {
                char symbolToAdd = input[i];

                if (char.IsUpper(symbolToAdd) && previous == WhiteSpace && !inWord)
                {
                    inWord = true;
                    isAbbreviation = true;
                    abbreviation.Append(symbolToAdd);
                }
                else if (char.IsUpper(symbolToAdd) && inWord)
                {
                    abbreviation.Append(symbolToAdd);
                    currentWord.Append(WhiteSpace);
                    symbolToAdd = char.ToLower(symbolToAdd);
                }
                else if (char.IsLower(symbolToAdd) && inWord)
                {
                    isAbbreviation = false;
                }
                else if (symbolToAdd == WhiteSpace)
                {
                    result.Append(isAbbreviation && abbreviation.Length > 1 ? abbreviation.ToString() : currentWord.ToString());
                    currentWord.Clear();
                    abbreviation.Clear();

                    if (result.Length > 0)
                    {
                        abbreviation.Append(WhiteSpace);
                    }

                    inWord = false;
                    isAbbreviation = false;
                }

                previous = symbolToAdd;
                currentWord.Append(symbolToAdd);
            }

            if (currentWord.Length > 0)
            {
                result.Append(isAbbreviation && abbreviation.Length > 1 ? abbreviation.ToString() : currentWord.ToString());
            }

            return result.ToString();
        }
    }
}