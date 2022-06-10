# InlineLinq

As the glutton for punishment, I'm embarking on a 3rd System.Linq replacement (...previous efforts [here](https://github.com/manofstick/Cistern.ValueLinq) and [here](https://github.com/manofstick/Cistern.Linq))

The design goal here is to create a very as simple and lightweight as possible core, such that it's easy to extend to provide specific optimizations, using the rules of c#'s type inference and method polymorphism to their maximum extent.

It will be a value-based linq, like Cistern.ValueLinq, but I'm avoiding the push-stream in the core, which simplifies the whole process greatly.

`ArrayPool<>` usage will be opt-in, I don't think a library should leave a persistent memory footprint without approval.

Originally it replicate the System.Linq surface, but will not automattically provide on and off ramps to and from `IEnumerable<>`, but the plan would be then to suppliment with 

    public delegate TReturn FuncS<TState, T, TReturn>(in TState state, T t)
        where TState : struct;
   
 So that lambdas can be written using the "static" form to avoid variable capture, and hence allocations.
 
 i.e. the standard Where: 
 
     public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
     
 would gain a counterpart
 
     public static IEnumerable<TSource> Where<TSource, TState>(this IEnumerable<TSource> source, in TState state, FuncS<TState, TSource, bool> predicate)
     
 although unfortunatly this isn't helped by the C# compilers lack of type inference for byref args, hence leading to usage such as
 
     xx.Where((1, 2, 3), static (in (int X, int Y, int Z) state, int x) => x == state.X + state.Y + state.Y);
     
 But who knows, maybe the compiler dudes can be convince to enhance type inference so that a future usage could be something like:
 
     xx.Where((X:1, Y:2, Z:3), static (in state, x) => x == state.X + state.Y + state.Y);
     
 Anyway, enough jibber jabbering, lets do this...
     
 
