using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerBLL;

namespace TestAppConsole
{
    public class Tester
    {
        static void Main(string[] args)
        {
            EntityData ed = new EntityData();
            ed.DeleteDB();
            ed.AddTestData();

            List<string> usersEmail = ed.ShowUsers();

            Console.WriteLine("All users emails in the database:");
            foreach (var item in usersEmail)
            {
                Console.WriteLine(item);
            }


            SimpleGameServerProvider provider = SimpleGameServerProvider.Instance;
            provider.Register("faqiraamir@gamil.com", "pass1234");

            bool IsSuccess;
            provider.Login("faamirpk@yahoo.com", "pass1", out IsSuccess);

            ed.DeleteDB();
            ed.Dispose();
        }
    }
}
