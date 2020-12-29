module Tests

open Swensen.Unquote
open System
open Xunit
open Demo
open Demo.Main

type ValidationTestCases () as this =
    inherit TheoryData<Input, Result<ValidInput, Input * string list>> ()
    
    let eightYearsAgo = DateTime.Now.AddYears -8 // <- valid
    let eighteenYearsAgo = DateTime.Now.AddYears -18 // <- invalid
    
    // all inputs missing
    do this.Add (
        { Name = None; DoB = None; Address = None },
        Error (
            { Name = None; DoB = None; Address = None },
            ["add1 is required"; "dob is required"; "name is required"]))

    // name present, but invalid
    do this.Add (
        { Name = Some "Bob"; DoB = None; Address = None },
        Error (
            { Name = None; DoB = None; Address = None },
            ["add1 is required"; "dob is required"; "no bob and toms allowed"]))

    // only name is valid
    do this.Add (
        { Name = Some "Alice"; DoB = None; Address = None },
        Error (
            { Name = Some "Alice"; DoB = None; Address = None },
            ["add1 is required"; "dob is required"]))

    // name valid, invalid date of birth
    do this.Add (
        { Name = Some "Alice"; DoB = Some eighteenYearsAgo; Address = None },
        Error (
            { Name = Some "Alice"; DoB = None; Address = None },
            ["add1 is required"; "get off my lawn"]))
    
    // address missing
    do this.Add (
        { Name = Some "Alice"; DoB = Some eightYearsAgo; Address = None },
        Error (
            { Name = Some "Alice"; DoB = Some eightYearsAgo; Address = None },
            ["add1 is required"]))
    
    // all inputs are valid
    do this.Add (
        { Name = Some "Alice"; DoB = Some eightYearsAgo; Address = Some "x" },
        Ok ({ Name = "Alice"; DoB = eightYearsAgo; Address = "x" }))

[<Theory; ClassData(typeof<ValidationTestCases>)>]
let ``Validation works`` input expected =
    let now = DateTime.Now
    let actual = validateInput now input
    actual =! expected
    