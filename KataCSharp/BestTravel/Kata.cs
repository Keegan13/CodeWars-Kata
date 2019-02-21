namespace KataCSharp.BestTravel
{
    using System.Linq;
    using System.Collections.Generic;
    public static class SumOfK
    {
        public static int? chooseBestSum(int t, int k, List<int> ls)
        {
            int max = 0;
            if (k == 1)
            {
                for (int i = 0; i < ls.Count; i++)
                    if (ls[i] > max && ls[i] <= t) max = ls[i];
            }
            else
                for (int i = 0; i < ls.Count; i++)
                {
                    var val = chooseBestSum(t - ls[i], k - 1, ls.Skip(i + 1).ToList());
                    if (val.HasValue && val.Value + ls[i] > max && val.Value + ls[i] <= t)
                        max = val.Value + ls[i];
                }
            return max > 0 ? max : new int?();
        }
    }
}

