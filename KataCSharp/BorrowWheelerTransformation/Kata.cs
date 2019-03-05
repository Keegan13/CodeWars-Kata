namespace KataCSharp.BorrowWheelerTransformation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Kata
    {
        public static Tuple<string, int> Encode(string s)
        {
            var array = new List<string>();
            for (int i = 0; i < s.Length; i++)
                array.Add(s.Substring(i) + s.Substring(0, i));
            array = array.OrderBy(x => x, StringComparer.Ordinal).ToList();
            var index = array.IndexOf(s);
            return Tuple.Create<string, int>(array.Aggregate("", (agg, next) => agg += next.Last()), index > 0 ? index : 0);
        }

        public static string Decode(string s, int i)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            List<string> restored = new List<string>();
            for (int j = 0; j < s.Length; j++)
                restored.Add("");
            for (int j = 0; j < s.Length; j++)
            {
                for (int k = 0; k < s.Length; k++)
                    restored[k] = s[k] + restored[k];
                restored = restored.OrderBy(x => x, StringComparer.Ordinal).ToList();
            }
            return restored[i];
        }
    }
}
