
namespace KataCSharp.ValidateSudokuNxN
{
    using System;
    using System.Linq;
    public class Sudoku
    {
        public readonly int[][] Board;
        public Sudoku(int[][] sudokuData)
        {
            Board = sudokuData;
        }

        public bool IsValid()
        {
            var N = Board.Length;
            if (Board.Any(x => x.Length != N)) return false;
            var sums = new int[3 * N];
            var n = (int)Math.Sqrt(N);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    sums[i] += Board[i][j];
                    sums[N + j] += Board[i][j];
                    sums[2 * N + n * (i / n) + j / n] += Board[i][j];
                }
            }
            var sum = Enumerable.Range(1, N).Sum();
            return sums.All(x => x == sum);
        }
    }
    public static class Entry
    {
        public static void Test(Sudoku s, string name, bool isValid)
        {
            var result = s.IsValid();
            if (result != isValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sudoku {0} is {1}valid but should be {2}valid", name, result ? "" : "not ", isValid ? "" : "not ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Sudoku {0} is {1}valid", name, result ? "" : "not ");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Run()
        {
            var goodSudoku1 = new Sudoku(
              new int[][] {
                    new int[] {7,8,4, 1,5,9, 3,2,6},
                    new int[] {5,3,9, 6,7,2, 8,4,1},
                    new int[] {6,1,2, 4,3,8, 7,5,9},

                    new int[] {9,2,8, 7,1,5, 4,6,3},
                    new int[] {3,5,7, 8,4,6, 1,9,2},
                    new int[] {4,6,1, 9,2,3, 5,8,7},

                    new int[] {8,7,6, 3,9,4, 2,1,5},
                    new int[] {2,4,3, 5,6,1, 9,7,8},
                    new int[] {1,9,5, 2,8,7, 6,3,4}
              });
            var goodSudoku2 = new Sudoku(
              new int[][] {
                    new int[] {1,4, 2,3},
                    new int[] {3,2, 4,1},

                    new int[] {4,1, 3,2},
                    new int[] {2,3, 1,4}
            });
            var badSudoku1 = new Sudoku(
              new int[][] {
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},

                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},

                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9}
              });
            var badSudoku2 = new Sudoku(
              new int[][] {
                    new int[] {1,2,3,4,5},
                    new int[] {1,2,3,4},

                    new int[] {1,2,3,4},
                    new int[] {1}
            });

            Test(goodSudoku1, nameof(goodSudoku1), true);
            Test(goodSudoku2, nameof(goodSudoku2), true);
            Test(badSudoku1, nameof(badSudoku1), false);
            Test(badSudoku2, nameof(badSudoku2), false);
        }
    }
}
