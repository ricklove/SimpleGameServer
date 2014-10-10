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
            //db.UserSessions.RemoveRange(db.UserSessions.Select(s=>s));
            db.UserSessions.RemoveRange(db.UserSessions);
            db.UserClients.RemoveRange(db.UserClients);
            db.Users.RemoveRange(db.Users);

            db.SaveChanges();
        }

        public void DeleteDB()
        {
            db.Database.Delete();
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

	}
}
