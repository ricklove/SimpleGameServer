using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace GameServerBLL.Tests
{
    public class RegisterTests
    {
        public RegisterTests()
        {
            AllTestPassed = false;
        }

        public bool AllTestPassed { get; set; }

        ISimpleGameServer provider = SimpleGameServerProvider.Instance;

        public void Register_IvalidEmail_Fails()
        {
            string invalidEmail = "fa@ab@_solution";

            if (false == provider.Register(invalidEmail, "pass"))
                AllTestPassed = true;
            else
                AllTestPassed = false;
        }

        public void Register_NoPassword_Fails()
        {
            string validNewEmail = "validNewEmail@yahoo.com";

            if (false == provider.Register(validNewEmail, String.Empty))
                AllTestPassed = true;
            else
                AllTestPassed = false;
        }

        public void Register_ExistingEmail_Fails()
        {
            string newEmail = "faamirpk@yahoo.com";

            if (false == provider.Register(newEmail, "pass"))
                AllTestPassed = true;
            else
                AllTestPassed = false;
        }

        public void Register_ValidnewEmail_AndPassword_Succeeds()
        {
            string newEmail = "validNewEmail@yahoo.com";
            string password = "password";

            if (true == provider.Register(newEmail, password))
                AllTestPassed = true;
            else
                AllTestPassed = false;
        }
    }
}
