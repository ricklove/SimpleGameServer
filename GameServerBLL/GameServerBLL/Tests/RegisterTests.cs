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

        public void Register_withIvalidEmail_Fails()
        {
            string invalidEmail = "fa@ab@_solution";
            Assert.IsFalse(provider.Register(invalidEmail, "pass"));
        }
    }
}
