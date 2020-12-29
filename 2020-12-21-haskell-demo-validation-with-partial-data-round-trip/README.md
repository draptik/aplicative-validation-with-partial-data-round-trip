# Project setup

We are using `ghcup` in combination with `cabal`. For details see:

- [Setting Up Haskell Development Environment: The Basics](https://schooloffp.co/2020/07/25/setting-up-haskell-development-environment-the-basics.html)
- [Whirlwind Tour Of Cabal For
  Beginners](https://schooloffp.co/2020/08/17/whirlwind-tour-of-cabal-for-beginners.html)

## Setup from scratch

```sh
take Demo
cabal init

# check if everything works:
cabal build
cabal run
# previous command should output hello world to console
```

Move code to `src` folder and update `*.cabal` file:

```sh
mkdir src
mv Main.hs src/
```

```haskell
-- in file Demo.cabal, uncomment hs-source-dirs and add our src folder
hs-source-dirs:    src
```

Running `cabal run` should still output hello world to the console.

## Adding dependencies

Example adding `time` and `validation` packages. In `*.cabal`, add packages to `build-depends` section:

```haskell
cabal-version:       2.0
-- ...

executable Demo
  main-is:             Main.hs
  -- other-modules:
  -- other-extensions:
  build-depends:       base >=4.14 && <4.15,
                       time ^>= 1.11.1.1,
                       validation ^>= 1.1
  hs-source-dirs:      src
  default-language:    Haskell2010
```

NOTE: When using the `^>=` syntax for defining the package version we have to define cabal version 2
(first line in example above).

## Using dependencies in REPL (GHCi)

Trying to load the previous example into REPL (GHCi) using `:l src/Main.hs` will not work:

```sh
$ ghci
GHCi, version 8.10.2: https://www.haskell.org/ghc/  :? for help
Prelude> :l src/Main.hs
[1 of 1] Compiling Main             ( src/Main.hs, interpreted )

src/Main.hs:8:1: error:
    Could not find module ‘Data.Validation’
    Use -v (or `:set -v` in ghci) to see a list of the files searched for.
  |
8 | import Data.Validation
  | ^^^^^^^^^^^^^^^^^^^^^^
Failed, no modules loaded.
```
