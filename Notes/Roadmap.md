NOTE: These specifications may not be perfect. It is the programmer's duty to implement the intended meaning.

# Create ISimpleGameServer interface and SimpleGameServerProvider class

- This will act as the main access point for the business logic
- SimpleGameServerProvider class implements the interface
- Providers class is a static class
	- Providers.SimpleGameServer is a static property point to the static instance of the server provider

---

# Implement Login

## Add Login methods to ISimpleGameServer

- void Register(string email, string password)
- void Verify(string email)
- Guid Login(string email, string password) // Returns ClientToken
- Guid CreateSession(Guid ClientToken) // Returns sessionToken

## Create the Entity models

// See - http://msdn.microsoft.com/en-us/data/jj679962.aspx

- User:
	- int UserID
	- string Email //[Index(IsUnique = true)] 
	- string EncodedPassword
	- bool IsVerified

- UserClient:
	- Guid UserClientToken //[Key] 
	- int UserID

- UserSession:
	- Guid UserSessionToken //[Key] 
	- Guid UserClientToken
	- int UserID
	

## Create test data

Create a new db with entity data migrations
Initialize the new db with some users and passwords

## Create tests

Call each method in valid ways and invalid ways to make sure each succeeds or fails appropriately:

For example:
- Register with invalid email should fail
- Register with no password should fail
- Register with an already existing email should fail
- Register with a valid email and password should succeed


## Implement Login methods in SimpleGameServerProvider

Register should:

- verify that the email is plausible with a simple email verification regex
- encode the password by calling Providers.Encorder.EncodePassword(password) // Add this as an empty placeholder that simply returns the password without modification
- add the email and password to the User table
- call Providers.Mailer.SendVerificationEmail(email) // Add this to providers as an empty placeholder

Verify should:

- set the IsVerified to true for the user

Login should:

- confirm the email, password is correct //Remember to encode the password 
- create a new client entry // Create a client token with Guid.NewGuid()
- return the client token

CreateSession should:

- confirm the client token
- create a new session token
- return the session token

---

# Implement Key-Value Storage

## Add Set and Get methods to ISimpleGameServer

- void SetValue(Guid sessionToken, KeyValueScope scope, string key, string value);
- string GetValue(Guid sessionToken, KeyValueScope scope, string key);

enum KeyValueScope{

	User,
	Shared
}

## Create the Entity models for Key and Value

- Key:
	- int KeyID // Primary Key
	- int ParentID //Null or KeyID must exist
	- int Depth
	- string Name

- Value:
	- int ValueID // Primary Key
	- int KeyID
	- KeyValueScope Scope
	- string Value
	- int SetByUserID
	- datetime SetAtTime //Timestamp


## Create test data

### Key Table

- KeyID	ParentID	depth	name
- 1		NULL		0		"TOLD"


- 10	1			1		"MyGame"

- 100	10			2		"HighScores"

- 1000	100			3		"1"
- 10000	1000		4		"PlayerID"
- 10001	1000		4		"Score"

- 1001	100			3		"2"
- 10010	1001		4		"PlayerID"
- 10011	1001		4		"Score"

- 1002	100			3		"3"
- 10020	1002		4		"PlayerID"
- 10021	1002		4		"Score"



- 11	1			1		"Players"

- 110	11			2		"123"
- 1100	110			3		"PlayerName"

- 111	11			2		"234"
- 1110	111			3		"PlayerName"

- 112	11			2		"345"
- 1120	112			3		"PlayerName"


### Value Table

- ValueID	KeyID	Scope	Value		SetByUserID		SetAtTime
- 100		10000	1		"123"		1				2014-01-01 12:00
- 101		10001	1		"9500"		1				2014-01-01 12:00

- 102		10010	1		"234"		2				2014-01-02 12:00
- 103		10011	1		"9400"		2				2014-01-02 12:00

- 104		10020	1		"345"		3				2014-01-03 12:00
- 105		10021	1		"9600"		3				2014-01-03 12:00


- 106		1100	1		"Matthew"	1				2014-01-01 12:00
- 107		1110	1		"Mark"		2				2014-01-02 12:00
- 108		1120	1		"Luke"		3				2014-01-03 12:00



## Create Tests

- Call GetValue for multiple keys and verify the correct value is returned
	- TOLD.MyGame.HighScores.1.PlayerID ->  "123"
	- TOLD.MyGame.HighScores.2.PlayerID ->  "234"
	- TOLD.MyGame.Players.123.PlayerName ->  "Matthew"
	- TOLD.MyGame.Players.234.PlayerName ->  "Mark"

- Call GetValue for a nonexistent key should return ""
- Call SetValue should change the existing value (verify with GetValue)
- Call SetValue for a new key should create the key and value (verify with GetValue)


## Implement

Set Value should:

- Break the key into parts (KeyID	ParentID	depth	name)
- Create a key row for any missing parts
- Lookup the leaf KeyID for the key
- Use the leaf KeyID to set the value

Get Value should:

- Lookup the KeyID for the key
- Get the value for that KeyID
- Return the value


### In-Memory Lookups

A copy of the entire key table can be stored in-memory for lookups. 

	Dictionary<int, Key> keyIdLookup;

Then, a hashtable can be built from the hash of the full key:

	Dictionary<int, List<Key>> keyHashLookup;

To build the hashtable:

- Go through each key
	- (Example: KeyData = 10000	1000	4	"PlayerID")
- Create its full key from recursively following it's parents 
	- (Example: GetFullKey() => "TOLD.MyGame.HighScores.1.PlayerID")
- Create a hash from this full key
	- (Example: TOLD.MyGame.HighScores.1.PlayerID -> 983249832)
- Add that key to the keyHashLookup
	- (Example: TOLD.MyGame.HighScores.1.PlayerID -> 983249832)

To do a lookup using the full key:

- Calculate the hash for the full key
	- (Example: TOLD.MyGame.HighScores.1.PlayerID -> 983249832)
- Find keys with matching hashes
	- var matches = keyHashLookup[983249832]
- Verify the correct match has the correct full path by recusively followint it's parents
	- var key = matches.FirstOrDefault(m=>m.GetFullKey() == fullKey)

