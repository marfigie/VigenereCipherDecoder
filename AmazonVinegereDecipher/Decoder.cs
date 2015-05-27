using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonVigenereDecipher
{
    class Decoder
    {
        public static string Decode(string longKey)
        {
            StringBuilder decodedMsg = new StringBuilder();

            for (int characternumber = 0; characternumber < longKey.Length; characternumber++)
            {
                if (((int)longKey[characternumber]) < 65 || ((int)longKey[characternumber]) > 90)
                {
                    decodedMsg.Append(longKey[characternumber]);
                }
                else
                {



                    for (int row = 0; row < 26; row++)
                    {
                        if (VigenereSquare.Vinegeresquare[row, 0].Equals(longKey[characternumber]))
                        {
                            for (int col = 0; col < 26; col++)
                            {
                                if (VigenereSquare.Vinegeresquare[row, col].Equals(Program.Message[characternumber]))
                                {
                                    decodedMsg.Append(VigenereSquare.Vinegeresquare[0, col]);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return decodedMsg.ToString();
        }
    }
}
