C# Programmer to Create the Business Layer for a Game Server

# Description

Create the business layer of a game server using Entities Code First.

Please begin your response with the following line:

"Hello Rick, I have read your description."

## Context

The business logic of a web server running asp.net which will be called through a RESTful web API.

## User Stories

### Register

- As a player
- I want to register my email (with a password)
- So that I can create an account

### Login

- As a player
- I want to login on my device 
- (By providing my email and password)
- So that I can store my data

### Store data

- As a player
- I want my data to be stored online
- So that it will be synced across devices

### Store any data

- As a game designer
- I want to use a simple data key to store any data on the server
- So that I don't have to change the server code to store new player data



## Structure Overview

Simple Login with Online Player Prefs and Shared Game Data

### Methods:

- Register(Email, EncodedPassword)
- Login(EncodedCredentials) : ClientToken
- CreateSession(ClientToken) : SessionToken
- SetUserValue( SToken, Key, Value )
- SetSharedValue( SToken, Key, Value)
- SetNextValue(SToken, KeyPrefix, Value) // KeyPrefix.1(Next)
- GetUserValue( SToken, Key) //Get Last Set Value for that specific key
- GetUserValues( SToken, KeyPrefix) //Get last n values for that keyprefix
- GetUserValueObject( SToken, Key) // Get JSON for entire sub
hierarchy of values

Game Table filters all values by a game and has a certain time to live
before it is deleted
- CreateGame(TimeToLive) : GameToken
- SetGameValue( SToken GameToken, Key)
- etc.




### Key is like javascript absolute object reference:
Told.FunRun.HighScores.1.PlayerID

### Server Side

Key is converted to a hierarchical adjacent database table:

#### Key Table

- ID
- ParentID
- StringPart (Exclusive)
- IntPart (Exclusive)

#### Value Table
- ID
- KeyID
- StringValue (Exclusive)
- IntValue (Exclusive)

(Only Keys that are not parentIDs are allowed to have values)

The server would cache the parentID lookups to reduce db calls with a simple
keyprefix dictionary:

Told = 1
Told.FunRun = 2
Told.FunRun.HighScores = 5
Told.FunRun.HighScores.1 = 23
Told.FunRun.HighScores.1.PlayerID = 24

GetValue would find the longest key prefix from the dictionary, then
load up the rest of the key in the cache before getting to the target
ID.

GetValues with a range would simply do a search using the parentID of
the KeyPrefix:

Told.FunRun.HighScores.[10-20] would search for int values 10-20 where
parentID=5.


#### Example Get Keys:

Told.FunRun.HighScores.[10-20] // the ids of the high score objects or
the JSON for them 10-20 (because it is a parent)

..HighScores.[10-20].PlayerID
// the player ids for those high scores (because PlayerID is not a parent)

..HighScores.[10-20].PlayerID{...FunRun.Players.[$].Name}
// Get the player names for the high scores 10-20 (Because Name is not a parent)


## Input

Call any method

## Results

Return the correct return value from the method call


# Implementation

## Code Interface

You will create a single class that implements the defined interface that will represent the single access point for the business logic.

You will create everything is a simple console application project that uses Entities Code First for the data access layer and the default localDB.

## Test Interface

The code will be tested with simple test methods that you will write. You will need to create sample inputs and expected outputs for each method. You will need to create a setup method that will initialize the database with a sample set of data to use for testing. This setup will be used to reset the database after each test method.

---

# Steps

After being accepted on the project, you should follow the following steps:

## Create a Plan
1. Write a short description of what you will need to create.
2. Send the Plan to me.
3. Wait for my feedback whether the Plan is satisfactory.

## Correct the Plan

Upon receiving my feedback concerning your Plan:

1. Correct your plan based on my feedback.
2. Send the updated Plan to me.
3. Wait for my feedback whether the Plan is satisfactory.

## Write the Tests

Upon receiving my feedback that the Plan is OK.

1. Write the Tests according to the description.
2. Send the Tests to me as a document.
3. Wait for my feedback whether the Tests are correct.

## Correct the Tests

Upon receiving my feedback concerning the Tests:

1. Correct any mistakes in the Tests as indicated.
2. Send the updates to me.
3. Wait for my feedback.

## Implement the Requirements

Upon receiving approval that the Tests are OK:

1. Implement the Code with the required behavior.
2. Run the Tests on the Code to make sure it is passing each case.
3. Send the Code to me.
4. Wait for my feedback.

## Correct the Code

Upon receiving my feedback on the Code:

1. Correct any mistakes in the Code as indicated.
2. Send the updates to me.
3. Wait for my feedback.

