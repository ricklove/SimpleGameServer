using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GameServerDAL;
using GameServerDAL.Entities;

namespace GameServerBLL
{
    sealed class Cache
    {
        bool is_Initialized;
        GameServerContext db;
        private Dictionary<string, int> keyIdLookup;
        private Dictionary<int, string> keyIdReverseLookup;

        private static readonly Cache instance = new Cache();

        static Cache()
        {
        }

        private Cache()
        {
            is_Initialized = false;
            db = GameServerContext.Instance;
            keyIdLookup = new Dictionary<string, int>();
            keyIdReverseLookup = new Dictionary<int,string>();
        }

        public static Cache Instance
        {
            get
            {
                return instance;
            }
        }

        // This method is only for adding test data from Entity Modeler class and then sync db and cache
        // It should be deleted along with field db if adding test data is no longer required.
        public void Initialize()
        {
            if (!is_Initialized)
            {
                is_Initialized = true;

                foreach (Key keyRow in db.Keys)
                {
                    keyIdLookup.Add(keyRow.Name, keyRow.KeyID);
                    keyIdReverseLookup.Add(keyRow.KeyID, keyRow.Name);
                }
            }
        }

        public int LookupKeyPart(string keyPart)
        {
            int val;

            if (int.TryParse(keyPart, out val))
            {
                return -val;
            }

            if (!keyIdLookup.ContainsKey(keyPart))
            {
                int keyID;
                AddKeyRow(keyPart, out keyID); // Add to DB key and Get the new ID for the new key

                // Add the mapping to the keyIDLookup and keyIDReverseLookup
                keyIdLookup.Add(keyPart, keyID);
                keyIdReverseLookup.Add(keyID, keyPart);
            }

            return keyIdLookup[keyPart];
        }

        public string LookupKeyID(int? keyID)
        {
            if (!keyID.HasValue)
            {
                return "";
            }

            if (keyID < 0)
            {
                return "" + -keyID.Value;
            }

            return keyIdReverseLookup[keyID.Value];
        }

        // add key row to Key table
        private void AddKeyRow(string name, out int KeyID)
        {
            var keyRow = new Key { Name = name }; // KeyID primary key is identity
            db.Keys.Add(keyRow);
            db.SaveChanges();

            KeyID = keyRow.KeyID;
        }
    }
}
