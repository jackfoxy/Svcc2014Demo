namespace Svcc2014Demo

module Either =
//    /// Convenience constructor function for creating a `Choice1Of2`.
//    let inline Success x = Choice1Of2 x
//    /// Convenience constructor function for creating a `Choice2Of2`.
//    let Failure x = Choice2Of2 x

    let (|Success|Failure|) = function
        | Choice1Of2 x -> Success x
        | Choice2Of2 x -> Failure x
    
    /// Convenience constructor function for creating a `Choice1Of2`.
    let inline Success x = Choice1Of2 x
    /// Convenience constructor function for creating a `Choice2Of2`.
    let inline Failure x = Choice2Of2 x

    /// Wraps a function invocation with a try/with, returning a `Choice<_,exn>`.
    let inline protect f x =
        try
            Success (f x)
        with e -> Failure e

    /// Binds a two-track input, `choice`, to a one-track function, `f`, that produces a two-track output.
    let inline bind f (choice: Choice<_,_>) =
        match choice with
        | Choice1Of2 x -> f x
        | Choice2Of2 x -> Failure x
    let inline (>>=) m f = bind f m

    /// Binds a two-track input, async `choice`, to a one-track function returning async choice, `f`, that produces a two-track output.
    let inline asyncBind (value : Async<Choice<'T, 'Error>>, f : 'T -> Async<Choice<'U, 'Error>>) : Async<Choice<'U, 'Error>> =
        async {
        let! value' = value
        match value' with
        | Failure error ->
            return Failure error
        | Success x ->
            return! f x
        }
    
    /// Applies a function `f`, which is wrapped in a `Choice<_,_>`, to a `Choice<_,_>` type.
    let inline apply f choice =
        match f, choice with
        | Choice1Of2 f, Choice1Of2 choice -> Success (f choice)
        | Choice2Of2 e, _              -> Failure e
        | _           , Choice2Of2 e      -> Failure e
    let inline (<*>) f choice = apply f choice
    
    /// Applies `success` handler on the success track and the `failure` handler on the failure track of a `Choice<_,_>`.
    let inline bimap success failure choice =
        match choice with
        | Choice1Of2 x -> Success (success x)
        | Choice2Of2 x -> Failure (failure x)

    /// Maps the success track of a `Choice<_,_>` with the specified function `f` and ignores the failure path.
    let inline map f choice = bimap f id choice
    let inline (<!>) m f = map f m

    /// Selects the first successful choice between two options.
    /// If both fail, returns the failure result from the first option.
    let inline choose choice1 choice2 =
        match choice1, choice2 with
        | Choice1Of2 x, _            -> Success x
        | _           , Choice1Of2 x -> Success x
        | Choice2Of2 e, _            -> Failure e
    let inline (<|>) choice1 choice2 = choose choice1 choice2
    
    /// Defines a computation expression for working with Choice types.
    type EitherBuilder() =
        member __.Return x = Success x
        member __.ReturnFrom (m: Choice<_,_>) = m
        member __.Bind(m, f) = bind f m
        member __.Delay f = fun () -> f()
    /// Computation expression for working with Choice types.
    let either = EitherBuilder()
