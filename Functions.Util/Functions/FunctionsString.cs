namespace Functions.Util.Functions
{
    public static class FunctionsString
    {
        public static bool EqualsString(string value, string valueTwo)
        {
            value = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
            valueTwo = string.IsNullOrWhiteSpace(valueTwo) ? string.Empty : valueTwo;
            if (value.Trim().Equals(valueTwo.Trim()))
                return true;

            return false;

        }
    }
}
