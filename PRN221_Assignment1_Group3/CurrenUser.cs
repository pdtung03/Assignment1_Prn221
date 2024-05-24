using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221_Assignment1_Group3
{
    public static class CurrentUser
    {
        public static string Username { get; private set; }
        public static bool IsAdmin { get; private set; }

        public static void Login(string username, bool isAdmin)
        {
            Username = username;
            IsAdmin = isAdmin;
        }

        public static void Logout()
        {
            Username = null;
            IsAdmin = false;
        }
    }
}
