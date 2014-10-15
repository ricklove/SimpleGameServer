Efficient key storage and lookups

The code in this document is pseudocode and indicates the intention, not the correct implementation.

--- 

Multiple Lineage Columns limits the depth, but it simplifies three issues:

- The DB is easier to understand (joining on the key columns will make the key obvious)
- Searching is simple (the key part ids can be found and searched)
- Finding a range is simple at any depth (each column can be filtered independently from others)

## Multiple Lineage Columns

- http://stackoverflow.com/a/6802687/684229

This will limit the depth of the keys, but that should be an acceptable design limitation.

30 max depth should be plenty for any key.

Each part of the key can have a big name (256 should be fine): nvarchar(256)


Key Table:

- ID	name
- 1		"TOLD"
- 37	"SpellWellFunRun"
- 59	"HighScores"
- 93123	"PlayerID"
- 24	"Players"
- 24732	"PlayerName"


Value Table:

A negative keyID represents an index rather than a specific key.

- ID		value		Key1ID	Key2ID	Key3ID	Key4ID	...	Key30ID		
- 4232		"Rick"		1		24		-3213	24732	...	NULL	
	- key = TOLD.Players.3213.PlayerName
- 32443		3213		1 		59 		-1		93123	...	NULL	
	- key = TOLD.HighScores.1.PlayerID




---

---

This is too complex and searching is a problem.


## Flat Table - Adjacency List with Depth (Rank is not needed)

Key Table:

- ID	ParentID	depth	name
- 1		NULL		0		"TOLD"
- 37	1			1		"SpellWellFunRun"
- 59	37			2		"HighScores"
- 1159	59			3		"1"
- 93123	1159		4		"PlayerID"
- 24	1			1		"Players"
- 7325	24			2		"3213"	
- 24732	7325		3		"PlayerName"	

Value Table:

- ID	KeyID	value
- 4232	24732	"Rick" 	// key = TOLD.Players.3213.PlayerName
- 32443	93123	3213 	// key = TOLD.HighScores.1.PlayerID
	

### Insertions

Insertions would simple break the path into multiple parts and ensure each part exists.

### Lookups

	SELECT *
	FROM TABLE
	WHERE name=="TOLD" AND depth==0
	OR name=="SpellWellFunRun" AND depth==1
	OR name=="HighScores" AND depth==2
	OR name=="1" AND depth==3

Then, for each "1" record, recursively check that the parent is in result set and is correct.

### In-Memory Lookups

A copy of the entire flat table can be stored in-memory for in-memory lookups. 

	Dictionary<int, KeyPart> idLookup;

In addition, it could be put in a hash lookup based on the full path:

	Dictionary<int, List<KeyPart>> hashLookup;

- The full path would be hashed and any potential matches found in hashLookup
- The parent id would be recursively checked using idLookup

### Range lookups


- Told.FunRun.HighScores.[10-20] 
	
	Get the JSON for highscores 10-20

- Told.FunRun.HighScores.[10-20].PlayerID

	the player ids for those high scores

- Told.FunRun.HighScores.[10-20].PlayerID{...FunRun.Players.$.Name}

	Get the player names for the high scores 10-20

#### Told.FunRun.HighScores.[10-20]

Convert to range lookup of children

	Dictionary<int, List<KeyPart>> childrenLookup;

Filter the children based on actual name:

	childrenLookup[GetId("Told.FunRun.HighScore")].Where( p=>p.name >= "10" && p.name <= "20" );

Recursively get all children part keys using childrenLookup.

Get all the data for every relevant key.

	SELECT *
	FROM Values
	WHERE key==a
	OR	key==b
	...
	OR key==z

Then, reconstruct the data into json:

	Told={FunRun={HighScores={10={PlayerID=432143},11={PlayerID=3213},...}}}


#### Told.FunRun.HighScores.[10-20].PlayerID

Same as above, but only include the PlayerID child and its children (which would be nothing).

#### Told.FunRun.HighScores.[10-20].PlayerID{Told.FunRun.Players.[$].PlayerName}

Same as above, except this includes a sub query for PlayerID.

After getting all the PlayerID values, run a separate query for each:

- Told.FunRun.Players.432143.PlayerName
- Told.FunRun.Players.3213.PlayerName

Combine all results into json:

	Told={FunRun={
		HighScores={10={PlayerID=432143},11={PlayerID=3213},...},
		Players={432143={ Name="Rick"},3213={Name="Firestorm"},...}
	}}



---

# References

Get only newest row:

- http://stackoverflow.com/questions/17327043/how-can-i-select-rows-with-most-recent-timestamp-for-each-key-value

A good overview of the options available for hierarchical data

- http://stackoverflow.com/questions/4048151/what-are-the-options-for-storing-hierarchical-data-in-a-relational-database

## Multiple Lineage Columns

- http://stackoverflow.com/a/6802687/684229

This will limit the depth of the keys, but that should be an acceptable design limitation.

30 max depth should be plenty for any key.

Each part of the key can have a big name (256 should be fine): nvarchar(256)


So the key will exist as a 32 column table: 1 pk int, 30 ints, and a nvarchar(256)

- PK	int	int		int		int		...	int		nvarchar(256)
- 1		1	NULL	NULL	NULL	...	NULL	"TOLD"
- 37	1 	37 		NULL	NULL	...	NULL	"SpellWellFunRun"
- 59	1 	37 		59		NULL	...	NULL	"HighScores"
- 1159	1 	37 		59		1		...	NULL	"1"

