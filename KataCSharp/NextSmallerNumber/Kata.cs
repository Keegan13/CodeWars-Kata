namespace KataCSharp.NextSmallerNumber
{
    using System.Linq;
    using System.Collections.Generic;
    public class Kata
    {
        public static long NextSmaller(long n)
        {
            var strN = n.ToString();
            for (int i = strN.Length - 2; i >= 0; i--)
            {
                var scopeVal = long.Parse(strN.Substring(i));
                List<char> primary = new List<char>(strN.Substring(i).OrderByDescending(x => x));
                List<char> secondary = new List<char>();
                while (primary.Count > 0)
                {
                    string tryVal = primary.First().ToString();
                    tryVal += secondary.Aggregate("", (agg, next) => agg += next);
                    secondary.Add(primary.First());
                    primary.RemoveAt(0);
                    tryVal += primary.Aggregate("", (agg, next) => agg += next);
                    if (long.Parse(tryVal) < scopeVal && (tryVal.First() != '0' || i > 0))
                        return long.Parse(strN.Substring(0, i) + tryVal);
                }
            }
            return -1;
        }
    }
}
