namespace Demo

open System

type Input = { Name: string option; DoB: DateTime option; Address: string option }
type ValidInput = { Name: string; DoB: DateTime; Address: string }

module Result =
    // Result<'a       ,(('b -> 'b) * 'c list)> ->
    // Result<'d       ,(('b -> 'b) * 'c list)> ->
    // Result<('a * 'd),(('b -> 'b) * 'c list)>
    let merge x y =
        match x, y with
        | Ok xres, Ok yres -> Ok (xres, yres)
        | Error (f, e1s), Error (g, e2s) -> Error (f >> g, e2s @ e1s)
        | Error e, Ok _ -> Error e
        | Ok _, Error e -> Error e

(*
NOTE: `BindReturn` and `MergeSources` are new in F# 5.

Resources (search for `MergeSources`...):

- https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/
- https://www.codemag.com/Article/2010072/F

- https://thinkbeforecoding.com/post/2020/10/03/applicatives-irl
- https://thinkbeforecoding.com/post/2020/10/07/applicative-computation-expressions
- https://thinkbeforecoding.com/post/2020/10/08/applicative-computation-expressions-2
*)        
type ValidationBuilder () =
    member _.BindReturn (x, f) = Result.map f x
    member _.MergeSources (x, y) = Result.merge x y

[<AutoOpen>]
module ComputationalExpressions =
    let validation = ValidationBuilder ()
    
module Main =

    let validateName ({Name = name} : Input) =
        match name with
        | Some n when n.Length > 3 -> Ok n
        | Some _ ->
            Error (
                (fun (args : Input) -> {args with Name = None}),
                ["no bob and toms allowed"])
        | None -> Error (id, ["name is required"])

    let validateDoB (now : DateTime) ({DoB = dob} : Input) =
        match dob with
        | Some d when d > now.AddYears -12 -> Ok d
        | Some _ ->
            Error (
                (fun (args: Input) -> {args with DoB = None}),
                ["get off my lawn"])
        | None -> Error (id, ["dob is required"])
        
    let validateAddress ({Address = address} : Input) =
        match address with
        | Some a -> Ok a
        | None -> Error (id, ["add1 is required"])
    
    let validateInput now args =
        validation {
            let! name = validateName args
            and! dob = validateDoB now args
            and! address = validateAddress args
            return { Name = name; DoB = dob; Address = address }
        }
        |> Result.mapError (fun (f, msgs) -> f args, msgs)
        