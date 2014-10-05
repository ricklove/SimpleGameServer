using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameServerBLL.Tests
{
    class RegisterTests
    {
        ISimpleGameServer provider = SimpleGameServerProvider.Instance;

        public void Register_IvalidEmail_Fails()
        {
            string invalidEmail = "fa@ab@_solution";
            Assert.IsFalse(provider.Register(invalidEmail, "pass"));
        }

        public void Register_NoPassword_Fails()
        {
            string validNewEmail = "validNewEmail@yahoo.com";
            Assert.IsFalse(provider.Register(validNewEmail, String.Empty));
        }

        public void Register_ExistingEmail_Fails()
        {
            string newEmail = "faamirpk@yahoo.com";
            Assert.IsFalse(provider.Register(newEmail, "pass"));
        }

        public void Register_ValidnewEmail_AndPassword_Succeeds()
        {
            string newEmail = "faamirpk@yahoo.com";
            string password = "password";
            Assert.IsTrue(provider.Register(newEmail, password));
        }
    }
}
