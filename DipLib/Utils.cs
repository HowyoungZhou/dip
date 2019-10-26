namespace DipLib
{
    public static class Utils
    {
        public static long GetMin(this long[] array)
        {
            long min = array[0];
            foreach (var element in array)
            {
                if (element < min) min = element;
            }
            return min;
        }
    }
}