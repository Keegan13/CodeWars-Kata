
namespace KataCSharp.TestAlignJustify
{
    using System.Linq;
    using System.Collections.Generic;
    using System;
    public class Kata
    {
        public static IEnumerable<int> Distribute(int sum, int count)
        {
            List<int> result = new List<int>(0);
            while (count > 0)
            {
                var cur = (int)Math.Round((double)sum / count);
                result.Add(cur);
                sum -= cur;
                count--;
            }
            return result.OrderByDescending(x => x);
        }
        public static string Justify(string str, int len)
        {
            var paragraphs = str.Split('\n');
            if (paragraphs.Count() > 1)
            {
                return paragraphs.Select(x=>Justify(x,len)).Aggregate("",(agg,next)=>agg+='\n'+next).Trim('\n');
            }

            if (str.Length == len) return str;
            string result = "";
            if (str.Length < len)
            {
                var words = str.Split(' ');
                if (words.Count() == 1) return str;
                var blanks = Distribute(len - words.Aggregate(0, (agg, next) => agg += next.Length), words.Count() - 1);
                var i = -1;
                foreach (var word in words)
                {
                    if (i >= 0 && i < blanks.Count()) result += new String(Enumerable.Repeat(' ', blanks.ElementAt(i)).ToArray());
                    result += word;
                    i++;
                }
                return result;
            }

            var lines = GetLines(str, len).ToArray();
            for(int i=0;i<lines.Length-1;i++)
            {
                result += Justify(lines[i], len) + "\n";
            }
            result += lines.Last();
            return result;
        }
        private static IEnumerable<string> GetLines(string str, int len)
        {
            var Words = str.Split(' ');
            var Lines = new List<string>();
            string Line = "";
            foreach (var word in Words)
            {
                if (Line.Length + word.Length > len)
                {
                    Lines.Add(Line.TrimEnd(' '));
                    Line = "";
                }
                Line += word + " ";
            }
            if (!String.IsNullOrEmpty(Line.TrimEnd(' ')))
                Lines.Add(Line.TrimEnd(' '));
            return Lines;
        }
    }
}
