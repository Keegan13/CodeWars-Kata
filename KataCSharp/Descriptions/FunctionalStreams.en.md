Introduction
Infinite sequences are common, natural numbers for example are defined such that they always have a successor. In Haskell, most of the time lists are used to model infinite sequences, but why should we check for the empty case, if it will never occur?

Streams offer an elegant alternative to model infinite data. A stream has a head element and an infinite tail, in lazy languages this is no problem, since the tail gets only evaluated when needed. But in languages with strict evaluation we need a lambda function to delay evaluation.

####  In C# the Lazy class is used to model delayed values.

// A function is passed, but no Foo is created yet.
Lazy<Foo> lazyFoo = new Lazy<Foo>(() => new Foo());

// Accessing the Value property will evaluate the function and share the result.
Foo foo = lazyFoo.Value;
In pure functional code data is not mutated. Every reference is immutable. Calling a function won't change any state and should always return the same value.

Basics
Write headS which returns the value at the head of the stream and tailS which drops the first value of the stream.

Let the streams flow!
Implement functions to construct streams:

repeatS will repeat the same value over and over again.
iterateS will repeatedly apply the same function to the result of the last iteration (starting with a given value).
cycleS will repeat a list forever.
fromS will count numbers upwards by one.
fromStepS will count numbers with a given step width.
Modifying and reducing streams
To work with streams, we always have to write a computation which ends in finite time, therefore we can only inspect a finite part of the stream. Implement common functions:

foldrS folds a stream with a function from the left (The resulting structure can also be infinite!).
filterS picks only those values, which satisfy the predicate.
takeS takes a given number of elements from the stream.
dropS drops a given number of elements from the stream. (Hint: Make sure to handle negative values.)
Haskell only: splitAtS does take and drop at the same time.
zipWithS merges 2 streams, by applying a function to each pair of values.
Implement fmap, which maps every value of a stream with a function.
Haskell only: Write an instance of Applicative for streams (type signatures are provided, in case you don't know what that means).
To infinity and beyond
With the written combinators, we can already do very useful things, for example we can write functions without relying on the unsafe head and tail functions for lists.

Write streams for the following sequences:

fibS is the stream of all fibonacci numbers (starting with 0).
primeS is the stream of all prime numbers.
Hint: The obvious definition of primeS would be with filterS and an isPrime predicate, while this sounds simple, it is not the most efficient solution (since it has to brute-force all numbers up to the given number). Figure out a way to reduce the amount of numbers to test in every step!