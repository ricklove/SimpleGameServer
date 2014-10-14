using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerDAL;
using GameServerDAL.Entities;

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
            var key1 = new Key { KeyID = 1, ParentID = null, Depth = 0, Name = "TOLD" };
            var key2 = new Key { KeyID = 10, ParentID = 1, Depth = 1, Name = "MyGame" };
            var key3 = new Key { KeyID = 100, ParentID = 10, Depth = 2, Name = "HighScores" };
            var key4 = new Key { KeyID = 1000, ParentID = 100, Depth = 3, Name = "1" };
            var key5 = new Key { KeyID = 10000, ParentID = 1000, Depth = 4, Name = "PlayerID" };
            var key6 = new Key { KeyID = 10001, ParentID = 1000, Depth = 4, Name = "Score" };

            var key7 = new Key { KeyID = 1001, ParentID = 100, Depth = 3, Name = "2" };
            var key8 = new Key { KeyID = 10010, ParentID = 1001, Depth = 4, Name = "PlayerID" };
            var key9 = new Key { KeyID = 10011, ParentID = 1001, Depth = 4, Name = "Score" };

            var key10 = new Key { KeyID = 1002, ParentID = 100, Depth = 3, Name = "3" };
            var key11 = new Key { KeyID = 10020, ParentID = 1002, Depth = 4, Name = "PlayerID" };
            var key12 = new Key { KeyID = 10021, ParentID = 1002, Depth = 4, Name = "Score" };

            var key13 = new Key { KeyID = 11, ParentID = 1, Depth = 1, Name = "Players" };
            var key14 = new Key { KeyID = 110, ParentID = 11, Depth = 2, Name = "123" };
            var key15 = new Key { KeyID = 1100, ParentID = 110, Depth = 3, Name = "PlayerName" };

            var key16 = new Key { KeyID = 111, ParentID = 11, Depth = 2, Name = "234" };
            var key17 = new Key { KeyID = 1110, ParentID = 111, Depth = 3, Name = "PlayerName" };

            var key18 = new Key { KeyID = 112, ParentID = 11, Depth = 2, Name = "345" };
            var key19 = new Key { KeyID = 1120, ParentID = 112, Depth = 3, Name = "PlayerName" };

            db.Keys.Add(key1);
            db.Keys.Add(key2);
            db.Keys.Add(key3);
            db.Keys.Add(key4);
            db.Keys.Add(key5);
            db.Keys.Add(key6);
            db.Keys.Add(key7);
            db.Keys.Add(key8);
            db.Keys.Add(key9);
            db.Keys.Add(key10);

            db.Keys.Add(key11);
            db.Keys.Add(key12);
            db.Keys.Add(key13);
            db.Keys.Add(key14);
            db.Keys.Add(key15);
            db.Keys.Add(key16);
            db.Keys.Add(key17);
            db.Keys.Add(key18);
            db.Keys.Add(key19);

            // value table
            var value1 = new Value { ValueID = 100, KeyID = 10000, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "123", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00") };
            var value2 = new Value { ValueID = 101, KeyID = 10001, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "9500", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00") };

            var value3 = new Value { ValueID = 102, KeyID = 10010, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "234", SetByUserID = 2, SetAtTime = DateTime.Parse("2014-01-02 12:00") };
            var value4 = new Value { ValueID = 103, KeyID = 10011, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "9400", SetByUserID = 2, SetAtTime = DateTime.Parse("2014-01-02 12:00") };

            var value5 = new Value { ValueID = 104, KeyID = 10020, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "345", SetByUserID = 3, SetAtTime = DateTime.Parse("2014-01-03 12:00") };
            var value6 = new Value { ValueID = 105, KeyID = 10021, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "9600", SetByUserID = 3, SetAtTime = DateTime.Parse("2014-01-03 12:00") };

            var value7 = new Value { ValueID = 106, KeyID = 1100, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "Matthew", SetByUserID = 1, SetAtTime = DateTime.Parse("2014-01-01 12:00") };
            var value8 = new Value { ValueID = 107, KeyID = 1110, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "Mark", SetByUserID = 2, SetAtTime = DateTime.Parse("2014-01-02 12:00") };
            var value9 = new Value { ValueID = 108, KeyID = 1120, Scope = GameServerDAL.Entities.KeyValueScope.Shared, Val = "Luke", SetByUserID = 3, SetAtTime = DateTime.Parse("2014-01-03 12:00") };

            db.Values.Add(value1);
            db.Values.Add(value2);
            db.Values.Add(value3);
            db.Values.Add(value4);
            db.Values.Add(value5);
            db.Values.Add(value6);
            db.Values.Add(value7);
            db.Values.Add(value8);
            db.Values.Add(value9);


            db.SaveChanges();
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

        public void DelteAllData()
        {
            db.UserSessions.RemoveRange(db.UserSessions.Select(s => s));
            db.UserSessions.RemoveRange(db.UserSessions);
            db.UserClients.RemoveRange(db.UserClients);
            db.Users.RemoveRange(db.Users);
            //if (db.Keys != null)
            db.Keys.RemoveRange(db.Keys);
            db.Values.RemoveRange(db.Values);

            db.SaveChanges();
        }

        public void DeleteDB()
        {
            db.Database.Delete();
        }

        public int GetHashForFullKey(string fullKey)
        {
            return fullKey.GetHashCode();
        }

        public void FindKeyForHash(int hashCode)
        {
            Cache cache = Cache.Instance;
            cache.Initialize(db.Keys, db.Values);

            cache.FindKeyForHash(hashCode);
        }

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
