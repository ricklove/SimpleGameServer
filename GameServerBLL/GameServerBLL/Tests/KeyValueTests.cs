using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace GameServerBLL.Tests
{
    public class KeyValueTests
    {
        public KeyValueTests()
        {
            AllTestPassed = false;
        }

        public bool AllTestPassed { get; set; }

        const string key1 = "TOLD.MyGame.HighScores.1.PlayerID"; // -> "123"
        const string key2 = "TOLD.MyGame.HighScores.2.PlayerID"; // -> "234"
        const string key3 = "TOLD.Players.1.PlayerName"; // -> "Matthew"
        const string key4 = "TOLD.Players.2.PlayerName"; // -> "Mark"

        const string value1 = "123";
        const string value2 = "234";
        const string value3 = "Matthew";
        const string value4 = "Mark";

        ISimpleGameServer provider = SimpleGameServerProvider.Instance;

        public void key1_ShoulReturnValue_value1()
        {
            string value = provider.GetValue(Guid.Empty, KeyValueScope.Shared, key1);
            AllTestPassed = value.Equals(value1);
        }

        public void key2_ShoulReturnValue_value2()
        {
            string value = provider.GetValue(Guid.Empty, KeyValueScope.Shared, key2);
            AllTestPassed = value.Equals(value2);
        }


        public void key3_ShoulReturnValue_value3()
        {
            string value = provider.GetValue(Guid.Empty, KeyValueScope.Shared, key3);
            AllTestPassed = value.Equals(value3);
        }


        public void key4_ShoulReturnValue_value4()
        {
            string value = provider.GetValue(Guid.Empty, KeyValueScope.Shared, key4);
            AllTestPassed = value.Equals(value4);
        }
    }
}
