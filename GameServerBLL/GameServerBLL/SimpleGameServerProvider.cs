using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerBLL
{
    // This class act as the main access point for the business logic
    public sealed class SimpleGameServerProvider : ISimpleGameServer
    {
        private SimpleGameServerProvider()
        {
        }

        public static SimpleGameServerProvider Instance { get { return Providers.instance; } }

        private static class Providers
        {
            static Providers()
            {
            }

            // It is a static property point to the static instance of the server provider
            internal static readonly SimpleGameServerProvider instance = new SimpleGameServerProvider();
        }

        // ISimpleGameServer Interface Methods

        // Login Methods
        public void Register(string email, string password) 
        { 
        }

        public void Verify(string email)
        {
        }


        public Guid Login(string email, string password) // Returns ClientToken
        {
            return new Guid();
        }

        public Guid CreateSession(Guid ClientToken) // Returns sessionToken
        {
            return new Guid();
        }
    }
}
