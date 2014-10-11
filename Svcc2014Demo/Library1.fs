namespace Svcc2014Demo

open DbIo
open Either
open ExtCore.Control
open FSharp.Data
open System

type BadSteamJob (message) = 
    inherit System.Exception(message)


type BadJob (message) = 
    inherit System.Exception(message)

type WellState() = 
    member this.X = "F#"

type Choice<'T1, 'T2> =
| Success of 'T1
| Failure of 'T2

exception OutOfBounds of string

module Library1 =

    let handleError() = ()
    let condition1 = true
    let condition2 = true
    let condition3 = true
    let condition4 = true
    let condition5 = true
    let doThis() = ()
    let doThat() = ()
    let doSomeOtherThing() = ()
    let aTask() = ()
    let anotherTask() = ()
    let yetAnotherTask() = ()
    let finallyDone() = ()

    if condition1 then 
        doThis()
        if condition2 then 
            doThat() 
            if condition3 then 
                doSomeOtherThing()
                if condition3 then 
                    aTask()
                    if condition4 then 
                        anotherTask()
                        if condition5 then 
                            yetAnotherTask()
                            if condition5 then 
                                finallyDone()
                            else handleError()
                        else handleError()
                    else handleError()
                else handleError()
            else handleError()
        else handleError()
    else handleError()

    let f x y z =
        let w = 1
        //… //do something
        w //return

    let steamJob (temperature :int) (barrelsOfSteam : int) (wellsToSteam : WellState list) =
        let w = 1
        //… //do something
        w //return

    let mySteamJob = steamJob 500 10000
    let mySteamJob2 = steamJob 5000

    let steamJob2 (temperature :int) (barrelsOfSteam : int) (wellsToSteam : WellState list) =
        let w = 1
        //… //do something
        if w = 1 then Success wellsToSteam
        else Failure ( BadSteamJob( sprintf "BadSteamJob temperature %i barrelsOfSteam %i wellsToSteam %A" temperature barrelsOfSteam wellsToSteam) :> Exception ) 

    let wellsToSteam = [WellState();WellState();]


    let result = steamJob2 500 4000 wellsToSteam

//    let Success x = Choice1Of2 x
    let Failure x = Choice2Of2 x

    match result with
    | Success steamedWells -> printfn "%A" steamedWells
    | Failure exn -> printfn "%s" exn.Message

    let productModel name = async {
        let! result = asyncChoice {
            let! productId = productIdByName name
            let! prodDescription = productAndDescription productId
            
            let containsFoo = prodDescription.Description.Contains("foo")

            if containsFoo then
                return! async { return Failure ( OutOfBounds("Don't show customers foo")) }
            else
                let descriptionWords = prodDescription.Description.Split(" ".ToCharArray())
                let! productId2 = productIdByName descriptionWords.[0]
                let! prodDescription2 = productAndDescription productId2
                return prodDescription2.ProductModel
            }
        match result with
        | Choice1Of2 x -> return Choice1Of2 x
        | Choice2Of2 exn -> return Choice2Of2 exn
        }