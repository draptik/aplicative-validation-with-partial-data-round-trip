module Main where

import Data.Bifunctor
import Data.Time
import Data.Semigroup
import Data.Validation

main :: IO ()
main = putStrLn "Hello, Haskell!"


data Input = Input {
    inputName :: Maybe String,
    inputDoB :: Maybe Day,
    inputAddress :: Maybe String
} deriving (Eq, Show)

data ValidInput = ValidInput {
    validName :: String,
    validDoB :: Day,
    validAddress :: String
} deriving (Eq, Show)


validateName :: Input -> Validation (Endo Input, [String]) String
validateName (Input (Just name) _ _) | length name > 3 = Success name
validateName (Input (Just _) _ _) =
  Failure (Endo $ \x -> x { inputName = Nothing }, ["no bob and toms allowed"])
validateName _ = Failure (mempty, ["name is required"])

validateDoB :: Day -> Input -> Validation (Endo Input, [String]) Day
validateDoB now (Input _ (Just dob) _) | addGregorianYearsRollOver (-12) now < dob =
  Success dob
validateDoB _ (Input _ (Just _) _) =
  Failure (Endo $ \x -> x { inputDoB = Nothing }, ["get off my lawn"])
validateDoB _ _ = Failure (mempty, ["dob is required"])

validateAddress :: Monoid a => Input -> Validation (a, [String]) String
validateAddress (Input _ _ (Just a)) = Success a
validateAddress _ = Failure (mempty, ["add1 is required"])

validateInput :: Day -> Input -> Either (Input, [String]) ValidInput
validateInput now args =
  toEither $
  first (first (`appEndo` args)) $
  ValidInput <$> validateName args <*> validateDoB now args <*> validateAddress args

{-|
*Main> current <- getCurrentTime
*Main> now = utctDay current
*Main> now
2020-12-29
*Main> :t now
now :: Day


*Main> current <- getCurrentTime
*Main> now = utctDay current
*Main> validateInput now & Input (Just "Alice") (Just & fromGregorian 2012 4 21) (Just "x")

<interactive>:3:19: error:
    Variable not in scope:
      (&) :: (Input -> Either (Input, [String]) ValidInput) -> Input -> t

<interactive>:3:48: error:
    Variable not in scope: (&) :: (a0 -> Maybe a0) -> Day -> Maybe Day
*Main>
-}
