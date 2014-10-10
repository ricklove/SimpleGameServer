using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerDAL;
using GameServerDAL.Entities;

namespace GameServerBLL
{
    // This class acts as the main access point for the business logic
    public sealed class SimpleGameServerProvider : ISimpleGameServer
    {
        private GameServerContext db;
        private Encoder encoder;
        private Mailer mailer;

        private SimpleGameServerProvider()
        {
            db = GameServerContext.Instance;
            encoder = new Encoder();
            mailer = new Mailer();
        }
        
        public static SimpleGameServerProvider Instance 
        { 
            get 
            {
                return Providers.SimpleGameServer; 
            } 
        }

        // Nested class
        private static class Providers
        {
            static Providers()
            {
            }

            // It is a static property that points to the static instance of the server provider
            internal static readonly SimpleGameServerProvider SimpleGameServer = new SimpleGameServerProvider();
        }


        #region ISimpleGameServer Interface Methods

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

            var user = new User { UserID = 1, Email = email, EncodedPassword = encoder.EncodePassword(password, "SHA512", null), IsVerified = false };
            db.Users.Add(user);
            db.SaveChanges();

            mailer.SendVerificationEmail(email);

            return true;
        }

        public bool Verify(string email)
        {
            User user = (from u in db.Users
                         where u.Email.Equals(email) 
                         select u).SingleOrDefault();

            if (user != null)
            {
                user.IsVerified = true;
                return true;
            }
            else
                return false;
        }

        public Guid Login(string email, string password, out bool isSuccess)
        {
            User user = (from u in db.Users
                         where u.Email.Equals(email) //&&
                         //u.EncodedPassword.Equals(UserSecurity.GetPasswordHash(username, password))
                         select u).SingleOrDefault();

            if ((user == null) || (!(encoder.VerifyHash(password, "SHA512", user.EncodedPassword))))
            {
                // Invalid user name or password
                isSuccess = false;
                return Guid.Empty;
            }
            else if (!(user.IsVerified))
            {
                // User not verified
                isSuccess = false;
                return Guid.Empty;
            }
            else
            {
                //success
                Guid userClientToken = Guid.NewGuid();

                var userClient = new UserClient { UserClientToken = userClientToken, UserID = user.UserID };
                db.UserClients.Add(userClient);
                db.SaveChanges();

                isSuccess = true;
                return userClientToken;
            }
        }

        public Guid CreateSession(Guid ClientToken)
        {
            UserClient userClient = (from uc in db.UserClients
                                     where uc.UserClientToken == ClientToken
                                     select uc).SingleOrDefault();

            if (userClient == null)
                return Guid.Empty;

            //success
            Guid userSessionToken = Guid.NewGuid();

            var userSession = new UserSession { UserSessionToken = userSessionToken, UserClientToken = ClientToken, UserID = userClient.UserID };
            db.UserSessions.Add(userSession);
            db.SaveChanges();

            return userSessionToken;
        }

        public void SetValue(Guid sessionToken, KeyValueScope scope, string key, string value)
        {

        }

        public string GetValue(Guid sessionToken, KeyValueScope scope, string key)
        {
            return String.Empty;
        }

        #endregion

        private IQueryable<string> AllUsersEmail()
        {
            var query = from u in db.Users
                        select u.Email;

            return query;
        }
    }
}
