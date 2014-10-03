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
        void Register(string email, string password);
        void Verify(string email);
        Guid Login(string email, string password); // Returns ClientToken
        Guid CreateSession(Guid ClientToken); // Returns sessionToken
    }
}
