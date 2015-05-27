using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonVigenereDecipher
{
    static class VigenereSquare
    {
        public static char[,] Vinegeresquare = new char[26, 26];

        public static void CreateSquare(char[] alphabet)
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    int counterHelper = j + i;
                    if (counterHelper > 25)
                        counterHelper -= 26;
                    Vinegeresquare[i, j] = alphabet[counterHelper];

                    //Console.Write(_alphabet[counterHelper]);
                }

                //Console.WriteLine();
            }
        }    
    }
}
