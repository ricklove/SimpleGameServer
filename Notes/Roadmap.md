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

The previous implementation proved to be too complex.

The below represents a complete replacement for the previous details.

This will use Multiple Lineage Columns for the key instead of an Adjacency List with depth. 


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
	- string Name

- Value:
	- int ValueID // Primary Key
	- string Value
	- int SetByUserID
	- datetime SetAtTime //Timestamp
	- KeyValueScope Scope
	- int Key1ID
	- int Key2ID
	- int Key3ID
	- ...
	- int Key23ID
	- int Key24ID


## Create test data

### Key Table

- KeyID	name
- 1		"TOLD"
- 2		"MyGame"
- 3		"HighScores"
- 4		"PlayerID"
- 5		"Score"

- 6		"Players"
- 7		"PlayerName"


### Value Table

Values:

- TOLD.MyGame.HighScores.1.PlayerID ->  "123"
- TOLD.MyGame.HighScores.1.Score ->  "9500"
- TOLD.MyGame.HighScores.2.PlayerID ->  "234"
- TOLD.MyGame.HighScores.2.Score ->  "9400"
- TOLD.MyGame.HighScores.3.PlayerID ->  "345"
- TOLD.MyGame.HighScores.3.Score ->  "9600"
- TOLD.Players.1.PlayeID ->  "345"
- TOLD.Players.1.PlayerName ->  "Matthew"
- TOLD.Players.2.PlayeID ->  "234"
- TOLD.Players.2.PlayerName ->  "Mark"
- TOLD.Players.3.PlayeID ->  "123"
- TOLD.Players.3.PlayerName ->  "Luke"

Table:

Int values in the key are mapped to a negative keyID.

- ValueID	Value		SetByUserID		SetAtTime			Scope	Key1ID	Key2ID	Key3ID	Key4ID	Key5ID  ... 	Key30ID
- 100		"123"		1				2014-01-01 12:00    1		1		2		3		-1		4		NULL	NULL
- 101		"9500"		1				2014-01-01 12:00    1		1		2		3		-1		5		NULL	NULL

- 102		"234"		2				2014-01-02 12:00    1		1		2		3		-2		4		...
- 103		"9400"		2				2014-01-02 12:00    1		1		2		3		-2		5		...

- 104		"345"		3				2014-01-03 12:00    1		1		2		3		-3		4		...
- 105		"9600"		3				2014-01-03 12:00    1		1		2		3		-3		5		...

- 106		"345"		3				2014-01-03 12:00    1		1		6		-1		4		NULL	...
- 107		"Matthew"	3				2014-01-03 12:00    1		1		6		-1		7		NULL	...
- 108		"234"		2				2014-01-02 12:00    1		1		6		-2		4		...
- 109		"Matthew"	2				2014-01-02 12:00    1		1		6		-1		7		...
- 110		"123"		1				2014-01-01 12:00    1		1		6		-3		4		...
- 111		"Matthew"	1				2014-01-01 12:00    1		1		6		-1		7		...



## Create Tests

- Call GetValue for multiple keys and verify the correct value is returned
	- TOLD.MyGame.HighScores.1.PlayerID ->  "123"
	- TOLD.MyGame.HighScores.2.PlayerID ->  "234"
	- TOLD.MyGame.Players.123.PlayerName ->  "Matthew"
	- TOLD.MyGame.Players.234.PlayerName ->  "Mark"
	- etc.

- Call GetValue for a nonexistent key should return ""
- Call SetValue should change the existing value (verify with GetValue)
- Call SetValue for a new key should create the key and value (verify with GetValue)


## Implement

Set Value should:

- Break the key into parts (string[] keyParts)
- Lookup the KeyID for each key part (int[] keyPartIDs)
	- Create a key row for any missing parts
	- Use a negative id value for any int key values
	- Example:
		- key = "TOLD.MyGame.HighScores.1.PlayerID"
		- keyParts = "TOLD", "MyGame", "HighScores", "1", "PlayerID"
		- // Map each key part to the correct keyID (see In-Memory Lookups below)
		- // TOLD=>1, MyGame=>2, HighScores=>3, 1=>-1, PlayerID=>4 
		- keyPartIDs = 1, 2, 3, -1, 4
		
- Map each part to its column (Key1ID Key2ID Key3ID ... Key30ID)
	- Example:
		- keyPartIDs = 1, 2, 3, -1, 4
		- Key1ID = 1
		- Key2ID = 2
		- Key3ID = 3
		- Key4ID = -1
		- Key5ID = 4

Get Value should:

- Get the KeyPartIDs (same as above)
- Get the value for that combination of KeyPartIDs	
		
		SELECT *
		FROM TABLE
		WHERE Key1ID == keyPartIDs[0]
		AND Key2ID == keyPartIDs[1]
		AND Key3ID == keyPartIDs[2]
		...
		AND Key30ID == keyPartIDs[29]

- Return the value


### In-Memory Lookups

A copy of the entire key table can be stored in-memory for lookups in some simple dictionaries. 

	Dictionary<string, int> keyIdLookup;
	Dictionary<int, string> keyIdReverseLookup;

Then it is simple to lookup any key without querying the db.

#### LookupKeyPart:

If the key is an int, it should return the negative value
If a key does not exist, it should be added to the db and the lookups

	int LookupKeyPart( string keyPart ) {

		int val;

		if( int.TryParse( keyPart, out val ) ){
			return -val;
		}

		if( !keyIDLookup.HasKey( keyPart ) ){
			// TODO: Add to DB key
			// TODO: Get the new ID for the new key
			// TODO: Add the mapping to the keyIDLookup and keyIDReverseLookup
		}
	
		return keyIDLookup[keyPart];

	}

Examples:

	LookupKeyPart("TOLD") => 1
	LookupKeyPart("MyGame") => 2
	LookupKeyPart("100") => -100

#### LookupKeyID:

	string LookupKeyID( int? keyID) {
		if( !keyID.HasValue ){
			return "";
		}

		if( keyID < 0 ){
			return "" + -keyID.Value;
		}

		return keyIdReverseLookup[keyID.Value];
	}

Examples:
	LookupKeyID(1) => "TOLD"
	LookupKeyID(-100) => 100



#### KeyIDs Property in Value class

	int[] KeyIDs{
		get{
			var ids = new List<int>();
			
			if( Key1ID.HasValue ) { ids.Add(Key1ID.Value); } else { return ids.ToArray(); }
			if( Key2ID.HasValue ) { ids.Add(Key2ID.Value); } else { return ids.ToArray(); }
			...
			if( Key30ID.HasValue ) { ids.Add(Key30ID.Value); } else { return ids.ToArray(); }
			
			return ids.ToArray();
		}
	}

#### KeyString Property in Value class

	string KeyString{
		get{
		
			StringBuilder text = new StringBuilder();
			
			foreach( var id in KeyIDs){
				text.Append( LookupKeyID( id ) + "." );
			}

			return text.ToString().TrimEnd('.');
		}
	}

