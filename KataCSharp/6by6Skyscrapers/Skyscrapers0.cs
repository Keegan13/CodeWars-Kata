using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KataCSharp._6by6Skyscrapers
{
    public class Skyscrapers0
    {
        public static int[][] SolvePuzzle(int[] clues)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            var notmine = new Skyscrapers0(clues);
            return notmine.solve(clues).Select(x => x.ToArray()).ToArray();
        }

        readonly int size;
        readonly int seed;
        int[] possible, s, e, inc, prev_possible;
        int[][] results;
        int[] Clues;
        const int SIDES = 4;

        public Skyscrapers0(int[] clues)
        {
            size = clues.Length / 4;
            seed = (1 << size) - 1;
            possible = new int[size * size];
            s = new int[SIDES * size];
            e = new int[SIDES * size];
            inc = new int[SIDES * size];
            results = new int[size][];
            for (int i = 0; i < size; i++)
                results[i] = new int[size];
            prev_possible = new int[size * size];
            Clues = clues;
        }


        List<int> order = new List<int>();

        public void print_binary(int x, int prev)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                int cur = x & (1 << i);
                int prv = prev & (1 << i);
                if (prv > cur)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (cur > prv)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write("{0}", cur > 0 ? 1 : 0);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        void print_possible()
        {

            for (int i = 0; i < size * size; i++)
            {
                print_binary(possible[i], prev_possible[i]);
                Console.Write(" ");
                if (i % size == size - 1) Console.WriteLine();
            }
            for (int i = 0; i < prev_possible.Length; i++)
                prev_possible[i] = possible[i];
        }

        private void Print(int[,] array)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    print_binary(array[i, j], 0);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        void set_value(int x, int v)
        {
            int m = seed ^ (1 << v);
            int s_row = x - x % size;
            int s_col = x % size;
            for (int i = 0; i < size; i++)
            {
                possible[s_row + i] &= m;
                possible[s_col + i * size] &= m;
            }
            possible[x] = 1 << v;
        }

        int check_unique()
        {
            int n_decides = 0;
            for (int i = 0; i < SIDES / 2 * size; i++)
            {

                Dictionary<int, List<int>> possible_indices = new Dictionary<int, List<int>>();
                for (int j = s[i], k = 0; k < size; j += inc[i], k++)
                {
                    for (int l = 0; l < size; l++)
                        if (((1 << l) & possible[j]) > 0)
                        {
                            if (!possible_indices.ContainsKey(l)) possible_indices.Add(l, new List<int>());
                            possible_indices[l].Add(j);
                        }
                }

                foreach (var iter in possible_indices)
                {
                    int val = iter.Key;
                    if (iter.Value.Count == 1)
                    {
                        int idx = iter.Value[0];
                        if (possible[idx] != (1 << val))
                        {
                            n_decides++;
                            set_value(idx, val);
                        }
                    }
                }

            }
            return n_decides;
        }

        private int count_possible(int val)
        {
            int n = 0;
            while (val > 0)
            {
                n += val & 1;
                val >>= 1;
            }
            return n;
        }

        bool valid()
        {
            for (int i = 0; i < SIDES * size; i++)
            {
                if (Clues[i] == 0) continue;

                bool is_decided = true;
                for (int j = s[i], k = 0; k < size; j += inc[i], k++)
                {
                    if (count_possible(possible[j]) != 1)
                    {
                        is_decided = false;
                        break;
                    }
                }

                if (is_decided)
                {
                    int largest = 0, n_clue = 0;
                    for (int j = s[i], k = 0; k < size; j += inc[i], k++)
                    {
                        if (largest < possible[j])
                        {
                            n_clue++;
                            largest = possible[j];
                        }
                    }
                    if (n_clue != Clues[i]) return false;
                }
            }

            return true;
        }

        void write_results()
        {
            for (int i = 0; i < size * size; i++)
            {
                int x = i / size, y = i % size;
                for (int j = 0; j < size; j++)
                {
                    if ((1 << j) == possible[i])
                    {
                        results[x][y] = j + 1;
                        break;
                    }
                }
            }
        }

        bool dfs(int idx)
        {
            if (idx >= order.Count)
            {
                if (valid())
                {
                    write_results();
                    return true;
                }
                return false;
            }

            //Console.WriteLine("Selecting {0} index",order[idx]);
            //print_possible();
            int i = order[idx];
            int[] possible_bak = new int[size * size];

            memcpy(possible_bak, possible);

            for (int j = 0; j < size; j++)
            {
                int m = (1 << j) & possible[i];
                if (m == 0) continue;

                set_value(i, j);
                bool found = false;
                if (valid())
                {
                    found = dfs(idx + 1);
                }
                if (found)
                {
                    return true;
                }
                memcpy(possible, possible_bak);
            }
            return false;
        }

        private void memcpy(int[] possible_bak, int[] possible)
        {
            for (int i = 0; i < possible.Length; i++)
                possible_bak[i] = possible[i];
        }

        public List<List<int>> solve(int[] clues)
        {
            Clues = clues;

            for (int i = 0; i < results.GetLength(0); i++)
                results[i] = new int[size];

            for (int i = 0; i < size * size; i++) possible[i] = seed;



            for (int i = 0; i < size; i++)
            {
                s[i] = i;
                e[i] = (size - 1) * size + i;
                inc[i] = size;
            }

            for (int i = 0, j = size; i < size; i++, j++)
            {
                s[j] = i * size + size - 1;
                e[j] = i * size;
                inc[j] = -1;
            }

            for (int i = 0, j = 2 * size; i < size; i++, j++)
            {
                s[j] = size * size - 1 - i;
                e[j] = size - 1 - i;
                inc[j] = -size;
            }

            for (int i = 0, j = 3 * size; i < size; i++, j++)
            {
                s[j] = (size - i - 1) * size;
                e[j] = (size - i) * size - 1;
                inc[j] = 1;
            }

            for (int i = 0; i < SIDES * size; i++)
            {
                // int i = 12;
                if (Clues[i] == 0) continue;
                for (int j = s[i], k = 0; k < size; j += inc[i], k++)
                {
                    int m = seed;
                    for (int l = size + k - Clues[i] + 1; l < size; l++) m ^= 1 << l;
                    possible[j] &= m;
                }
            }

            while (check_unique() > 0) ;

            List<KeyValuePair<int, int>> idx_npos = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < size * size; i++)
            {
                int n_possible = count_possible(possible[i]);
                if (n_possible > 1)
                {
                    idx_npos.Add(new KeyValuePair<int, int>(n_possible, i));
                }
            }

            idx_npos = idx_npos.OrderBy(x => x.Key).ToList();
            order.Clear();
            for (int i = 0; i < idx_npos.Count; i++)
            {
                order.Add(idx_npos[i].Value);
            }
            dfs(0);

            List<List<int>> r = new List<List<int>>();
            for (int i = 0; i < size; i++)
            {
                List<int> vec = new List<int>();
                for (int j = 0; j < size; j++) vec.Add(results[i][j]);
                r.Add(vec);
            }
            return r;
        }
    }
}
