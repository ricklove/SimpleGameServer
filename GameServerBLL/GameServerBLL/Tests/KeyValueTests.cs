using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameServerBLL.Tests
{
    class KeyValueTests
    {
        ISimpleGameServer provider = SimpleGameServerProvider.Instance;

        public void key1_ShoulReturnValue_123()
        {
            string key = "1100, 110, 3, PlayerName";
            string value = "Matthew";

            string playerName = provider.GetValue(Guid.Empty, KeyValueScope.Shared, key);
            Assert.IsTrue(value.Equals(playerName));
        }
    }
}
