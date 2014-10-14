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
            //em.DeleteDB();
            //em.SaveChanges();

            em.DelteAllData();
            em.AddTestData();
            em.AddTestDataKeyValue();

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
                provider.SetValue(userSessionToken, GameServerBLL.KeyValueScope.Shared, "xxx_KeyID, xxx_ParentID, xxx_Depth, PlayerName", "xxx_ValueID, xxx_KeyID, 1, Faqir, 4, xxx_SetAtTime");
                string key = "1130, 113, 3, PlayerName";
                string value = provider.GetValue(userSessionToken, KeyValueScope.Shared, key);

                Console.WriteLine("key : ( {0} ) ----- Value : {1}", key, value);

                // Test to look Up using Full key
                provider.BuildCache();

                string fullKey = "TOLD.MyGame.HighScores.1.PlayerID";
                int hashCodeFullKey = em.GetHashForFullKey(fullKey);

                em.FindKeyForHash(hashCodeFullKey);



                Console.ReadLine();
            }

            provider.Dispose();
            em.Dispose();
        }
    }
}
