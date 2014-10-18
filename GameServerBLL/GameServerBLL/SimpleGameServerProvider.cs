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
    // This class acts as the main access point for the business logic
    public sealed class SimpleGameServerProvider : ISimpleGameServer, IDisposable
    {
        bool is_disposed = false;
        private GameServerContext db;
        private Encoder encoder;
        private Mailer mailer;
        private Cache cache;

        private SimpleGameServerProvider()
        {
            db = GameServerContext.Instance;
            encoder = new Encoder();
            mailer = new Mailer();
            cache = Cache.Instance;
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


        // Key Value Storage
        public void SetValue(Guid sessionToken, KeyValueScope scope, string key, string value)
        {
            int?[] keyPartIDs = GetKeyPartIDs(key);

            int userID = GetUserIdForSession(sessionToken);
            Value valueRow = new Value { Val = value, SetByUserID = userID, SetAtTime = DateTime.Now, Scope = (DALScope)scope,
                                         Key1ID = keyPartIDs[0],
                                         Key2ID = keyPartIDs[1],
                                         Key3ID = keyPartIDs[2],
                                         Key4ID = keyPartIDs[3],
                                         Key5ID = keyPartIDs[4],
                                         Key6ID = keyPartIDs[5],
                                         Key7ID = keyPartIDs[6],
                                         Key8ID = keyPartIDs[7],
                                         Key9ID = keyPartIDs[8],
                                         Key10ID = keyPartIDs[9],
                                         Key11ID = keyPartIDs[10],
                                         Key12ID = keyPartIDs[11],
                                         Key13ID = keyPartIDs[12],
                                         Key14ID = keyPartIDs[13],
                                         Key15ID = keyPartIDs[14],
                                         Key16ID = keyPartIDs[15],
                                         Key17ID = keyPartIDs[16],
                                         Key18ID = keyPartIDs[17],
                                         Key19ID = keyPartIDs[18],
                                         Key20ID = keyPartIDs[19],
                                         Key21ID = keyPartIDs[20],
                                         Key22ID = keyPartIDs[21],
                                         Key23ID = keyPartIDs[22],
                                         Key24ID = keyPartIDs[23],
                                         Key25ID = keyPartIDs[24],
                                         Key26ID = keyPartIDs[25],
                                         Key27ID = keyPartIDs[26],
                                         Key28ID = keyPartIDs[27],
                                         Key29ID = keyPartIDs[28],
                                         Key30ID = keyPartIDs[29]
                                        };

            db.SaveChanges();
        }

        public string GetValue(Guid sessionToken, KeyValueScope scope, string key)
        {
            int?[] keyPartIDs = GetKeyPartIDs(key);

            string value = db.Values.Where(v => v.Key1ID == keyPartIDs[0]
                                                && v.Key2ID == keyPartIDs[1]
                                                && v.Key3ID == keyPartIDs[2]
                                                && v.Key4ID == keyPartIDs[3]
                                                && v.Key5ID == keyPartIDs[4]
                                                && v.Key6ID == keyPartIDs[5]
                                                && v.Key7ID == keyPartIDs[6]
                                                && v.Key8ID == keyPartIDs[7]
                                                && v.Key9ID == keyPartIDs[8]
                                                && v.Key10ID == keyPartIDs[9]
                                                && v.Key11ID == keyPartIDs[10]
                                                && v.Key12ID == keyPartIDs[11]
                                                && v.Key13ID == keyPartIDs[12]
                                                && v.Key14ID == keyPartIDs[13]
                                                && v.Key15ID == keyPartIDs[14]
                                                && v.Key16ID == keyPartIDs[15]
                                                && v.Key17ID == keyPartIDs[16]
                                                && v.Key18ID == keyPartIDs[17]
                                                && v.Key19ID == keyPartIDs[18]
                                                && v.Key20ID == keyPartIDs[19]
                                                && v.Key21ID == keyPartIDs[20]
                                                && v.Key22ID == keyPartIDs[21]
                                                && v.Key23ID == keyPartIDs[22]
                                                && v.Key24ID == keyPartIDs[23]
                                                && v.Key25ID == keyPartIDs[24]
                                                && v.Key26ID == keyPartIDs[25]
                                                && v.Key27ID == keyPartIDs[26]
                                                && v.Key28ID == keyPartIDs[27]
                                                && v.Key29ID == keyPartIDs[28]
                                                && v.Key30ID == keyPartIDs[29]
                                            ).FirstOrDefault().Val;

            return value;
        }

        #endregion

        private int?[] GetKeyPartIDs(string key)
        {
            string[] keyParts = key.Trim().Split('.');
            int?[] keyPartIDs = new int?[30];

            int index = 0;
            foreach (string keyPart in keyParts)
            {
                int id = cache.LookupKeyPart(keyPart);
                keyPartIDs[index++] = id;
            }

            return keyPartIDs;
        }

        public int GetUserIdForSession(Guid sessionToken)
        {
            return db.UserSessions.Where(s => s.UserSessionToken == sessionToken).SingleOrDefault().UserID;
        }

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
            var query = db.Keys.Where(keyRow => keyRow.Name == "Players").Select(keyRow => new { KeyID = keyRow.KeyID, Depth = 3/*keyRow.Depth  todo*/ }).SingleOrDefault();
            int playersRow_KeyID = query.KeyID;
            int playersRow_Depth = 1;//query.Depth; //TODO

            // leaf node for player
            int leafPlayer_KeyID = db.Keys.Where(keyRow => 3/*keyRow.ParentID  todo*/ == playersRow_KeyID).Select(x => x.KeyID).Max();

            // insert player's id row
            int playerIDRow_KeyID = leafPlayer_KeyID + 1;
            int playerIDRow_ParentID = playersRow_KeyID;
            int playerIDRow_Depth = playersRow_Depth + 1;
            string playerIDRow_Name = "456"; // hard code "456" not sure how calculate this id
            var playerIdRow = new Key { KeyID = playerIDRow_KeyID, Name = playerIDRow_Name }; // todo
            db.Keys.Add(playerIdRow);

            //insert player's name row
            int playerNameRow_KeyID = Convert.ToInt32(playerIDRow_KeyID.ToString() + "0");
            int playerNameRow_ParentID = playerIDRow_KeyID;
            int playerNameRow_Depth = playerIDRow_Depth + 1;
            string playerNameRow_Name = arrKey[3].Trim();

            var playerNameRow = new Key { KeyID = playerNameRow_KeyID, Name = playerNameRow_Name }; //todo
            db.Keys.Add(playerNameRow);

            // insert value for above key
            string[] arrValue = value.Split(',');

            // ValueID = 0; // identity_row
            int valueRow_KeyID = playerNameRow_KeyID;
            GameServerDAL.Entities.KeyValueScope valueRow_Scope = (GameServerDAL.Entities.KeyValueScope)Convert.ToInt32(arrValue[2].Trim());
            string valueRow_Value = arrValue[3].Trim();
            int valueRow_SetByUserID = Convert.ToInt32(arrValue[4].Trim());
            DateTime valueRow_SetAtTime = DateTime.Now;

            //var valueRow = new Value { /*ValueID is identity_val*/ KeyID = valueRow_KeyID, Scope = valueRow_Scope, Val = valueRow_Value, SetByUserID = valueRow_SetByUserID, SetAtTime = valueRow_SetAtTime }; todo
            var valueRow = new Value { /*ValueID is identity_val*/ Scope = valueRow_Scope, Val = valueRow_Value, SetByUserID = valueRow_SetByUserID, SetAtTime = valueRow_SetAtTime };

            db.Values.Add(valueRow);
        }

        //private void BuildCache()
        //{
        //    Cache cache = Cache.Instance;
        //    cache.Initialize(db.Keys, db.Values);
        //}
    
        private void Dispose(bool disposing)
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

        ~SimpleGameServerProvider()
        {
            Dispose(false);
        }
    }
}