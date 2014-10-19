using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerDAL;
using GameServerDAL.Entities;

using DALScope = GameServerDAL.KeyValueScope;

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
            Value ValRow = GetValueRow(keyPartIDs);

            if (ValRow == null)
            {
                int userID = GetUserIdForSession(sessionToken);
                Value valueRow = new Value
                {
                    Val = value,
                    SetByUserID = userID,
                    SetAtTime = DateTime.Now,
                    Scope = (DALScope)scope,
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

                db.Values.Add(valueRow);
                db.SaveChanges();
            }
            else
            {
                ValRow.Val = value;
                db.SaveChanges();
            }
        }

        public string GetValue(Guid sessionToken, KeyValueScope scope, string key)
        {
            int?[] keyPartIDs = GetKeyPartIDs(key);

            Value valRow = GetValueRow(keyPartIDs);

            if (null != valRow)
                return valRow.Val;
            else
                return String.Empty;
  
        }

        private Value GetValueRow(int?[] keyPartIDs)
        {
            int? keyPartID_0 = keyPartIDs[0];
            int? keyPartID_1 = keyPartIDs[1];
            int? keyPartID_2 = keyPartIDs[2];
            int? keyPartID_3 = keyPartIDs[3];
            int? keyPartID_4 = keyPartIDs[4];
            int? keyPartID_5 = keyPartIDs[5];
            int? keyPartID_6 = keyPartIDs[6];
            int? keyPartID_7 = keyPartIDs[7];
            int? keyPartID_8 = keyPartIDs[8];
            int? keyPartID_9 = keyPartIDs[9];
            int? keyPartID_10 = keyPartIDs[10];

            int? keyPartID_11 = keyPartIDs[11];
            int? keyPartID_12 = keyPartIDs[12];
            int? keyPartID_13 = keyPartIDs[13];
            int? keyPartID_14 = keyPartIDs[14];
            int? keyPartID_15 = keyPartIDs[15];
            int? keyPartID_16 = keyPartIDs[16];
            int? keyPartID_17 = keyPartIDs[17];
            int? keyPartID_18 = keyPartIDs[18];
            int? keyPartID_19 = keyPartIDs[19];
            int? keyPartID_20 = keyPartIDs[20];

            int? keyPartID_21 = keyPartIDs[21];
            int? keyPartID_22 = keyPartIDs[22];
            int? keyPartID_23 = keyPartIDs[23];
            int? keyPartID_24 = keyPartIDs[24];
            int? keyPartID_25 = keyPartIDs[25];
            int? keyPartID_26 = keyPartIDs[26];
            int? keyPartID_27 = keyPartIDs[27];
            int? keyPartID_28 = keyPartIDs[28];
            int? keyPartID_29 = keyPartIDs[29];

            Value valRow = db.Values.Where(v => v.Key1ID == keyPartID_0
                                        && v.Key2ID == keyPartID_1
                                        && v.Key3ID == keyPartID_2
                                        && v.Key4ID == keyPartID_3
                                        && v.Key5ID == keyPartID_4
                                        && v.Key6ID == keyPartID_5
                                        && v.Key7ID == keyPartID_6
                                        && v.Key8ID == keyPartID_7
                                        && v.Key9ID == keyPartID_8
                                        && v.Key10ID == keyPartID_9
                                        && v.Key11ID == keyPartID_10
                                        && v.Key12ID == keyPartID_11
                                        && v.Key13ID == keyPartID_12
                                        && v.Key14ID == keyPartID_13
                                        && v.Key15ID == keyPartID_14
                                        && v.Key16ID == keyPartID_15
                                        && v.Key17ID == keyPartID_16
                                        && v.Key18ID == keyPartID_17
                                        && v.Key19ID == keyPartID_18
                                        && v.Key20ID == keyPartID_19
                                        && v.Key21ID == keyPartID_20
                                        && v.Key22ID == keyPartID_21
                                        && v.Key23ID == keyPartID_22
                                        && v.Key24ID == keyPartID_23
                                        && v.Key25ID == keyPartID_24
                                        && v.Key26ID == keyPartID_25
                                        && v.Key27ID == keyPartID_26
                                        && v.Key28ID == keyPartID_27
                                        && v.Key29ID == keyPartID_28
                                        && v.Key30ID == keyPartID_29
                                        ).FirstOrDefault();
            return valRow;
        }

        public string GetKeyString(Value value)
        {
            StringBuilder text = new StringBuilder();

            foreach (var id in value.KeyIDs)
            {
                text.Append(cache.LookupKeyID(id) + ".");
            }

            return text.ToString().TrimEnd('.');
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

        #region Dispose
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SimpleGameServerProvider()
        {
            Dispose(false);
        } 
        #endregion
    }
}