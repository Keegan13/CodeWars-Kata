using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KataCSharp
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Execution Started");
            Skyscrapers();
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
        static void Skyscrapers()
        {
            var clues = new[]{ 2, 2, 1, 4, 3,
                               0, 4, 1, 2, 2,
                               3, 0, 4, 0, 2,
                               0 ,2, 0, 1, 0};

            //var clues = new[]{ 3, 2, 2, 3, 2, 1,
            //               1, 2, 3, 3, 2, 2,
            //               5, 1, 2, 2, 4, 3,
            //               3, 2, 1, 2, 2, 4};

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
    }
}
