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
            SimpleGameServerProvider provider = SimpleGameServerProvider.Instance;

            EntityModeler em = new EntityModeler();
            em.AddTestData();

            bool testsResultRegisterLogin = Run_ReisterLoginTests();

            if (testsResultRegisterLogin)
                Console.WriteLine("All tests for Register Login passed \n");
            else
                Console.WriteLine("Some or all tests for Register Login failed \n");

            List<string> usersEmail = provider.ShowUsers();
            Console.WriteLine("All users emails in the database:");
            foreach (var item in usersEmail)
            {
                Console.WriteLine(item);
            }

            // register a user
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
                bool testsResult = Run_KeyValueTests(userSessionToken);

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

                Console.ReadLine();
            }

            provider.Dispose();
            em.Dispose();
        }

        static bool Run_KeyValueTests(Guid userSessionToken)
        {
            GameServerBLL.Tests.KeyValueTests tests = new GameServerBLL.Tests.KeyValueTests(userSessionToken);

            tests.key1_ShouldReturnValue_value1();
            tests.key2_ShouldReturnValue_value2();
            tests.key3_ShouldReturnValue_value3();
            tests.key4_ShouldReturnValue_value4();
            tests.nonExistingKey_ShouldReturnValue_EmptyString();
            tests.existingValueSet_ShouldChangeValue_newValue();
            tests.newKey_ShouldCreateKeyAndValue();

            return tests.AllTestPassed;
        }

        static bool Run_ReisterLoginTests()
        {
            GameServerBLL.Tests.RegisterTests tests = new GameServerBLL.Tests.RegisterTests();

            tests.Register_IvalidEmail_Fails();
            tests.Register_NoPassword_Fails();
            tests.Register_ExistingEmail_Fails();
            tests.Register_ValidnewEmail_AndPassword_Succeeds();

            return tests.AllTestPassed;
        }
    }
}
