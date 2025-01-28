using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class ServerLogic
    {
        public static bool isItPalindrom(string input)
        {
            for (int i = 0; i < input.Length / 2; i++)
            {
                if (input[i] != input[input.Length - i-1])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
