using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerDAL;
using GameServerDAL.Entities;

namespace GameServerBLL
{
    // This class act as the main access point for the business logic
    public sealed class SimpleGameServerProvider : ISimpleGameServer
    {
        private GameServerContext db;

        private SimpleGameServerProvider()
        {
            db = GameServerContext.Instance;
        }
        
        public static SimpleGameServerProvider Instance 
        { 
            get 
            { 
                return Providers.instance; 
            } 
        }

        // Nested class
        private static class Providers
        {
            static Providers()
            {
            }

            // It is a static property that points to the static instance of the server provider
            internal static readonly SimpleGameServerProvider instance = new SimpleGameServerProvider();
        }


        // ISimpleGameServer Interface Methods

        // Login Methods
        public bool Register(string email, string password) 
        {
            Util util = new Util();
            if (!(util.IsValidEmail(email)))
                return false;

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                return false;


            IQueryable<string> emails = AllUsersEmail();
            if (emails.Contains(email))
                return false;

            var user = new User { UserID = 1, Email = email, EncodedPassword = password, IsVerified = true };
            db.Users.Add(user);
            db.SaveChanges();

            return true;
        }

        public bool Verify(string email)
        {
            return false;
        }

        public Guid Login(string email, string password, out bool isSuccess) // Returns ClientToken
        {
            User user = (from u in db.Users
                         where u.Email.Equals(email) //&&
                            //u.EncodedPassword.Equals(UserSecurity.GetPasswordHash(username, password))
                         select u).FirstOrDefault();

            if ( (user == null) || (!(Util.VerifyHash(password, "SHA512", user.EncodedPassword))) )
            {
                // Invalid user name or password
                isSuccess = false;
                return Guid.NewGuid();
            }
            else if (!(user.IsVerified))
            {
                // User not verified
                isSuccess = false;
                return Guid.NewGuid();
            }
            else
            {
                //success
                isSuccess = true;
                return Guid.NewGuid();
            }
        }

        public Guid CreateSession(Guid ClientToken) // Returns sessionToken
        {
            return new Guid();
        }

        private IQueryable<string> AllUsersEmail()
        {
            var query = from u in db.Users
                        select u.Email;

            return query;
        }
    }
}
