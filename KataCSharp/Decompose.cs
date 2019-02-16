namespace KataCSharp
{
    using System.Collections.Generic;
    using System.Linq;

    public class Decompose
    {
        public string decompose(long n)
        {
            var result = decompose(n * n, n);
            return result.Count() > 0?result.Aggregate("", (agg, next) => agg += " " + next).Trim(' '):null;
        }
        private IEnumerable<long> decompose(long sum, long limit)
        {
            if (limit == 1 && sum == 1) return new long[] { 1 };
            for (long i = limit - 1; i > 0; i--)
            {
                if (i * i == sum) return new long[] { i };
                if (i * i < sum)
                {
                    var reduced = decompose(sum - i * i, i)?.ToList();
                    if (reduced != null && reduced.Distinct().Count() == reduced.Count())
                    {
                        reduced.Add(i);
                        return reduced;
                    }
                }
            }
            return null;
        }
    }
}