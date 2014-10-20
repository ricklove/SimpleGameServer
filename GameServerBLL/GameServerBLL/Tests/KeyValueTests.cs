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
        public KeyValueTests(Guid userSessionToken)
        {
            AllTestPassed = false;
            this.userSessionToken = userSessionToken;
            provider = SimpleGameServerProvider.Instance;
        }

        Guid userSessionToken;
        SimpleGameServerProvider provider;
        public bool AllTestPassed { get; set; }

        const string key1 = "TOLD.MyGame.HighScores.1.PlayerID"; // -> "123"
        const string key2 = "TOLD.MyGame.HighScores.2.PlayerID"; // -> "234"
        const string key3 = "TOLD.Players.1.PlayerName"; // -> "Matthew"
        const string key4 = "TOLD.Players.2.PlayerName"; // -> "Mark"

        const string value1 = "123";
        const string value2 = "234";
        const string value3 = "Matthew";
        const string value4 = "Mark";

        const string nonExistingKey = "TOLD.MyGame.Players.2.PlayerName";
        const string keyForExistingValue = "TOLD.MyGame.HighScores.1.Score"; // (original value 9500)  (new value 9700)
        const string newKey =  "TOLD.Levles.1.LevelName"; // -> "Beginner"

        public void key1_ShouldReturnValue_value1()
        {
            string value = provider.GetValue(userSessionToken, KeyValueScope.Shared, key1);
            AllTestPassed = value.Equals(value1);
        }

        public void key2_ShouldReturnValue_value2()
        {
            string value = provider.GetValue(userSessionToken, KeyValueScope.Shared, key2);
            AllTestPassed = value.Equals(value2);
        }


        public void key3_ShouldReturnValue_value3()
        {
            string value = provider.GetValue(userSessionToken, KeyValueScope.Shared, key3);
            AllTestPassed = value.Equals(value3);
        }


        public void key4_ShouldReturnValue_value4()
        {
            string value = provider.GetValue(userSessionToken, KeyValueScope.Shared, key4);
            AllTestPassed = value.Equals(value4);
        }

        // Call GetValue for a nonexistent key should return ""
        public void nonExistingKey_ShouldReturnValue_EmptyString()
        {
            string value = provider.GetValue(userSessionToken, KeyValueScope.Shared, nonExistingKey);
            AllTestPassed = value.Equals(String.Empty);
        }

        // Call SetValue should change the existing value (verify with GetValue)
        // (original value 9500)  (new value 9700)
        public void existingValueSet_ShouldChangeValue_newValue()
        {
            string originalValue = provider.GetValue(userSessionToken, KeyValueScope.Shared, keyForExistingValue);
            string newValue = "9700";
            provider.SetValue(userSessionToken, KeyValueScope.Shared, keyForExistingValue, newValue);
            string out_newValue = provider.GetValue(userSessionToken, KeyValueScope.Shared, keyForExistingValue);
            AllTestPassed = newValue.Equals(out_newValue);

            //get back to original value after the test
            provider.SetValue(userSessionToken, KeyValueScope.Shared, keyForExistingValue, originalValue);
        }

        // Call SetValue for a new key should create the key and value (verify with GetValue)
        public void newKey_ShouldCreateKeyAndValue()
        {
            string value = "Beginner";
            provider.SetValue(userSessionToken, GameServerBLL.KeyValueScope.Shared, newKey, value);
            string out_Value = provider.GetValue(userSessionToken, KeyValueScope.Shared, newKey);

            AllTestPassed = value.Equals(out_Value);
        }
    }
}
