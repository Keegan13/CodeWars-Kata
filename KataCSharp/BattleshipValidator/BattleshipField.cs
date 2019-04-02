namespace KataCSharp.BattleshipValidator
{
    using System;
    public class BattleshipField
    {
        public static bool ValidateBattlefield(int[,] field)
        {
            var shipCount = new int[4];
            int len = field.GetLength(0);
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                {
                    int shipSize = 1;
                    if (field[i, j] == 0) continue;
                    if (i + 1 < len && ((j + 1 < len && field[i + 1, j + 1] == 1) || (j > 0 && field[i + 1, j - 1] == 1))) return false;
                    if (i == 0 || field[i - 1, j] != 1)
                    {
                        while (i + shipSize < len && field[i + shipSize, j] == 1) shipSize++;
                        if (shipSize == 1)
                        {
                            while (j + shipSize < len && field[i, j + shipSize] == 1) shipSize++;
                            j += shipSize;
                        }
                        if (shipSize > 4) return false;
                        shipCount[shipSize - 1]++;
                    }
                }
            for (int i = 0; i < 4; i++)
                if (shipCount[i] != 4 - i)
                    return false;
            return true;
        }
    }
    public static class Entry
    {
        public static void Run()
        {
            Test();
        }

        public static void Test()
        {
            int[,] field = new int[10, 10]
                     {{1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                      {1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
                      {1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
                      {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                      {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                      {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                      {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                      {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                      {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
            Console.WriteLine("Should be true: {0}", BattleshipField.ValidateBattlefield(field));
        }
    }
}
