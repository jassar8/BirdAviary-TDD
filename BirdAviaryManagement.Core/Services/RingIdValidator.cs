namespace BirdAviaryManagement.Core.Services
{
    // Validates that Ring IDs contain digits only (no letters or symbols).
    public static class RingIdValidator
    {
        public static bool IsValid(string ringId)
        {
            if (string.IsNullOrWhiteSpace(ringId))
            {
                return false;
            }

            foreach (char c in ringId)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        // Live UI filter: strips non-digit characters as the user types.
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
                if (c >= '0' && c <= '9')
                {
                    filtered[length++] = c;
                }
            }

            return new string(filtered, 0, length);
        }
    }
}
