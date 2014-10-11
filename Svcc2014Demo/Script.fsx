
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
    if temperature > 450 then Success wellsToSteam
    else Failure ( BadSteamJob( sprintf "BadSteamJob temperature %i barrelsOfSteam %i wellsToSteam %A" temperature barrelsOfSteam wellsToSteam) :> Exception ) 

let wellsToSteam = [WellState();WellState();]


let result = steamJob2 500 4000 wellsToSteam

match result with
| Success steamedWells -> printfn "%A" steamedWells
| Failure exn -> printfn "%s" exn.Message

let bind nextFunction lastOutput = 
    match lastOutput with
    | Success s -> nextFunction s
    | Failure f -> Failure f

let asyncBind (nextFunction : 'T -> Async<Choice<'U, 'Error>>) (lastOutput : Async<Choice<'T, 'Error>>) : Async<Choice<'U, 'Error>> =
        async {
            let! value' = lastOutput
            match value' with
            | Success x -> return! nextFunction x
            | Failure exn -> return Failure exn
        }

let acidJob (solution : float) (barrelsOfAcid : int) (wellsToAcid : WellState list) =
    if barrelsOfAcid > 10 then Success wellsToSteam
    else Failure ( BadSteamJob( sprintf "BadAcidJob solution %f barrelsOfAcid %i wellsToSteam %A" solution barrelsOfAcid wellsToAcid) :> Exception ) 

let step1Temp = 500
let step1Bbl = 10000
let step2Solution = 25.0
let step2Bbl = 20

let step3Temp = 500
let step3Bbl = 10000
let step4Solution = 25.0
let step4Bbl = 20

let processWells = 
    steamJob2 step1Temp step1Bbl
    >> bind (acidJob step2Solution step2Bbl)
    >> bind (steamJob2 step3Temp step3Bbl)
    >> bind (acidJob step4Solution step4Bbl)

let run() =
    match processWells wellsToSteam with
    | Success xs -> printfn "success %A" xs
    | Failure exn -> printfn "%s" exn.Message

[<Measure>] type degF                   // degrees Fahrenheit
[<Measure>] type pct                    // percent
[<Measure>] type bbl                    // barrels
[<Measure>] type acidId                 // acid Id

let acidJobX (solution : float<pct>) (substanceId : int<acidId>) (barrelsOfAcid : int<bbl>) (wellsToAcid : WellState list) =
    if barrelsOfAcid > 10<bbl> then Success wellsToAcid
    else Failure ( BadSteamJob( sprintf "BadAcidJob solution %A barrelsOfAcid %A wellsToSteam %A" solution barrelsOfAcid wellsToAcid) :> Exception ) 

let steamJobX (temperature :int<degF>) (barrelsOfSteam : int<bbl>) (wellsToSteam : WellState list) =
    if temperature > 450<degF> then Success wellsToSteam
    else Failure ( BadSteamJob( sprintf "BadSteamJob temperature %A barrelsOfSteam %A wellsToSteam %A" temperature barrelsOfSteam wellsToSteam) :> Exception ) 

let tryUnitize<[<Measure>]'Measure> value = value |> Option.map (LanguagePrimitives.FloatWithMeasure<'Measure>)
let unitize32<[<Measure>]'Measure> value = LanguagePrimitives.Int32WithMeasure<'Measure> value

let step1TempX = 500<degF>
let step1BblX = 10000<bbl>
let step2SolutionX = 25.0<pct>
let step2BblX = 20<bbl>

let step3TempX = 500<degF>
let step3BblX = 10000<bbl>
let step4SolutionX = 25.0<pct>
let step4BblX = 20<bbl>