
namespace KataCSharp._6by6Skyscrapers
{
    using System;
    public class Skyscrapers
    {
        public static int[][] SolvePuzzle(int[] clues)
        {
            return new Skyscrapers(clues).Solve();
        }
        public int[][] Solve()
        {
            //var original = new int[size, size];
            //Array.Copy(solution, original, size * size);
            Reduce();
            //Print(solution, size, original);
            var result = this.Solve(0, 0);
            return result ? FormatResult() : null;
        }

        readonly int size;
        readonly int seed;
        int[] Clues;
        int[,] solution;

        public Skyscrapers(int[] clues)
        {
            size = clues.Length / 4;
            seed = (1 << size) - 1;
            Clues = clues;
            solution = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    solution[i, j] = seed;
        }

        private int[][] FormatResult()
        {
            int[][] formated = new int[size][];
            for (int i = 0; i < size; i++)
            {
                formated[i] = new int[size];
                for (int j = 0; j < size; j++)
                    formated[i][j] = GetInteger(solution[i, j]);
            }
            return formated;
        }

        private static void PrintBinVal(int value, int prev_val, int size)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                int cur = value & (1 << i);
                int prv = prev_val & (1 << i);
                if (prv > cur)
                    Console.ForegroundColor = ConsoleColor.Red;
                if (cur > prv)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0}", cur > 0 ? 1 : 0);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private static void Print(int[,] array, int size, int[,] prev = null)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    PrintBinVal(array[i, j], prev == null ? 0 : prev[i, j], size);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private void SetValue(int row, int col, int val)
        {
            var binVal = 1 << val;
            for (int i = 0; i < size; i++)
            {
                solution[row, i] &= solution[row, i] ^ binVal;
            }
            for (int i = 0; i < size; i++)
            {
                solution[i, col] &= solution[i, col] ^ binVal;
            }
            solution[row, col] = binVal;
        }
        private void RemoveValue(int row, int col, int val)
        {
            var binVal = 1 << val;
            solution[row, col] &= solution[row, col] ^ binVal;
        }

        private int GetInteger(int binVal)
        {
            for (int i = 0; i < size; i++)
            {
                if ((binVal & 1 << i) > 0) return i + 1;
            }
            return 0;
        }

        private bool Solve(int row, int col)
        {
            if (row < size && col < size)
            {
                for (int k = 0; k < size; k++)
                {
                    int[,] prev = new int[size, size];
                    Array.Copy(solution, prev, size * size);

                    if ((solution[row, col] & 1 << k) > 0)
                    {
                        SetValue(row, col, k);
                        //Print(solution);
                        if (TestSolution(row, col) && Solve(col + 1 == size ? row + 1 : row, col + 1 == size ? 0 : col + 1))
                            return true;
                        else
                            Array.Copy(prev, solution, size * size);
                    }
                }
                return false;
            }
            else
            {
                return TestSolution(size - 1, size - 1);
            }
        }
        private void Reduce()
        {
            //int[,] prev = new int[size, size];

            for (int index = 0; index < Clues.Length; index++)
            {
                //Array.Copy(solution, prev, size * size);
                if (Clues[index] == 0) continue;
                int rStart = 0, rInc = 0, cInc = 0, cStart = 0;
                if (index < size)
                {
                    rStart = 0;
                    rInc = 1;
                    cStart = index;
                    cInc = 0;
                }
                if (index >= 3 * size)
                {
                    cInc = 1;
                    cStart = 0;
                    rInc = 0;
                    rStart = 4 * size - index - 1;
                }
                if (index >= size && index < 2 * size)
                {
                    rStart = index - size;
                    cInc = -1;
                    cStart = size - 1;
                    rInc = 0;
                }
                if (index >= 2 * size && index < 3 * size)
                {
                    rInc = -1;
                    rStart = size - 1;
                    cStart = 3 * size - index - 1;
                    cInc = 0;
                }

                if (Clues[index] == size)
                {
                    for (int i = rStart, j = cStart, k = 0; k < size; k++, i += rInc, j += cInc)
                    {
                        SetValue(i, j, k);
                    }
                }
                if (Clues[index] == 1)
                {
                    SetValue(rStart, cStart, size - 1);
                }

                for (int i = rStart, j = cStart, k = 0; k < size; k++, i += rInc, j += cInc)
                {
                    for (int f = size + k - Clues[index] + 1; f < size; f++)
                    {
                        RemoveValue(i, j, f);
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    for (int v = 0; v < size; v++)
                    {
                        int count = 0;
                        int pos = 0;
                        for (int j = 0; j < size; j++)
                        {
                            if ((solution[i, j] & 1 << v) > 0)
                            {
                                count++;
                                pos = j;
                                if (count > 1) break;
                            }
                        }
                        if (count == 1)
                            SetValue(i, pos, v);
                    }
                }
                for (int j = 0; j < size; j++)
                {
                    for (int v = 0; v < size; v++)
                    {
                        int count = 0;
                        int pos = 0;
                        for (int i = 0; i < size; i++)
                        {
                            if ((solution[i, j] & 1 << v) > 0)
                            {
                                count++;
                                pos = i;
                                if (count > 1) break;
                            }
                        }
                        if (count == 1)
                            SetValue(pos, j, v);
                    }
                }

                //Print(solution, size, prev);
                //Console.WriteLine();
            }
        }
        private bool TestSolution(int row, int col)
        {
            if (row == size - 1)
            {
                for (int j = 0; j < col + 1; j++)
                {
                    if (Clues[j] == 0) continue;
                    int max = 0;
                    int count = 0;
                    for (int i = 0; i < size; i++)
                        if (solution[i, j] > max)
                        {
                            max = solution[i, j];
                            count++;
                        }
                    if (count != Clues[j]) return false;
                }
                for (int j = col; j >= 0; j--)
                {
                    if (Clues[3 * size - j - 1] == 0) continue;
                    int max = 0;
                    int count = 0;
                    for (int i = size - 1; i >= 0; i--)
                        if (solution[i, j] > max)
                        {
                            max = solution[i, j];
                            count++;
                        }
                    if (count != Clues[3 * size - j - 1]) return false;
                }
            }
            if (col == size - 1)
            {
                for (int i = 0; i < row + 1; i++)
                {
                    if (Clues[4 * size - i - 1] == 0) continue;
                    int max = 0;
                    int count = 0;
                    for (int j = 0; j < size; j++)
                        if (solution[i, j] > max)
                        {
                            max = solution[i, j];
                            count++;
                        }
                    if (count != Clues[4 * size - i - 1]) return false;
                }
                for (int i = row; i >= 0; i--)
                {
                    if (Clues[size + i] == 0) continue;
                    int max = 0;
                    int count = 0;
                    for (int j = size - 1; j >= 0; j--)
                        if (solution[i, j] > max)
                        {
                            max = solution[i, j];
                            count++;
                        }
                    if (count != Clues[size + i]) return false;
                }
            }
            return true;
        }
    }
}
