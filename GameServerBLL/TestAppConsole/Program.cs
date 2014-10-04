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
            GameServerBLL.EntityData ed = new EntityData();
            ed.AddTestData();

            List<string> usersEmail = ed.ShowUsers();

            Console.WriteLine("All users in the database:");
            foreach (var item in usersEmail)
            {
                Console.WriteLine(item);
            }

            ed.Dispose();
        }
    }
}
