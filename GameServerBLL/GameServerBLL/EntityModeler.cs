using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerDAL;
using GameServerDAL.Entities;

using DALScope = GameServerDAL.Entities.KeyValueScope;

namespace GameServerBLL
{
    public class EntityModeler : IDisposable
    {
        bool is_disposed = false;
        private GameServerContext db;
        private Encoder encoder;

        public EntityModeler()
        {
            db = GameServerContext.Instance;
            encoder = new Encoder();
        }

        public void AddTestData()
        {
            var user1 = new User { UserID = 1, Email = "faamirpk@yahoo.com", EncodedPassword = encoder.EncodePassword("pass1", "SHA512", null), IsVerified = true };
            var user2 = new User { UserID = 2, Email = "test2222@yahoo.com", EncodedPassword = encoder.EncodePassword("pass2", "SHA512", null), IsVerified = true };
            var user3 = new User { UserID = 3, Email = "test3333@yahoo.com", EncodedPassword = encoder.EncodePassword("pass3", "SHA512", null), IsVerified = true };
            var user4 = new User { UserID = 4, Email = "test4444@yahoo.com", EncodedPassword = encoder.EncodePassword("pass4", "SHA512", null), IsVerified = true };

            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Users.Add(user3);
            db.Users.Add(user4);

            var aGuid = Guid.NewGuid();

            var userClient1 = new UserClient { UserClientToken = aGuid, UserID = 1 };
            db.UserClients.Add(userClient1);

            var userSession1 = new UserSession { UserSessionToken = Guid.NewGuid(), UserClientToken = aGuid, UserID = 1 };
            db.UserSessions.Add(userSession1);

            db.SaveChanges();
        }

        public void AddTestDataKeyValue()
        {
            // key table
            var key1 = new Key { Name = "TOLD" }; // KeyID primary key is identity
            var key2 = new Key { Name = "MyGame" };
            var key3 = new Key { Name = "HighScores" };
            var key4 = new Key { Name = "PlayerID" };
            var key5 = new Key { Name = "Score" };

            var key6 = new Key { Name = "Players" };
            var key7 = new Key { Name = "PlayerName" };

            List<Key> keyList = new List<Key>() { key1, key2, key3, key4, key5, key6, key7 };
            db.Keys.AddRange(keyList);

            // value table /*ValueID = 100,*/
            var value1 = new Value { Val = "123", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 2, Key3ID = 3, Key4ID = -1, Key5ID = 4 };
            var value2 = new Value { Val = "9500", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 2, Key3ID = 3, Key4ID = -1, Key5ID = 5 };

            var value3 = new Value { Val = "234", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-02 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 2, Key3ID = 3, Key4ID = -2, Key5ID = 4 };
            var value4 = new Value { Val = "9400", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-02 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 2, Key3ID = 3, Key4ID = -2, Key5ID = 5 };

            var value5 = new Value { Val = "345", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-03 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 2, Key3ID = 3, Key4ID = -3, Key5ID = 4 };
            var value6 = new Value { Val = "9600", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-03 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 2, Key3ID = 3, Key4ID = -3, Key5ID = 5 };

            var value7 = new Value { Val = "345", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-03 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 6, Key3ID = -1, Key4ID = 4 };
            var value8 = new Value { Val = "Matthew", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-03 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 6, Key3ID = -1, Key4ID = 7 };
            var value9 = new Value { Val = "234", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-02 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 6, Key3ID = -1, Key4ID = 4, };
            var value10 = new Value { Val = "Matthew", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-02 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 6, Key3ID = -1, Key4ID = 7 };
            var value11 = new Value { Val = "123", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 6, Key3ID = -1, Key4ID = 4 };
            var value12 = new Value { Val = "Matthew", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00"), Scope = DALScope.Shared, Key1ID = 1, Key2ID = 6, Key3ID = -1, Key4ID = 7 };

            List<Value> valueList = new List<Value>() { value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12 };
            db.Values.AddRange(valueList);

            // save changes to DB
            db.SaveChanges();

            // Build Cache
            Cache cache = Cache.Instance;
            cache.Initialize(db.Keys, db.Values);
        }

        // add key row to Key table
        public void AddKeyRow(string name)
        {
            var keyRow = new Key { Name = name }; // KeyID primary key is identity
            db.Keys.Add(keyRow);
            db.SaveChanges();
        }

        // TODO: Get the ID for the new key
        public int GetIdForKeyPart(string keyPart)
        {
            return db.Keys.Where(keyRow => keyRow.Name.Equals(keyPart)).SingleOrDefault().KeyID;
        }

        public List<string> ShowUsers()
        {
            List<string> usersEmail = new List<string>();

            var query = from u in db.Users
                        orderby u.Email
                        select u;

            foreach (var item in query)
            {
                usersEmail.Add(item.Email);
            }

            return usersEmail;
        }

        public int GetHashForFullKey(string fullKey)
        {
            return fullKey.GetHashCode();
        }

        //public void FindKeyForHash(int hashCode)
        //{
        //    Cache cache = Cache.Instance;
        //    cache.Initialize(db.Keys, db.Values);

        //    cache.FindKeyForHash(hashCode);
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (!is_disposed)
            {
                if (disposing)
                {
                }
                this.db.Dispose();
            }
            this.is_disposed = true;
        }

        public void Dispose( )
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EntityModeler()
        {
            Dispose(false);
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }


	}
}
