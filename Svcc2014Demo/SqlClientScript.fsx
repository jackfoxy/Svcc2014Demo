
#load @"C:\FsEye\FsEye.fsx"
#I "C:\Users\Jack\Dropbox\GitHub\Svcc2014Demo\packages" 
#I "C:\Users\Jack\Dropbox\GitHub\Svcc2014Demo\Svcc2014Demo"
#r @"FSharp.Data.SqlClient.1.3.6-beta\lib\net40\FSharp.Data.SqlClient.dll"
#I "C:\Users\Jack\Dropbox\GitHub\Svcc2014Demo\packages" 

open System

open FSharp.Data

[<Literal>]
let connectionString = @"Data Source=.;Initial Catalog=AdventureWorks2012;Integrated Security=SSPI"

type AdventureWorks = SqlProgrammabilityProvider<connectionString>

type Dbo = AdventureWorks.dbo

type AccountingStartDate = AdventureWorks.dbo.ufnGetAccountingStartDate

let cmd = new AccountingStartDate()
cmd.AsyncExecute() |> Async.RunSynchronously

type BillOfMaterials = AdventureWorks.dbo.uspGetBillOfMaterials

let cmd2 = new BillOfMaterials()
cmd2.AsyncExecute(1, DateTime.UtcNow) |> Async.RunSynchronously

type ProductCategory = SqlEnumProvider<"
    SELECT Name, ProductCategoryID 
    FROM Production.ProductCategory WITH (NOLOCK)", connectionString>

let AccessoriesId = ProductCategory.Accessories