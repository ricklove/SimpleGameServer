using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GameServerDAL.Entities;

namespace GameServerBLL
{
    sealed class Cache
    {
        private static readonly Cache instance = new Cache();

        static Cache()
        {
        }

        private Cache()
        {
            is_Initialized = false;
            keyIdLookup = new Dictionary<string, int>();
            keyIdReverseLookup = new Dictionary<int,string>();

            EntityModeler em = new EntityModeler();
        }

        public static Cache Instance
        {
            get
            {
                return instance;
            }
        }

        bool is_Initialized;
        EntityModeler em;
        private Dictionary<string, int> keyIdLookup;
        private Dictionary<int, string> keyIdReverseLookup;
        private string strFullKey;

        public void Initialize(DbSet<Key> keys, DbSet<Value> values)
        {
            if (!is_Initialized)
            {
                is_Initialized = true;

                foreach (Key keyRow in keys)
                {
                    keyIdLookup.Add(keyRow.Name, keyRow.KeyID);
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
                // Add to DB key
                em.AddKeyRow(keyPart);
                // Get the new ID for the new key
                int keyID = em.GetIdForKeyPart(keyPart);
                // Add the mapping to the keyIDLookup and keyIDReverseLookup
                keyIdLookup.Add(keyPart, keyID);
            }

            return keyIdLookup[keyPart];
        }

        public void FindKeyForHash(int hashCode)
        {
            //var matches = keyHashLookup[hashCode];
        }

        private void GetKeyAncestry(Key keyData, List<Key> keyList)
        {
            //keyList.Add(keyData);

            //if (keyData.ParentID == null)
            //    keyList.Reverse();
            //else
            //    GetKeyAncestry(Keys.Find(keyData.ParentID), keyList);

        }

        private string GetFullkey(Key keyData, List<string> fullKey)
        {
            fullKey.Add(keyData.Name);

            //if (keyData.ParentID == null)
            if (6 == 9) //todo
            {
                fullKey.Reverse();
                strFullKey = String.Join(".", fullKey);
                return strFullKey;
            }
            else
            {
                //GetFullkey(Keys.Find(keyData.ParentID), fullKey); todo

            }
            return strFullKey;
        }

    }
}
