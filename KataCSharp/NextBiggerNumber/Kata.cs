namespace KataCSharp.NextBiggerNumber
{
    using System.Linq;
    public class Kata
    {
        public static long NextBiggerNumber(long n)
        {
            var strN = n.ToString();
            for (int i = strN.Length - 2; i >= 0; i--)
            {
                string scopeVal = strN.Substring(i),
                primary = scopeVal.OrderBy(x => x).Aggregate("", (agg, next) => agg += next),
                secondary = "";
                while (primary.Length > 0)
                {
                    string reorderedVal = primary.First() + secondary + primary.Substring(1);
                    secondary += primary.First();
                    primary = primary.Substring(1);
                    if (reorderedVal.CompareTo(scopeVal) > 0)
                        return long.Parse(strN.Substring(0, i) + reorderedVal);
                }
            }
            return -1;
        }
    }
}
