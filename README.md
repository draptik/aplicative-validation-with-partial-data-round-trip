# Applicative validation with partial data round trip

Code examples from Mark Seemann's blog during 2020 F# advent of code.

Blog posts:

= [A Haskell proof of concept of validation with partial data round trip](https://blog.ploeh.dk/2020/12/21/a-haskell-proof-of-concept-of-validation-with-partial-data-round-trip/)
- [An F# demo of validation with partial data round trip](https://blog.ploeh.dk/2020/12/28/an-f-demo-of-validation-with-partial-data-round-trip/)

Applicatives with "a twist" [source](https://blog.ploeh.dk/2020/12/28/an-f-demo-of-validation-with-partial-data-round-trip/):

> The twist is that validation, when it fails, should return not only a list of error messages; it should also retain that part of the input that was valid.

NOTE: The F# demo requires F#5 (which is shipped by .NET5).
