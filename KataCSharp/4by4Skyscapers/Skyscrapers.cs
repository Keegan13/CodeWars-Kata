namespace KataCSharp._4by4Skyscapers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public static class Skyscrapers
    {
        public static int[][] SolvePuzzle(int[] clues)
        {
            int size = clues.Length / 4;
            var incSeq = GetIncSeq(size);
            var oldPerm = incSeq.GetAllPermutations(size);
            var areas = oldPerm.GetAllAreaPermutations(size).ToArray();
            foreach (var area in areas)
            {
                Print(area);
                if (IsComfortRules(area, clues))
                    return area.Select(x => x.ToArray()).ToArray();
            }
            return null;
        }
        public static int[] GetIncSeq(int size)
        {
            var sequance = new int[size];
            for (int i = 0; i < sequance.Length; i++)
                sequance[i] = i + 1;
            return sequance;

        }
        public static int CountSteps(IEnumerable<int> array)
        {
            int stepHeight = 0, count = 0;
            foreach (var val in array)
                if (val > stepHeight) { stepHeight = val; count++; }
            return count;
        }
        public static bool IsComfortRules(IEnumerable<IEnumerable<int>> area, int[] rules)
        {
            int size = area.Count();
            for (int i = 0; i < size; i++)
            {
                if (rules[size + i] != 0 && rules[size + i] != CountSteps(area.ElementAt(i).Reverse())) return false;
                if (rules[4 * size - i - 1] != 0 && rules[4 * size - i - 1] != CountSteps(area.ElementAt(i))) return false;
            }
            for (int j = 0; j < size; j++)
            {
                var column = new List<int>();
                for (int i = 0; i < size; i++)
                {
                    var current = area.ElementAt(i).ElementAt(j);
                    if (column.Contains(current)) return false;
                    else column.Add(current);
                }
                if (rules[j] != 0 && rules[j] != CountSteps(column)) return false;
                if (rules[3 * size - j - 1] != 0 && rules[3 * size - j - 1] != CountSteps(Enumerable.Reverse(column))) return false;
            }
            return true;
        }

        public static void Print(IEnumerable<IEnumerable<int>> area)
        {
            string message = "";
            foreach (var row in area) message += row.Aggregate("", (agg, next) => agg += ", " + next.ToString()).Trim(',') + Environment.NewLine;
            Console.WriteLine(message);
        }
        public class ArrayComparer : IEqualityComparer<IEnumerable<int>>
        {
            public bool Equals(IEnumerable<int> left, IEnumerable<int> right)
            {
                //Console.Write(left.Aggregate("", (a, n) => a += " " + n) + " and " + right.Aggregate("", (a, n) => a += " " + n));
                bool result = left.Zip(right, (r, l) => r == l).All(x => x == true);
                //Console.WriteLine("is {0} equal", result ? "" : "not");
                return result;
            }
            public int GetHashCode(IEnumerable<int> obj)
            {
                int z = 976410549;
                bool swap = false;
                foreach (var item in obj)
                {
                    if (swap)
                        z |= item.GetHashCode();
                    else
                        z *= item.GetHashCode();
                    swap = !swap;
                }
                return z;
            }
        }
        public static IEnumerable<IEnumerable<int>> GetAllPermutations(this IEnumerable<int> values, int count)
        {
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
            if (values == null) throw new ArgumentNullException(nameof(values));
            return count == 1
                ? values.Select(x => new[] { x }).ToArray()
                : values.SelectMany(value => values.Except(new[] { value }).GetAllPermutations(count - 1).Select(x => new[] { value }.Concat(x)));
        }
        public static IEnumerable<IEnumerable<IEnumerable<int>>> GetAllAreaPermutations(this IEnumerable<IEnumerable<int>> values, int count)
        {
            if (count == 1)
                return values.Select(x => new[] { x });
            else
                return values.SelectMany(value => GetAllAreaPermutations(values.Where(x => value.Zip(x, (v, X) => v != X).All(f => f == true)), count - 1).Select(x => Enumerable.Repeat(value, 1).Concat(x)));
        }
    }

}
