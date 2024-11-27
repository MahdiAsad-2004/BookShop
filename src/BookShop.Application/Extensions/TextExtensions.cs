
namespace BookShop.Application.Extensions
{
    public static class TextExtensions
    {
        public static string RemoveLastOccurrenceOfWord(this string input, string word)
        {
            // Find the last occurrence of the word
            int index = input.LastIndexOf(word);

            // Check if the word exists in the string
            if (index != -1)
            {
                // Calculate the new length after removing the last occurrence of the word
                int newLength = index;

                // Return the substring without the last occurrence of the word
                return input.Substring(0, newLength).TrimEnd() + input.Substring(index + word.Length);
            }

            // Return the original string if the word is not found
            return input;
        }

    }
}
