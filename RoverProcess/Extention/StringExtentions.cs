namespace RoverProcess.Extention
{
    public static class StringExtentions
    {
        public static bool IsNUllOrWhiteSpace(this string text) 
        {
            return string.IsNullOrWhiteSpace(text);
        }
    }
}
