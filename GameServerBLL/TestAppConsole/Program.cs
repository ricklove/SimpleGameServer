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
            em.AddTestData();

            List<string> usersEmail = em.ShowUsers();

            Console.WriteLine("All users emails in the database:");
            foreach (var item in usersEmail)
            {
                Console.WriteLine(item);
            }


            SimpleGameServerProvider provider = SimpleGameServerProvider.Instance;
            provider.Register("faqiraamir@gamil.com", "pass1234");

            bool IsSuccess;
            provider.Login("faamirpk@yahoo.com", "pass1", out IsSuccess);

            //em.DeleteDB();
            em.Dispose();
        }
    }
}
