namespace KataCSharp._6by6Skyscrapers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Skyscrapers
    {
        public static int[][] SolvePuzzle(int[] clues)
        {
            // Start your coding here...
            return null;
        }
        public class Cell
        {
            public Cell(Index index) { this.Index = index; }
            public Cell(Index index, int value) : this(index) { this.Value = value; }
            public Cell(int i, int j, int val) : this(new Index(i, j), val) { }
            public Index Index { get; set; }
            public int Value { get; set; }
            public override string ToString() => String.Format("{0} {1}", Index.ToString(), Value.ToString());
        }
        public struct Index
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public Index(int row, int column) { Row = row; Column = column; }
            public override string ToString() => String.Format("[{0}, {1}]", Row.ToString(), Column.ToString());
        }




        public class CellNode
        {
            public IEnumerable<Cell> Previous;
            public int Size;
            public Seed Seed;
            public bool IsInvalid;
            public Constraint[][] RowConstraints;
            public Constraint[][] ColumnConstraints;
            public Cell Current;
            private bool CheckRow(IEnumerable<int> row)
            {
                foreach (var con in RowConstraints[Current.Index.Row])
                {
                    if (!con.Satisfies(row))
                        return false;

                }
                return true;
            }
            private bool CheckColumn(IEnumerable<int> col) => ColumnConstraints[Current.Index.Column].All(x => x.Satisfies(col));

            private bool TryValue(int value)
            {
                var i = Current.Index.Row;
                var j = Current.Index.Column;
                if (i != Size - 1 && j != Size - 1) return true;

                if (i == Size - 1)
                {
                    if (!CheckColumn(GetCurrentColumn().Concat(new[] { value }))) return false;
                }
                if (j == Size - 1)
                {
                    if (!CheckRow(GetCurrentRow().Concat(new[] { value }))) return false;
                }
                return true;
            }

            public IEnumerable<Cell> Solve()
            {
                var i = Current.Index.Row;
                var j = Current.Index.Column;
                if (i == Size - 1 && j == Size - 1)
                {
                    foreach (var value in Seed.Rows[i])
                    {
                        if (Seed.Columns[j].Contains(value))
                        {
                            if (TryValue(value))
                                return Previous.Concat(new[] { new Cell(i, j, value) });
                        }
                    }
                }
                else
                {
                    foreach (var value in Seed.Rows[i])
                    {
                        if (Seed.Columns[j].Contains(value))
                        {
                            if (TryValue(value))
                            {
                                var child = CreateChild(value).Solve();
                                if (child.Count() > 0)
                                    return child;
                            }
                        }
                    }
                }
                return Enumerable.Empty<Cell>();
            }



            private Index GetNextIndex()
            {
                int i = Current.Index.Row, j = Current.Index.Column;
                if (j == Size - 1)
                {
                    i++;
                    j = 0;
                }
                else
                {
                    j++;
                }
                return new Index(i, j);
            }
            protected IEnumerable<int> CurrentRow;
            protected IEnumerable<int> CurrentColumn;
            protected IEnumerable<int> GetCurrentRow()
            {
                if (CurrentRow == null) CurrentRow = Previous.Where(x => x.Index.Row == this.Current.Index.Row).Select(x => x.Value);
                return CurrentRow;
            }
            protected IEnumerable<int> GetCurrentColumn()
            {
                if (CurrentColumn == null) CurrentColumn = Previous.Where(x => x.Index.Column == this.Current.Index.Column).Select(x => x.Value);
                return CurrentColumn;
            }


            private CellNode CreateChild(int value)
            {
                var seed = Seed.Clone();
                seed.Remove(this.Current.Index.Row, this.Current.Index.Column, value);
                return new CellNode()
                {
                    Current = new Cell(GetNextIndex()),
                    Size = this.Size,
                    Seed = seed,
                    RowConstraints = this.RowConstraints,
                    ColumnConstraints = this.ColumnConstraints,
                    Previous = this.Previous.Concat(new[] { new Cell(Current.Index, value) })
                };
            }
        }
        public class Seed
        {
            public List<int>[] Rows;
            public List<int>[] Columns;
            protected Seed(List<int>[] rows, List<int>[] columns)
            {
                this.Rows = rows;
                this.Columns = columns;
            }
            public Seed(int size)
            {
                if (size < 1) throw new ArgumentOutOfRangeException(nameof(size));
                Rows = new List<int>[size];
                Columns = new List<int>[size];
                for (int i = 0; i < size; i++)
                {
                    Rows[i] = GetIncSeq(size).ToList();
                    Columns[i] = GetIncSeq(size).ToList();
                }
            }
            public void Remove(int row, int column, int value)
            {
                this.Rows[row].Remove(value);
                this.Columns[column].Remove(value);
            }
            public void Add(int row, int column, int value)
            {
                this.Rows[row].Add(value);
                this.Columns[column].Add(value);
            }
            public Seed Clone() => new Seed(this.Rows.Select(x => new List<int>(x)).ToArray(), this.Columns.Select(x => new List<int>(x)).ToArray());
        }

        public class Constraint
        {
            private readonly Func<IEnumerable<int>, bool> constraint;
            public Constraint(Func<IEnumerable<int>, bool> predicate)
            {
                this.constraint = predicate;
            }
            public bool Satisfies(IEnumerable<int> values) => constraint(values);
        }



        public class Puzzle
        {
            Constraint[][] RowRules;
            Constraint[][] ColRules;
            int _size;
            public Puzzle(int[] clues)
            {
                _size = clues.Length / 4;
                this.RowRules = GetDefaultConstraints(_size).Select(x => new[] { x }).ToArray();
                this.ColRules = GetDefaultConstraints(_size).Select(x => new[] { x }).ToArray();
                ParseCluesConstraints(clues);
            }
            public int[][] Solve()
            {
                var start = new CellNode()
                {
                    RowConstraints = RowRules,
                    ColumnConstraints = ColRules,
                    Current = new Cell(0, 0, 0),
                    Previous = Enumerable.Empty<Cell>(),
                    Seed = new Seed(_size),
                    Size = _size
                };
                var solution = start.Solve();
                if (solution.Count() > 0) return GetSolution(solution);
                return null;
            }
            protected int[][] GetSolution(IEnumerable<Cell> raw)
            {
                int[][] solution = new int[_size][];
                for (int i = 0; i < solution.Length; i++)
                    solution[i] = new int[_size];
                foreach (var cell in raw)
                {
                    int i = cell.Index.Row;
                    int j = cell.Index.Column;
                    solution[i][j] = cell.Value;
                }
                return solution;
            }
            public static IEnumerable<Constraint> GetDefaultConstraints(int size)
            {
                var constraints = new List<Constraint>();
                for (int i = 0; i < size; i++)
                    constraints.Add(new Constraint(UniqueConstraint));
                return constraints;
            }
            private void ParseCluesConstraints(int[] clues)
            {
                for (int i = 0; i < _size; i++)
                {
                    var colC = clues[i];
                    var rowC = clues[4 * _size - i - 1];
                    var colCR = clues[3 * _size - i - 1];
                    var rowCR = clues[_size + i];
                    if (colC != 0)
                        this.ColRules[i] = this.ColRules[i].Concat(new[] { new Constraint(GetEqualsPredicate(colC)) }).ToArray();
                    //normal
                    if (rowC != 0)
                        this.RowRules[i] = this.RowRules[i].Concat(new[] { new Constraint(GetEqualsPredicate(rowC)) }).ToArray();
                    //reverse
                    if (colCR != 0)
                        this.ColRules[i] = this.ColRules[i].Concat(new[] { new Constraint(GetReversEqualsPredicate(colCR)) }).ToArray();
                    if (rowCR != 0)
                        this.RowRules[i] = this.RowRules[i].Concat(new[] { new Constraint(GetReversEqualsPredicate(rowCR)) }).ToArray();
                }
            }
            public static Func<IEnumerable<int>, bool> GetEqualsPredicate(int count)
            {
                return x => CountSteps(x) == count;
            }
            public static Func<IEnumerable<int>, bool> GetReversEqualsPredicate(int count)
            {
                return x => CountSteps(x.Reverse()) == count;
            }

            public static bool UniqueConstraint(IEnumerable<int> values)
            {
                if (values.Distinct().Count() == values.Count())
                    return true;
                return false;
            }
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
        public static void Print(IEnumerable<Cell> mat)
        {
            string message = Environment.NewLine;
            if (mat.Count() > 0)
            {
                int row = mat.First().Index.Row;
                foreach (var cell in mat)
                {
                    if (cell.Index.Row != row)
                    {
                        message += Environment.NewLine;
                        row = cell.Index.Row;
                    }
                    message += "\t" + cell.ToString();
                }
            }
            Console.WriteLine(message);
        }
    }
}
