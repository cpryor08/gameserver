using System;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static bool ValidString(string Name)
        {
            foreach (char ch in Name)
            {
                if (!
                    ((ch >= 48 && ch <= 57) ||
                    (ch >= 65 && ch <= 90) ||
                    (ch >= 97 && ch <= 122))
                    )
                    return false;
            }
            return true;
        }
    }
}
