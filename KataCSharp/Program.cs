using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace KataCSharp
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Execution Started");
            //Skyscraper6();
            Calculator();
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
        static void Decompose()
        {
            var dec = new Decompose();
            Console.WriteLine(dec.decompose(50));
        }
        static void TextAlignJustify()
        {
            Console.WriteLine(KataCSharp.TestAlignJustify.Kata.Distribute(12, 5).Aggregate("", (agg, next) => agg += " " + next));
            Console.WriteLine(KataCSharp.TestAlignJustify.Kata.Distribute(4, 5).Aggregate("", (agg, next) => agg += " " + next));
            Console.WriteLine(KataCSharp.TestAlignJustify.Kata.Distribute(5, 5).Aggregate("", (agg, next) => agg += " " + next));
            Console.WriteLine(KataCSharp.TestAlignJustify.Kata.Distribute(30, 7).Aggregate("", (agg, next) => agg += " " + next));
            var result = KataCSharp.TestAlignJustify.Kata.Justify(null, 7);
            Console.WriteLine("\"{0}\":{1}", result, result.Length);
        }
        static void FunctionalStreams()
        {
            void Print<T>(IEnumerable<T> collection) => Console.WriteLine("[" + collection.Aggregate("", (agg, next) => agg += ", " + next).Trim(',') + "]");

            var stream = KataCSharp.FunctionalStreams.Stream.Primes();
            Print(KataCSharp.FunctionalStreams.Stream.Take(stream, 10));
        }
        static void NextSmallerNumber()
        {
            var numbers = new[] { 21, 531, 2071 };
            foreach (var num in numbers)
                Console.WriteLine("f({0})={1}", num, KataCSharp.NextSmallerNumber.Kata.NextSmaller(num));

        }
        static void BestTravel()
        {
            Console.WriteLine(KataCSharp.BestTravel.SumOfK.chooseBestSum(501, 3, new[] { 162, 163, 165, 165, 167, 168, 170, 172, 173, 175 }.ToList()));
        }
        static void NextBiggerNumber()
        {
            Console.WriteLine("f({0})={1}", 2017, KataCSharp.NextBiggerNumber.Kata.NextBiggerNumber(513));
        }
        static void Skyscraper6()
        {
            //var clues = new[] { 2,2,3,1,4,
            //                    2,3,2,3,1,
            //                    1,3,2,2,2,
            //                    3,1,2,2,2 };

            var clues = new[]{ 0, 0, 0, 2, 2, 0,
                            0, 0, 0, 6, 3, 0,
                            0, 4, 0, 0, 0, 0,
                            4, 4, 0, 3, 0, 0};

            //            Execution Started
            //Old: 00:00:00.0000013
            //New: 00:27:56.4845257
            // 5, 6, 1, 4, 3, 2
            // 4, 1, 3, 2, 6, 5
            // 2, 3, 6, 1, 5, 4
            // 6, 5, 4, 3, 2, 1
            // 1, 2, 5, 6, 4, 3
            // 3, 4, 2, 5, 1, 6









            //var puzzle = new _6by6Skyscrapers.Skyscrapers.Puzzle(clues);
            //var solution = puzzle.Solve();
            //if (solution != null)
            //    _4by4Skyscapers.Skyscrapers.Print(solution);
            //else
            //    Console.WriteLine("Not found");

            Stopwatch s = new Stopwatch();
            s.Start();
            //var result = _4by4Skyscapers.Skyscrapers.SolvePuzzle(clues);
            s.Stop();
            Console.WriteLine("Old:" + s.Elapsed.ToString());
            //_4by4Skyscapers.Skyscrapers.Print(result);



            s.Restart();
            var puzzle = new _6by6Skyscrapers.Skyscrapers.Puzzle(clues);
            var result2 = puzzle.Solve();
            s.Stop();


            Console.WriteLine("New: " + s.Elapsed.ToString());
            _4by4Skyscapers.Skyscrapers.Print(result2);
        }
        static void Skyscrapers()
        {
            //var clues = new[]{ 2, 2, 1, 4, 3,
            //                   0, 4, 1, 2, 2,
            //                   3, 0, 4, 0, 2,
            //                   0 ,2, 0, 1, 0};

            var clues = new[]{ 3, 2, 2, 3, 2, 1,
                           1, 2, 3, 3, 2, 2,
                           5, 1, 2, 2, 4, 3,
                           3, 2, 1, 2, 2, 4};

            var expected = new[]{new []{ 2, 1, 4, 3, 5, 6},
                             new []{ 1, 6, 3, 2, 4, 5},
                             new []{ 4, 3, 6, 5, 1, 2},
                             new []{ 6, 5, 2, 1, 3, 4},
                             new []{ 5, 4, 1, 6, 2, 3},
                             new []{ 3, 2, 5, 4, 6, 1 }};

            var result = _4by4Skyscapers.Skyscrapers.SolvePuzzle(clues);
            if (result != null)
                _4by4Skyscapers.Skyscrapers.Print(result);
            else
                Console.WriteLine("Not found");
        }
        static void WheelerTransformation()
        {
            string[] words = new[] { "Humble Bundle" };
            foreach (var word in words)
            {
                var encoded = BorrowWheelerTransformation.Kata.Encode(word);
                var decoded = BorrowWheelerTransformation.Kata.Decode(encoded.Item1, encoded.Item2);
                Console.WriteLine("Encoding {0} : result: {1}", word, encoded.Item1);
                Console.WriteLine("Decoding {0} result: {1}", encoded.Item1, decoded);

            }
        }
        static void Skyscrapers6Vol2()
        {
            //Stopwatch s = new Stopwatch();
            //s.Start();
            //var puzzle = new _6by6Skyscrapers.Skysrapers2.Seed(7);
            //s.Stop();
            //foreach (var g in puzzle.LeftSteps)
            //{
            //    foreach (var arr in g)
            //        Console.WriteLine("From left {1} \t {0}", arr.Aggregate("", (agg, next) => agg += " " + next.ToString()), g.Key);
            //}
            //foreach (var g in puzzle.RightSteps)
            //{
            //    foreach (var arr in g)
            //        Console.WriteLine("From right {1} \t {0}", arr.Aggregate("", (agg, next) => agg += " " + next.ToString()), g.Key);
            //}
            //Console.WriteLine("Intialization time: {0}", s.Elapsed);

            //var clues = new[] { 0,0,1,2,
            //                  0,2,0,0,
            //                  0,3,0,0,
            //                  0,1,0,0};
            //var clues = new[]{ 2, 2, 1, 4, 3,
            //                   0, 4, 1, 2, 2,
            //                   3, 0, 4, 0, 2,
            //                   0 ,2, 0, 1, 0};

            var clues = new[]{ 3, 2, 2, 3, 2, 1,
                           1, 2, 3, 3, 2, 2,
                           5, 1, 2, 2, 4, 3,
                           3, 2, 1, 2, 2, 4};
            var s = new _6by6Skyscrapers.Skysrapers2.Node(clues);

        }


        static void Calculator()
        {
            var expressions = new[] { "123.45*(678.90 / (-2.5+ 11.5)-(80 -19) *33.25) / 20 + 11" };
            var calc = new KataCSharp.Calculator.Evaluator();
            foreach (var expression in expressions)
            {
                double result = 0;
                result = calc.Evaluate(expression);
                Console.WriteLine("Result of expression {0} = {1}", expression, result.ToString());
            }
        }
    }
}
