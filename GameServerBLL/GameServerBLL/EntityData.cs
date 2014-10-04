using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerDAL;
using GameServerDAL.Entities;

namespace GameServerBLL
{
    public class EntityData : IDisposable
    {
        bool is_disposed = false;
        private GameServerContext db;

        public EntityData()
        {
            db = new GameServerContext();
        }

        public void AddTestData()
        {
            var user1 = new User { UserID = 1, Email = "faamirpk@yahoo.com", EncodedPassword = "pass1", IsVerified = true };
            var user2 = new User { UserID = 2, Email = "test2222@yahoo.com", EncodedPassword = "pass2", IsVerified = true };
            var user3 = new User { UserID = 3, Email = "test3333@yahoo.com", EncodedPassword = "pass3", IsVerified = true };
            var user4 = new User { UserID = 4, Email = "test4444@yahoo.com", EncodedPassword = "pass4", IsVerified = true };

            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Users.Add(user3);
            db.Users.Add(user4);

            var aGuid = Guid.NewGuid();

            var userClient1 = new UserClient { UserClientToken = aGuid, UserID = 1 };
            db.UserClients.Add(userClient1);

            var userSession1 = new UserSession { UserSessionToken = Guid.NewGuid(), UserClientToken = aGuid };
            db.UserSessions.Add(userSession1);


            db.SaveChanges();
        }

        public List<string> ShowUsers()
        {
            List<string> usersEmail = new List<string>();

            // Display all Users 
            var query = from u in db.Users
                        orderby u.Email
                        select u;

            foreach (var item in query)
            {
                usersEmail.Add(item.Email);
            }

            return usersEmail;
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

        ~EntityData()
        {
            Dispose(false);
        }

	}
}
