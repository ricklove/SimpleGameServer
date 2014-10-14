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


        // Key-valueRow_Value Storage
        public void SetValue(Guid sessionToken, KeyValueScope scope, string key, string value)
        {
            string[] arrKey = key.Split(',');
       
            if (arrKey[3].Trim() == "PlayerName")
                SetValue_PlayerName(sessionToken, scope, key, value);

            if (arrKey[3].Trim() == "Score") ;
                //SetValue_Score(sessionToken, scope, key, value);

            db.SaveChanges();
        }

        public string GetValue(Guid sessionToken, KeyValueScope scope, string key)
        {
            string[] arrKey = key.Split(',');
            int p_key = Convert.ToInt32(arrKey[0].Trim());
            Value value = db.Values.Where( val => val.KeyID == p_key).SingleOrDefault();

            return value.Val;
        }

        #endregion

        private IQueryable<string> AllUsersEmail()
        {
            var query = from u in db.Users
                        select u.Email;

            return query;
        }

        // add a new Player Name to the Key table and its respective value to Value table
        private void SetValue_PlayerName(Guid sessionToken, KeyValueScope scope, string key, string value)
        {
            string[] arrKey = key.Split(',');

            // valueRow_KeyID for Players row
            // asuming players row is always there (if assumption wrong, playres row can be added here if not present)
            var query = db.Keys.Where(keyRow => keyRow.Name == "Players").Select(keyRow => new { KeyID = keyRow.KeyID, Depth = keyRow.Depth }).SingleOrDefault();
            int playersRow_KeyID = query.KeyID;
            int playersRow_Depth = query.Depth;

            // leaf node for player
            int leafPlayer_KeyID = db.Keys.Where(keyRow => keyRow.ParentID == playersRow_KeyID).Select(x => x.KeyID).Max();

            // insert player's id row
            int playerIDRow_KeyID = leafPlayer_KeyID + 1;
            int playerIDRow_ParentID = playersRow_KeyID;
            int playerIDRow_Depth = playersRow_Depth + 1;
            string playerIDRow_Name = "456"; // hard code "456" not sure how calculate this id
            var playerIdRow = new Key { KeyID = playerIDRow_KeyID, ParentID = playerIDRow_ParentID, Depth = playerIDRow_Depth, Name = playerIDRow_Name };
            db.Keys.Add(playerIdRow);

            //insert player's name row
            int playerNameRow_KeyID = Convert.ToInt32(playerIDRow_KeyID.ToString() + "0");
            int playerNameRow_ParentID = playerIDRow_KeyID;
            int playerNameRow_Depth = playerIDRow_Depth + 1;
            string playerNameRow_Name = arrKey[3].Trim();

            var playerNameRow = new Key { KeyID = playerNameRow_KeyID, ParentID = playerNameRow_ParentID, Depth = playerNameRow_Depth, Name = playerNameRow_Name };
            db.Keys.Add(playerNameRow);

            // insert value for above key
            string[] arrValue = value.Split(',');

            // ValueID = 0; // identity_row
            int valueRow_KeyID = playerNameRow_KeyID;
            GameServerDAL.Entities.KeyValueScope valueRow_Scope = (GameServerDAL.Entities.KeyValueScope)Convert.ToInt32(arrValue[2].Trim());
            string valueRow_Value = arrValue[3].Trim();
            int valueRow_SetByUserID = Convert.ToInt32(arrValue[4].Trim());
            DateTime valueRow_SetAtTime = DateTime.Now;

            var valueRow = new Value { /*ValueID is identity_val*/ KeyID = valueRow_KeyID, Scope = valueRow_Scope, Val = valueRow_Value, SetByUserID = valueRow_SetByUserID, SetAtTime = valueRow_SetAtTime };
            db.Values.Add(valueRow);
        }
    }
}