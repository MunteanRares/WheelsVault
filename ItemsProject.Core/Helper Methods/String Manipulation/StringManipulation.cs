


namespace ItemsProject.Core.Helper_Methods.String_Manipulation
{
    public static class StringManipulation
    {
        public static string Capitalize(this string input)
        {
            string[] splitInputs = input.Split(" ");
            List<string> outputArray = new List<string>();
            foreach(string item in splitInputs)
            {
                if (item == "")
                {
                    splitInputs = splitInputs.Where(i => i != item).ToArray();
                }
                else if (item == null)
                {
                    throw new ArgumentException($"{nameof(item)} cannot be empty", nameof(item));
                }
                else
                {
                    var itemChars = FirstCharToUpper(item);
                    outputArray.Add(new string(itemChars));
                }
            }

            string outputString = string.Join(" ", outputArray);
            return outputString;
        }

        public static char[] FirstCharToUpper(string input)
        {
            var output = input.ToArray();
            output[0] = Char.ToUpperInvariant(output[0]);
            return output;
        }
    }
}
