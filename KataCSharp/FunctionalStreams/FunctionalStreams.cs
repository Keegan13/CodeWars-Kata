namespace KataCSharp.FunctionalStreams
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /*
        A Stream is an infinite sequence of items. It is defined recursively
        as a head item followed by the tail, which is another stream.
        Consequently, the tail has to be wrapped with Lazy to prevent
        evaluation.
    */
    public class Stream<T>
    {
        public readonly T Head;
        public readonly Lazy<Stream<T>> Tail;

        public Stream(T head, Lazy<Stream<T>> tail)
        {
            Head = head;
            Tail = tail;
        }
    }

    static class Stream
    {
        /*
            Your first task is to define a utility function which constructs a
            Stream given a head and a function returning a tail.
        */
        public static Stream<T> Cons<T>(T h, Func<Stream<T>> t)
        {

            return new Stream<T>(h, new Lazy<Stream<T>>(t, false));
        }

        // .------------------------------.
        // | Static constructor functions |
        // '------------------------------'

        // Construct a stream by repeating a value.
        public static Stream<T> Repeat<T>(T x) => Cons(x, () => Repeat(x));

        // Construct a stream by repeatedly applying a function.
        public static Stream<T> Iterate<T>(Func<T, T> f, T x) => Cons(x, () => Iterate(f, f(x)));

        // Construct a stream by repeating an enumeration forever.
        public static Stream<T> Cycle<T>(IEnumerable<T> a) => Cycle(a.GetEnumerator());

        private static Stream<T> Cycle<T>(IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext()) { enumerator.Reset(); enumerator.MoveNext(); }
            return Cons(enumerator.Current, () => Cycle(enumerator));
        }

        // Construct a stream by counting numbers starting from a given one.
        public static Stream<int> From(int x) => Cons(x, () => From(x + 1));

        // Same as From but count with a given step width.
        public static Stream<int> FromThen(int x, int d)
        {
            return Cons(x, () => Cons(x + d, () => FromThen(x + 2 * d, d)));
        }

        // .------------------------------------------.
        // | Stream reduction and modification (pure) |
        // '------------------------------------------'

        /*
            Being applied to a stream (x1, x2, x3, ...), Foldr shall return
            f(x1, f(x2, f(x3, ...))). Foldr is a right-associative fold.
            Thus applications of f are nested to the right.
        */
        public static U Foldr<T, U>(this Stream<T> s, Func<T, Func<U>, U> f)
        {
            return f(s.Head, () => s.Tail.Value.Foldr(f));
        }

        // Filter stream with a predicate function.
        public static Stream<T> Filter<T>(this Stream<T> s, Predicate<T> p)
        {
            var current = s;
            while (!p(current.Head))
                current = current.Tail.Value;
            return Cons(current.Head, () => Filter<T>(current.Tail.Value, p));
        }

        // Returns a given amount of elements from the stream.
        public static IEnumerable<T> Take<T>(this Stream<T> s, int n)
        {
            var current = s;
            List<T> result = new List<T>();
            for (int i = 0; i < n; i++)
            {
                result.Add(current.Head);
                current = current.Tail.Value;
            }
            return result;
        }

        // Drop a given amount of elements from the stream.
        public static Stream<T> Drop<T>(this Stream<T> s, int n)
        {
            var current = s;
            int i = 0;
            while (i < n)
            {
                current = current.Tail.Value; i++;
            }
            return current;
        }

        // Combine 2 streams with a function.
        public static Stream<R> ZipWith<T, U, R>(this Stream<T> s, Func<T, U, R> f, Stream<U> other)
        {
            return new Stream<R>(f(s.Head, other.Head), new Lazy<Stream<R>>(() => s.Tail.Value.ZipWith(f, other.Tail.Value)));
        }

        // Map every value of the stream with a function, returning a new stream.
        public static Stream<U> FMap<T, U>(this Stream<T> s, Func<T, U> f)
        {
            return new Stream<U>(f(s.Head), new Lazy<Stream<U>>(() => s.Tail.Value.FMap(f)));
        }

        // Return the stream of all fibonacci numbers.
        public static Stream<int> Fib()
        {
            Stream<int> first = null, second = null;
            first = Cons(0, () => second);
            second = Cons(1, () => Fib(first, second));
            return first;
        }
        private static Stream<int> Fib(Stream<int> prev1, Stream<int> prev2)
        {
            Stream<int> current = null;
            return current = Cons(prev1.Head + prev2.Head, () => Fib(prev2, current));
        }

        // Return the stream of all prime numbers.
        public static Stream<int> Primes()
        {
            Stream<int> current = null;
            return current = Cons(2, () => Primes(current));
        }
        public static Stream<int> Primes(Stream<int> prev)
        {
            var i = prev.Head + 1;
            while (!IsPrime(i)) i++;
            Stream<int> current = null;
            current = Cons(i, () => Primes(current));
            return current;
        }

        private static bool IsPrime(int val)
        {
            if (val == 1) return false;
            for (int i = 2; i <= Math.Ceiling(Math.Sqrt(val)); i++)
                if (val % i == 0) return false;
            return true;
        }
    }
}
