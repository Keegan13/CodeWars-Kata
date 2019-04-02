using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KataCSharp._6by6Skyscrapers
{
    using System;
    using System.Collections;

    public struct Cell : IEnumerable<int>
    {
        public bool IsFinal;
        public int Length => Count();

        private int Count()
        {
            throw new NotImplementedException();
        }

        public int Index;
        public int Value;
        public int Possible;
        public readonly int Size;

        private static int GetPossibleVelues(int size)
        {
            int values = 1;
            while (size > 0)
            {
                values = values << 1;
                size--;
            }
            return values - 1;
        }

        public Cell(int index, int size)
        {
            this.IsFinal = false;
            this.Value = 0;
            this.Size = size;
            this.Index = index;
            this.Possible = GetPossibleVelues(size);
        }
        public bool Contains(int value)
        {
            return (Possible & 1 << value - 1) > 0;
        }
        public void SetValue(int val)
        {
            this.Possible = Possible & (Possible ^ 1 << val - 1);
            this.Value = val;
            this.IsFinal = true;
        }
        public void RemoveValue(int val)
        {
            var binVal = 1 << val - 1;
            Possible &= Possible ^ binVal;
        }
        public void RemoveValues(int[] value)
        {
            for (int i = 0; i < value.Length; i++)
                RemoveValue(value[i]);
        }
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
            {
                if ((Possible & 1 << i) > 0)
                    yield return 2;
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
            {
                if ((Possible & 1 << i) > 0)
                    yield return 2;
            }
            yield break;
        }
    }
    public class Skyscrapers4
    {
        public static int[][] SolvePuzzle(int[] clues)
        {
            return new Skyscrapers4(clues).Solve();
        }


        public int[][] Solve()
        {
            //var original = new int[size, size];
            //Array.Copy(solution, original, size * size);
            Reduce();
            //Print(solution, size, original);
            var result = this.Solve(0);
            return result ? FormatResult() : null;
        }

        readonly int size;
        readonly int seed;
        int[] Clues;
        Cell[] solution;

        public Skyscrapers4(int[] clues)
        {
            size = clues.Length / 4;
            seed = (1 << size) - 1;
            Clues = clues;
            solution = new Cell[size * size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    solution[size * i + j] = new Cell(size * i + j, size);
        }

        private int[][] FormatResult()
        {
            int[][] formated = new int[size][];
            for (int i = 0; i < size; i++)
            {
                formated[i] = new int[size];
                for (int j = 0; j < size; j++)
                    formated[i][j] = solution[i * size + j].Value;
            }
            return formated;
        }

        private static void PrintCell(Cell cell)
        {
            for (int i = 1; i <= cell.Size; i++)
            {
                if (cell.Contains(i))
                    Console.ForegroundColor = ConsoleColor.Blue;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                if (cell.Value == i)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0}", i);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private static void Print(Cell[] solution, int size, Cell[] previous)
        {
            Console.WriteLine();
            for (int i = 0; i < solution.Length; i++)
            {
                PrintCell(solution[i]);
                if ((i + 1) % size == 0) Console.WriteLine();
                else
                    Console.Write(" ");
            }
        }

        private void SetValue(int index, int val)
        {
            int left = index - index % size;
            int right = ((index + 1) / size) * size;
            for (int i = index - 1; i > left; i--)
                solution[i].RemoveValue(val);

            for (int i = index + 1; i < right; i++)
                solution[i].RemoveValue(val);

            for (int i = index - size; i >= 0; i -= size)
                solution[i].RemoveValue(val);

            for (int i = index + size; i < solution.Length; i += size)
                solution[i].RemoveValue(val);

            solution[index].SetValue(val);
        }

        private bool Solve(int index)
        {
            if (index < size * size)
            {
                Cell[] prev = new Cell[size * size];
                Array.Copy(solution, prev, size * size);
                foreach (var val in solution[index].ToArray())
                {
                    solution[index].SetValue(val);
                    //Print(solution);
                    if (TestSolution(index) && Solve(index + 1))
                        return true;
                    else
                        Array.Copy(prev, solution, size * size);
                }

                return false;
            }
            else
            {
                return TestSolution(index);
            }
        }
        private void Reduce()
        {
            //int[,] prev = new int[size, size];
            for (int index = 0; index < Clues.Length; index++)
            {
                //Array.Copy(solution, prev, size * size);
                if (Clues[index] == 0) continue;
                int start = GetStart(index), inc = GetIncrement(index);
                if (Clues[index] == size)
                    for (int i = start, k = 1; k <= size; k++, i += inc)
                        solution[i].SetValue(k);

                if (Clues[index] == 1)
                    solution[start].SetValue(size);

                for (int i = start, k = 1; k <= size; k++, i += inc)
                {
                    var f = size + k - Clues[index] + 1;
                    var valToRemove = new List<int>();
                    while (f <= size)
                        solution[i].RemoveValue(f++);

                }
                Print(solution, size, null);
                //Console.WriteLine();
            }
        }

        private void TrySet(int start, int inc)
        {
            for (int i = start, k = 0; k < size; k++, i += inc)
            {
                for (int v = 1; v <= size; v++)
                {
                    int count = 0;
                    for (int j = 0; j < size; j++)
                    {
                        if (solution[i].Contains(v))
                        {
                            count++;
                            if (count > 1) break;
                        }
                    }
                    if (count == 1)
                        solution[i].SetValue(v);
                }
            }
        }

        public int GetStart(int index)
        {
            if (index < size) return index;
            if (index >= 3 * size) return size * (4 * size - index - 1);
            if (index >= size) return size * (index - size) - 1;
            return (3 * size - index - 1) + (size - 1) * size;
        }
        public int GetIncrement(int index)
        {
            if (index < size) return size;
            if (index >= 3 * size) return 1;
            if (index <= 2 * size) return -1;
            return -size;
        }

        private bool TestSolution(int f)
        {
            for (int index = 0; index < Clues.Length; index++)
            {
                if (Clues[index] == 0) continue;
                int start = GetStart(index),
                    inc = GetIncrement(index),
                    max = 0,
                    count = 0;
                for (int j = start, k = start; k < size; k++, j += inc)
                {
                    if (!solution[j].IsFinal) break;
                    if (solution[j].Value > max)
                    {
                        max = solution[j].Value;
                        count++;
                    }
                    if (count != Clues[j]) return false;
                }
            }
            return true;
        }
    }
}
