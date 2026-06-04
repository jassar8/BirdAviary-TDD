namespace BirdAviaryManagement.Core.Services
{
    public static class ColorMutationValidator
    {
        public static bool IsValid(string colorMutation)
        {
            if (string.IsNullOrWhiteSpace(colorMutation))
            {
                return false;
            }

            foreach (char c in colorMutation)
            {
                if (c == ' ')
                {
                    continue;
                }

                if (!IsEnglishOrHebrewLetter(c))
                {
                    return false;
                }
            }

            return true;
        }

        public static string FilterInput(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] filtered = new char[text.Length];
            int length = 0;

            foreach (char c in text)
            {
                if (c == ' ' || IsEnglishOrHebrewLetter(c))
                {
                    filtered[length++] = c;
                }
            }

            return new string(filtered, 0, length);
        }

        private static bool IsEnglishOrHebrewLetter(char c)
        {
            return (c >= 'A' && c <= 'Z')
                || (c >= 'a' && c <= 'z')
                || (c >= '\u05D0' && c <= '\u05EA');
        }
    }
}
