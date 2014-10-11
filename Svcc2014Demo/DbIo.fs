namespace Svcc2014Demo

open Either
open FSharp.Data
open System

type SelectNotFound (message) = 
    inherit System.Exception(message)

type AdventureWorks = SqlProgrammabilityProvider<"name = AdventureWorks2012">

type Dbo = AdventureWorks.dbo

type xx = AdventureWorks.dbo.ufnGetAccountingStartDate

module DbIo =

    
    type ProductIdByName = SqlCommandProvider<"
    
    SELECT ProductID from Production.Product
    WHERE Name = @name

    ", "name = AdventureWorks2012", SingleRow = true>

    let productIdByName name =

        async {
            let! result =
                async {
                    use cmd = new ProductIdByName()
                    return! cmd.AsyncExecute(name = name) 
                }
                |> Async.Catch 
            
            match result  with
            | Choice1Of2 (Some productID) -> return Success productID
            | Choice1Of2 _ -> return Failure ( SelectNotFound( sprintf "unable to select ProductID for name '%s'" name ) :> Exception )
            | Choice2Of2 exn -> return Failure exn
        }

    type ProductAndDescription = SqlCommandProvider<"
    
    SELECT * FROM Production.vProductAndDescription
    WHERE ProductID = @productID

    ", "name = AdventureWorks2012", SingleRow = true>

    let productAndDescription productID =

        async {
            let! result =
                async {
                    use cmd = new ProductAndDescription()
                    return! cmd.AsyncExecute(productID = productID) 
                }
                |> Async.Catch 
            
            match result  with
            | Choice1Of2 (Some productID) -> return Success productID
            | Choice1Of2 _ -> return Failure ( SelectNotFound( sprintf "unable to select ProductAndDescription for productID '%i'" productID ) :> Exception )
            | Choice2Of2 exn -> return Failure exn
        }