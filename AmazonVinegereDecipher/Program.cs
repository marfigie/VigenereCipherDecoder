using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace AmazonVigenereDecipher
{
    class Program
    {
        public static string Message = @"HUTFGU WWNG VY NTTVTQTH HJYART XYAP
WSGGI. VM'X SVDGIC GU - BECO HYD LGY
AQLGYUTXCPF, CATVE, QTAN, OOSOOWB, SRT
UYQI. LERU FLTGKT DSBZCUT TC TPSQ ST VM
NTOLEUC OOW SSQL OYQH. FSE GEU NGGQ
HDOWE PJ TIESSQ ST FSEQ AHLO MGYP. OGLP
AX SVNS TK IQNOQOGX UARLP XBIHLOQ OOGU
GEU QGRV HFI QGPU SOOO ACIIYYGPI NSEKH.
MDOSTL HGCCR OSM.EJGJUR.KSC/TXOS GL MPC
SSRR MY LCUP YAO JSBT.";

        private static char[] _alphabet =  {'A', 'B', 'M','D','E','F','S','H','I','J','T','L','C','N','O','V','X','R','G','K','U','P','W','Q','Y','Z'};
        private static bool _terminationFlag = false;
        private static string _terminationStringInfo;

        private static BackgroundWorker Worker1 = new BackgroundWorker();
        private static BackgroundWorker Worker2 = new BackgroundWorker();
        private static BackgroundWorker Worker3 = new BackgroundWorker();
        private static BackgroundWorker Worker4 = new BackgroundWorker();

        private static int[] _progress = new int[4]; //Holds each worker's progress (as a percentage)

        private static System.Timers.Timer _messageTimer;
        
        static void Main(string[] args)
        {
            //Generating Vigenere square from given alphabet
            VigenereSquare.CreateSquare(_alphabet);

            // Dividing given interval into four sections
            KeyFinder keyFinderOne = new KeyFinder(0, 2970344);
            KeyFinder keyFinderTwo = new KeyFinder(2970344, 2 * 2970344);
            KeyFinder keyFinderThree = new KeyFinder(2 * 2970344, 3 * 2970344);
            KeyFinder keyFinderFour = new KeyFinder(3 * 2970344, 4 * 2970344);

            InitializeBackgroundWorkers();
            Worker1.RunWorkerAsync(keyFinderOne);
            Worker2.RunWorkerAsync(keyFinderTwo);
            Worker3.RunWorkerAsync(keyFinderThree);
            Worker4.RunWorkerAsync(keyFinderFour);

            Console.WriteLine("Working..");

            _messageTimer = new System.Timers.Timer(2000);
            _messageTimer.Elapsed += _messageTimer_Elapsed;
            _messageTimer.Enabled = true;

            while (Worker1.IsBusy) {}
            Console.ReadKey();            
        }

        static void _messageTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Working..");
            Console.WriteLine("Progres on Worker1: {0} %", _progress[0]);
            Console.WriteLine("Progres on Worker2: {0} %", _progress[1]);
            Console.WriteLine("Progres on Worker3: {0} %", _progress[2]);
            Console.WriteLine("Progres on Worker4: {0} %", _progress[3]);

            if (_terminationFlag) PrintTerminationDialog();
        }

        private static void InitializeBackgroundWorkers()
        {
            Worker1.DoWork += new DoWorkEventHandler(Worker1_DoWork);
            Worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker1_RunWorkerCompleted);
            Worker1.ProgressChanged += new ProgressChangedEventHandler(Worker1_ProgressChanged);
            Worker1.WorkerSupportsCancellation = true;
            Worker1.WorkerReportsProgress = true;

            Worker2.DoWork += new DoWorkEventHandler(Worker2_DoWork);
            Worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker2_RunWorkerCompleted);
            Worker2.ProgressChanged += new ProgressChangedEventHandler(Worker2_ProgressChanged);
            Worker2.WorkerSupportsCancellation = true;
            Worker2.WorkerReportsProgress = true;

            Worker3.DoWork += new DoWorkEventHandler(Worker3_DoWork);
            Worker3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker3_RunWorkerCompleted);
            Worker3.ProgressChanged += new ProgressChangedEventHandler(Worker3_ProgressChanged);
            Worker3.WorkerSupportsCancellation = true;
            Worker3.WorkerReportsProgress = true;

            Worker4.DoWork += new DoWorkEventHandler(Worker4_DoWork);
            Worker4.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker4_RunWorkerCompleted);
            Worker4.ProgressChanged += new ProgressChangedEventHandler(Worker4_ProgressChanged);
            Worker4.WorkerSupportsCancellation = true;
            Worker4.WorkerReportsProgress = true;
        }

        /*
         * Background Worker's event handling section
         * 
         */

        private static void Worker4_DoWork(object sender, DoWorkEventArgs e)
        {
 	        KeyFinder keyFinder = (KeyFinder)e.Argument;
            e.Result = keyFinder.FindKey(Worker4);
        }

        private static void Worker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
 	        _progress[3] = e.ProgressPercentage;
        }

        private static void Worker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
 	         string result = (string)e.Result;
            _terminationFlag = true;
            _terminationStringInfo = result;
        }   

        private static void Worker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progress[2] = e.ProgressPercentage;
        }

        private static void Worker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = (string)e.Result;
            _terminationFlag = true;
            _terminationStringInfo = result;
        }

        private static void Worker3_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyFinder keyFinder = (KeyFinder)e.Argument;
            e.Result = keyFinder.FindKey(Worker3);
        }
       
        private static void Worker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progress[1] = e.ProgressPercentage;
        }

        private static void Worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = (string)e.Result;
            _terminationFlag = true;
            _terminationStringInfo = result;
        }

        private static void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyFinder keyFinder = (KeyFinder)e.Argument;
            e.Result = keyFinder.FindKey(Worker2);
        }

        private static void Worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progress[0] = e.ProgressPercentage;
        }

        private static void Worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = (string)e.Result;
            _terminationFlag = true;
            _terminationStringInfo = result;
        }

        private static void Worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyFinder keyFinder = (KeyFinder)e.Argument;
            e.Result = keyFinder.FindKey(Worker1);
        }

        private static void PrintTerminationDialog()
        {
            Console.WriteLine(_terminationStringInfo);
            Console.WriteLine("\n\nOne of the workers found solution. Do you want to termiante other background workers? If yes, press [Y] if no, pres any other key");
            var userInput = Console.ReadKey();
            KeyFinder.WorkersSleepFlag = true;

            if (userInput.KeyChar == 'y')
            {
                Worker1.CancelAsync();
                Worker2.CancelAsync();
                Worker3.CancelAsync();
                Worker4.CancelAsync();
            }
            else
            {
                _terminationFlag = false;
                KeyFinder.WorkersSleepFlag = false;
            }

        }
    }
}
