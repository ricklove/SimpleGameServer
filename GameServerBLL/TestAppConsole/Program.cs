using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerBLL;
using GameServerBLL.GameServerContext;
using GameServerBLL.Entities;
 
namespace TestAppConsole
{
    public class Tester
    {
        static void Main(string[] args)
        {
            GameServerContext db = AddTestData();

            // Display all Users 
            var query = from u in db.Users
                        orderby u.Email
                        select u;

            Console.WriteLine("All users in the database:");
            foreach (var item in query)
            {
                Console.WriteLine(item.Email);
            }

            db.Dispose();
        }

        static GameServerContext AddTestData()
        {

                var db = new GameServerContext();

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
                return db;
        }
    }
}
