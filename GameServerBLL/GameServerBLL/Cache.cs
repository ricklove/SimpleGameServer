using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GameServerDAL.Entities;
using System.Linq;

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
            keyHashLookup = new Dictionary<int, List<Key>>();
        }

        public static Cache Instance
        {
            get
            {
                return instance;
            }
        }

        bool is_Initialized;
        DbSet<Key> Keys;
        DbSet<Value> Values;

        private Dictionary<int, Key> keyIdLookup;
        private Dictionary<int, List<Key>> keyHashLookup;
        private string strFullKey;

        public void Initialize(DbSet<Key> p_Keys, DbSet<Value> p_Values)
        {
            if (!is_Initialized)
            {
                is_Initialized = true;

                this.Keys = p_Keys;
                this.Values = p_Values;

                FillCache(); 
            }
        }

        public void FindKeyForHash(int hashCode)
        {
            var matches = keyHashLookup[hashCode];
        }

        private void FillCache()
        {
            List<string> fullKey;

            foreach (Key keyData in Keys)
            {
                fullKey = new List<string>();
                string sFullKey = GetFullkey(keyData, fullKey);
                List<Key> keyAncestry = new List<Key>();
                GetKeyAncestry(keyData, keyAncestry);

                int hashKey = sFullKey.GetHashCode();
                keyHashLookup.Add(hashKey, keyAncestry);
            }
        }

        private void GetKeyAncestry(Key keyData, List<Key> keyList)
        {
            keyList.Add(keyData);

            if (keyData.ParentID == null)
                keyList.Reverse();
            else
                GetKeyAncestry(Keys.Find(keyData.ParentID), keyList);

        }

        private string GetFullkey(Key keyData, List<string> fullKey)
        {
            fullKey.Add(keyData.Name);

            if (keyData.ParentID == null)
            {
                fullKey.Reverse();
                strFullKey = String.Join(".", fullKey);
                return strFullKey;
            }
            else
                GetFullkey(Keys.Find(keyData.ParentID), fullKey);

            return strFullKey;
        }

    }
}
