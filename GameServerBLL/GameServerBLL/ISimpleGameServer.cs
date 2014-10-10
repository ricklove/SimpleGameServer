using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerBLL
{
    interface ISimpleGameServer
    {
        // Login Methods
        bool Register(string email, string password);
        bool Verify(string email);
        Guid Login(string email, string password, out bool isSuccess); // Returns ClientToken
        Guid CreateSession(Guid ClientToken); // Returns sessionToken
        void SetValue(Guid sessionToken, KeyValueScope scope, string key, string value);
        string GetValue(Guid sessionToken, KeyValueScope scope, string key);
    }

    public enum KeyValueScope
    {
        User,
        Shared
    }
}
