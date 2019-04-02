namespace KataCSharp._6by6Skyscrapers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public static class Skysrapers2
    {

        public static int CountSteps(int[] array)
        {
            int stepHeight = 0, count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > stepHeight) { stepHeight = array[i]; count++; }
            }
            return count;
        }
        public static int CountStepsReverse(int[] array)
        {
            int stepHeight = 0, count = 0;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] > stepHeight) { stepHeight = array[i]; count++; }
            }
            return count;
        }
        public class Seed
        {
            public int[][] Combos;
            public IEnumerable<IGrouping<int, int[]>> RightSteps;
            public IEnumerable<IGrouping<int, int[]>> LeftSteps;
            public IEnumerable<IGrouping<Wtf, int[]>> Steps;
            public Seed(int size)
            {
                var combos = Enumerable.Range(1, size).Permute(size).Select(x => x.ToArray());
                Steps = combos.GroupBy(x => new Wtf(CountSteps(x), CountStepsReverse(x)));
                LeftSteps = combos.GroupBy(x => CountSteps(x));
                RightSteps = combos.GroupBy(x => CountStepsReverse(x));
            }
            public IEnumerable<int[]> GetLeft(int count)
            {
                return LeftSteps.Where(x => x.Key == count).SelectMany(x => x.Select(g => g));
            }
            public IEnumerable<int[]> GetRight(int count)
            {
                return RightSteps.Where(x => x.Key == count).SelectMany(x => x.Select(g => g));
            }
            public IEnumerable<int[]> Get(int lCount, int rCount)
            {
                return Steps.Where(x => x.Key == new Wtf(lCount, rCount)).SelectMany(x => x);
            }
        }
        public struct Wtf
        {
            public int lCount;
            public int rCount;
            public Wtf(int l, int r)
            {
                this.lCount = l;
                this.rCount = r;
            }
            public static bool operator ==(Wtf x, Wtf y) => x.lCount == y.lCount && x.rCount == y.rCount;
            public static bool operator !=(Wtf x, Wtf y) => x.lCount != y.lCount || x.rCount != y.rCount;
        }

        public class Node
        {
            private int _size;
            private Seed seed;
            private int[] Strategy;
            private int[] Clues;
            public Node(int[] clues)
            {
                this._size = clues.Length / 4;
                this.seed = new Seed(_size);
                this.Clues = clues;
                this.Strategy = GetStrategy(clues).ToArray();

                var result = Solve(Enumerable.Empty<Line>(), 0);
            }
            private int GetDimension(int index)
            {
                if (index < _size) return 1;
                return 0;
            }
            private int GetLexicalIndex(int index)
            {
                return GetLexicalIndex(index, _size);
            }
            private static int GetLexicalIndex(int clueIndex, int size)
            {
                if (clueIndex < size) return clueIndex;

                if (clueIndex >= 3 * size) return 4 * size - clueIndex - 1;

                if (clueIndex >= 2 * size) return 3 * size - clueIndex - 1;

                return clueIndex - size;
            }

            private IEnumerable<int[]> GetLines(int index)
            {
                int coIndex = GetCoIndex(index, _size);

                if (Clues[index] != 0 && Clues[coIndex] != 0)
                {
                    return index < _size ? seed.Get(Clues[index], Clues[coIndex]) : seed.Get(Clues[coIndex], Clues[index]);
                }
                else
                    if (Clues[index] != 0)
                    return index < _size ? seed.GetLeft(Clues[index]) : seed.GetRight(Clues[index]);
                else
                    return index < _size ? seed.GetRight(Clues[coIndex]) : seed.GetLeft(Clues[coIndex]);
            }

            public IEnumerable<Line> Solve(IEnumerable<Line> solutions, int sIndex)
            {
                if (sIndex < Strategy.Length)
                {
                    var index = Strategy[sIndex];
                    IEnumerable<int[]> possible = GetLines(index);
                    int k = GetLexicalIndex(index);
                    int dim = GetDimension(index);

                    //foreach (var l in solutions)
                    //    Console.WriteLine(l.ToString());

                    var filtered = FilterLines(
                        applied: solutions,
                        dimension: dim,
                        index: k,
                        lines: possible);
                    //_4by4Skyscapers.Skyscrapers.Print(filtered);
                    foreach (var p in filtered)
                    {
                        var current = new Line(p, dim, k);
                        var result = Solve(solutions.Concat(new[] { current }), sIndex + 1);
                        if (result.Count() != 0)
                        {
                            return result;
                        }
                    }
                    return Enumerable.Empty<Line>();
                }
                else
                {
                    return solutions;
                }
            }



            private static int GetCoIndex(int index, int size)
            {
                if (index < size) return 3 * size - index - 1;

                if (index >= 3 * size) return 4 * size - index + size - 1;

                if (index >= 2 * size) return 3 * size - index - 1;

                return 5 * size - index - 1;
            }


            public static int[] GetStrategy(int[] clues)
            {
                Dictionary<int, int> st = new Dictionary<int, int>();
                int size = clues.Length / 4;
                for (int i = 0; i < clues.Length; i++)
                {
                    if (clues[i] != 0)
                    {
                        var index = GetStrategyIndex(i, size);
                        if (st.ContainsKey(index))
                        {
                            st[index] *= clues[i];
                        }
                        else
                            st.Add(index, clues[i]);
                    }
                }
                return st.Where(x => x.Value != 0).OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();
            }
            public static int GetStrategyIndex(int index, int size)
            {
                if (index < size) return index;

                if (index >= 3 * size) return 5 * size - index - 1;

                if (index >= 2 * size) return 3 * size - index - 1;

                return index;
            }

            private static IEnumerable<int[]> FilterLines(IEnumerable<int[]> lines, IEnumerable<Line> applied, int index, int dimension)
            {
                int count = applied.Count();
                foreach (var line in lines)
                {
                    if (count == 0 || Test(applied, line, index, dimension))
                        yield return line;
                }
                yield break;
            }
            private static bool Test(IEnumerable<Line> mask, int[] array, int index, int dimension)
            {
                foreach (var row in mask.Where(x => x.Dimension != dimension))
                {
                    if (row.Members[index] != array[row.Index])
                        return false;
                }
                return true;
            }

        }

        public class Line
        {
            public Line(int[] members, int dimension, int index)
            {
                this.Dimension = dimension;
                this.Members = members;
                this.Index = index;
            }
            public readonly int[] Members;
            public readonly int Dimension;
            public readonly int Index;
            public override string ToString()
            {
                return String.Format("{1} {2}: {0} ", Members.Aggregate("", (a, n) => a += " " + n), Index, Dimension == 0 ? "row" : "col");
            }
        }
        public static IEnumerable<IEnumerable<int>> Permute(this IEnumerable<int> values, int count)
        {
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
            if (values == null) throw new ArgumentNullException(nameof(values));
            return count == 1
                ? values.Select(x => new[] { x }).ToArray()
                : values.SelectMany(value => values.Except(new[] { value }).Permute(count - 1).Select(x => new[] { value }.Concat(x)));
        }



        public static int[][] SolvePuzzle(int[] clues)
        {
            // Start your coding here...
            return null;
        }

        public class MatrixSeed
        {
            public CellSeed[,] Seed;
            private readonly int _size;
            public MatrixSeed(int size)
            {
                this._size = size;
                this.Seed = new CellSeed[size, size];
                var seed = Enumerable.Range(1, size);
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        Seed[i, j] = new CellSeed(seed, new Skyscrapers5.Index(i, j));
            }
            public CellSeed[] GetRowSeed(int index)
            {
                CellSeed[] row = new CellSeed[_size];
                for (int i = 0; i < _size; i++)
                    row[i] = Seed[index, i];
                return row;
            }
            public CellSeed[] GetColumnSeed(int index)
            {
                CellSeed[] col = new CellSeed[_size];
                for (int i = 0; i < _size; i++)
                    col[i] = Seed[i, index];
                return col;
            }
            public void RemoveFromRow(int index, int val)
            {
                for (int i = 0; i < _size; i++)
                    Seed[index, i].RemoveValue(val);
            }
            public void RemoveFromColumn(int index, int val)
            {
                for (int i = 0; i < _size; i++)
                    Seed[i, index].RemoveValue(val);
            }
            public void RemoveValue(int i, int j, int val)
            {
                RemoveFromRow(i, val);
                RemoveFromColumn(j, val);
            }

        }

        public struct CellSeed
        {
            public readonly Skyscrapers5.Index Index;
            public CellSeed(IEnumerable<int> values, Skyscrapers5.Index index)
            {
                this.Index = index;
                this.Values = new HashSet<int>();
                foreach (var val in values)
                    Values.Add(val);
            }
            public HashSet<int> Values;
            public void RemoveValue(int value)
            {
                Values.Remove(value);
            }
            public bool TryRemoveValue(int value)
            {
                if (Values.Contains(value))
                {
                    Values.Remove(value);
                    return true;
                }
                return false;
            }
            public bool Contains(int value)
            {
                return Values.Contains(value);
            }
        }
    }
}
