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

        public void Get_MyGame_Players_123_PlayerName_Matthew_Succeeds()
        {
            string key = "1100, 110, 3, PlayerName";
            string value = "Matthew";

            string playerName = provider.GetValue(Guid.Empty, KeyValueScope.Shared, key);
            Assert.IsTrue(value.Equals(playerName));
        }
    }
}
