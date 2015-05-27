using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmazonVigenereDecipher
{
    class KeyFinder
    {
        private int _startValue;
        private int _endValue;     
        private char[] _key = new char[5];

        public static bool WorkersSleepFlag { get; set; }

        public KeyFinder(int startValue, int endValue)
        {
            this._startValue = startValue;
            this._endValue = endValue;
        }

        private static string GenerateKey(char[] givenKey)
        {
            int counter = 0;
            StringBuilder longKey = new StringBuilder();

            foreach (char character in Program.Message)
            {
                if (((int)character) < 65 || ((int)character) > 90)
                {
                    longKey.Append(character);
                }
                else
                {
                    longKey.Append(givenKey[counter]);

                    counter++;
                }
                if (counter == 5) counter = 0;
            }
            return longKey.ToString();
        }

        public string FindKey(BackgroundWorker bw)
        {

            string decodedMsg;
            Stopwatch sw = new Stopwatch();
            
            sw.Start();
           
            for (int i = _startValue; i < _endValue; i++)
            {
                while (WorkersSleepFlag) {  } 

                int _modLevel1 = i % 26;
                int _modLevel2 = ((i % 676) - _modLevel1) / 26;
                int _modLevel3 = ((i % (17576)) - _modLevel2) / (676);
                int _modLevel4 = ((i % (456976)) - _modLevel3) / (17576);
                int _modLevel5 = ((i % (11881376)) - _modLevel4) / (456976);

                this._key[0] = (char)(65 + _modLevel5);
                this._key[1] = (char)(65 + _modLevel4);
                this._key[2] = (char)(65 + _modLevel3);
                this._key[3] = (char)(65 + _modLevel2);
                this._key[4] = (char)(65 + _modLevel1);

                decodedMsg = Decoder.Decode(KeyFinder.GenerateKey(_key));

                double progress = ((i - _startValue) / ((double)(_endValue - _startValue))) * 100;
                bw.ReportProgress((int)progress);

                if ((decodedMsg.Substring(328, 14).Equals("WWW.AMAZON.COM")))
                {
                    sw.Stop(); 
                    return "Solution found!! \n\nEncryption key: " + _key[0] + _key[1] + _key[2] + _key[3] + _key[4] + "\n\n" + decodedMsg;
                }              
            }
            return "No solution found for given interval {" + _startValue + ", " + _endValue + "}";
        }
    }
}
