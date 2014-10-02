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