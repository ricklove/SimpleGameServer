using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerBLL;

////////////////////////////////////////////////

// Developer : Faqir Aamir
// Email     : faamirpk@yahoo.com
// skype     : faqir.aamir

////////////////////////////////////////////////

namespace TestAppConsole
{
    public class Tester
    {
        static void Main(string[] args)
        {
            EntityModeler em = new EntityModeler();
            em.AddTestData();

            bool testsResultRegisterLogin = Run_ReisterLoginTests();

            if (testsResultRegisterLogin)
                Console.WriteLine("All tests for Register Login passed \n");
            else
                Console.WriteLine("Some or all tests for Register Login failed \n");

            List<string> usersEmail = em.ShowUsers();

            Console.WriteLine("All users emails in the database:");
            foreach (var item in usersEmail)
            {
                Console.WriteLine(item);
            }

            SimpleGameServerProvider provider = SimpleGameServerProvider.Instance;
            provider.Register("faqiraamir@gamil.com", "pass1234");

            bool IsSuccess;
            Guid userSessionToken = Guid.Empty;
            Guid userClientToken = provider.Login("faamirpk@yahoo.com", "pass1", out IsSuccess);

            //if (IsSuccess)
            if (userClientToken != Guid.Empty)
            {
                userSessionToken = provider.CreateSession(userClientToken);
            }
            else
            {
                Console.WriteLine("Invalid user name or password");
            }

            if (userSessionToken != Guid.Empty)
            {
                // test key-value storage
                em.AddTestDataKeyValue();
                bool testsResult = Run_KeyValueTests();

                if (testsResult)
                    Console.WriteLine("\n All tests for key-value storage passed");
                else
                    Console.WriteLine("\n Some or all tests for key-value storage failed");

                // add key value through provider // "TOLD.Players.4.PlayeID";
                string key = "TOLD.Players.4.PlayerID";
                string value = "456";
                provider.SetValue(userSessionToken, GameServerBLL.KeyValueScope.Shared, key, value);
                string out_value = provider.GetValue(userSessionToken, KeyValueScope.Shared, key);

                if (value.Equals(out_value))
                {
                    Console.WriteLine("Retrieval successful");
                    Console.WriteLine("key : ( {0} ) ----- Value : {1}", key, value);
                }
                else
                    Console.WriteLine("Retrieval Invalid");

                // Test to look Up using Full key
                //provider.BuildCache();

                //string fullKey = "TOLD.MyGame.HighScores.1.PlayerID";
                //int hashCodeFullKey = em.GetHashForFullKey(fullKey);

                //em.FindKeyForHash(hashCodeFullKey);



                Console.ReadLine();
            }

            provider.Dispose();
            em.Dispose();
        }

        static bool Run_KeyValueTests()
        {
            GameServerBLL.Tests.KeyValueTests test = new GameServerBLL.Tests.KeyValueTests();
            test.key1_ShoulReturnValue_value1();
            test.key2_ShoulReturnValue_value2();
            test.key3_ShoulReturnValue_value3();
            test.key4_ShoulReturnValue_value4();

            return test.AllTestPassed;
        }

        static bool Run_ReisterLoginTests()
        {
            GameServerBLL.Tests.RegisterTests test = new GameServerBLL.Tests.RegisterTests();
            test.Register_IvalidEmail_Fails();
            test.Register_NoPassword_Fails();
            test.Register_ExistingEmail_Fails();
            test.Register_ValidnewEmail_AndPassword_Succeeds();

            return test.AllTestPassed;
        }
    }
}
