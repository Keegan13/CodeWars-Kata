using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KataCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Execution Started");
            FunctionalStreams();
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
    }
}
