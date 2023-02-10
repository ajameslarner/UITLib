using UITestingFramework.Models;
using UITestingFramework.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UITestingFramework.Core
{
    /// <summary>
    /// The Sequence Testing Engine Console.
    /// </summary>
    public static class UITestConsole
    {
        #region Internal Console Code
        internal const uint GENERIC_WRITE = 0x40000000;
        internal const uint GENERIC_READ = 0x80000000;
        internal const int SWP_NOSIZE = 0x0001;
        internal const int MF_BYCOMMAND = 0x00000000;
        internal const int SC_CLOSE = 0xF060;
        internal const int SC_MINIMIZE = 0xF020;
        internal const int SC_MAXIMIZE = 0xF030;
        internal const int SC_SIZE = 0xF000;//resize
        internal const int HWND_TOPMOST = -1;
        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_ERROR_HANDLE = -12;

        [DllImport("kernel32")]
        internal static extern bool AllocConsole();
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPTStr)] string filename, [MarshalAs(UnmanagedType.U4)] uint access, [MarshalAs(UnmanagedType.U4)] FileShare share, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes, IntPtr templateFile);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);
        [DllImport("user32.dll")]
        internal static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        internal static void DisableConsoleResize()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);//resize
            }
        }
        internal static void ConsoleRedirect()
        {
            AllocConsole();
            var hOut = GetStdHandle(STD_ERROR_HANDLE);
            var hRealOut = CreateFile("CONOUT$", GENERIC_READ | GENERIC_WRITE, FileShare.Write, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
            if (hRealOut != hOut)
            {
                SetStdHandle(STD_OUTPUT_HANDLE, hRealOut);
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding) { AutoFlush = true });
            }

            SetWindowPos(GetConsoleWindow(), new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, 0);
            DisableConsoleResize();
        }
        #endregion

        #region Fields
        private static ConcurrentDictionary<ITester, List<object>> _uiTestList;
        private static UITracer _tracer;
        private static int _totalPassedTests;
        private static int _totalFailedTests;
        #endregion

        #region Properties
        /// <summary>
        /// List of all the defined sequence tests written externally.
        /// </summary>
        public static ConcurrentDictionary<ITester, List<object>> UITestList { get => _uiTestList; set => _uiTestList = value; }
        /// <summary>
        /// The CommandInputListener (CIL) thread.
        /// </summary>
        public static Thread CIL { get; private set; }
        /// <summary>
        /// The Sequence Tracer class to facilitate Trace Recording.
        /// </summary>
        public static UITracer Tracer { get => _tracer; set => _tracer = value; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialise the Console for Sequence Testing.
        /// </summary>
        /// <param name="tests">List of all the defined sequence tests written externally.</param>
        public static void InitializeTestingConsole(ConcurrentDictionary<ITester, List<object>> tests)
        {
            UITestList = tests;
            CIL = new Thread(UITestingConsole)
            {
                IsBackground = true,
                Name = "[CIL]ATFramework"
            };
            CIL.Start();
        }
        /// <summary>
        /// Initialise the Console for Sequence Testing.
        /// </summary>
        /// <param name="tracer">The Sequence Tracer class object passed as <see cref="IMessageFilter"/></param>
        public static void InitializeTracingConsole(IMessageFilter tracer)
        {
            Tracer = (UITracer)tracer;
            CIL = new Thread(UITracingConsole)
            {
                IsBackground = true,
                Name = "[CIL]ATFramework"
            };
            CIL.SetApartmentState(ApartmentState.STA);
            CIL.Start();
        }
        #endregion

        #region Private Methods
        private static void CreateConsole(int width = 75, int height = 20)
        {
            ConsoleRedirect();

            Console.Title = "UI Testing";
            Console.WindowWidth = width;
            Console.WindowHeight = height;
            Console.BufferWidth = width;
        }
        
        private static void UITestingConsole()
        {
            CreateConsole();

            Console.ForegroundColor = ConsoleColor.Cyan;

            while (true)
            {
                PrintTable();
                Console.WriteLine("Press [P] to run tests in Parallel.");
                Console.WriteLine("Press [S] to run tests in Sequence.");
                var res = Console.ReadKey(true).Key;
                Console.Clear();

                
                if (res == ConsoleKey.P)
                {
                    var testThread = new Thread(() => RunTestsParallel())
                    {
                        IsBackground = true
                    };

                    testThread.Start();

                    do
                    {
                        PrintUpdateTable();

                    } while (testThread.IsAlive);

                    PrintResultsTable();

                    Console.WriteLine("All tests complete.");
                    Console.WriteLine("Enter Enter to re-run tests...");
                    Console.ReadLine();
                }
                else if (res == ConsoleKey.S)
                {
                    var testThread = new Thread(() => RunTests())
                    {
                        IsBackground = true
                    };

                    testThread.Start();

                    do
                    {
                        PrintUpdateTable();

                    } while (testThread.IsAlive);

                    PrintResultsTable();

                    Console.WriteLine("All tests complete.");
                    Console.WriteLine("Enter Enter to re-run tests...");
                    Console.ReadLine();
                }                
            }
        }
        private static void UITracingConsole()
        {
            CreateConsole();
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("                       Sequence Tracing Mode Enabled                       ");
            Console.WriteLine("                           [Press H for commands]                          ");
            Console.WriteLine("---------------------------------------------------------------------------");

            while (true)
            {
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        Console.WriteLine("Recording Started...");
                        Tracer.IsRecording = true;
                        break;
                    case ConsoleKey.Spacebar:
                        Console.WriteLine(Tracer.IsRecording ? "Trace Recording Paused.": "Trace Recording Resumed.");
                        Tracer.IsRecording = !Tracer.IsRecording;
                        break;
                    case ConsoleKey.Escape:
                        Tracer.RecordedSteps.Clear();
                        Console.Clear();
                        Console.WriteLine("---------------------------------------------------------------------------");
                        Console.WriteLine("                       Sequence Tracing Mode Enabled                       ");
                        Console.WriteLine("                           [Press H for commands]                          ");
                        Console.WriteLine("---------------------------------------------------------------------------");
                        break;
                    case ConsoleKey.H:
                        Console.WriteLine("Press [Enter] to begin your recording session.");
                        Console.WriteLine("Press [Space] to pause and resume your recording session.");
                        Console.WriteLine("Press [Escape] to reset and clear your recording session.");
                        Console.WriteLine("Press [H] to view a list of commands.");
                        Console.WriteLine("Press [S] to stop your recording session.");
                        break;
                    case ConsoleKey.S:
                        if (Tracer.RecordedSteps.Count <= 0)
                        {
                            Console.WriteLine("No Data Recorded.");
                            break;
                        }
                        Console.WriteLine("Recording Session Stopped.");
                        Tracer.IsRecording = false;
                        Console.WriteLine("Would you like to save your recorded sequence?");
                        ConsoleKey response;
                        do
                        {
                            Console.Write("[Y/N]: ");
                            response = Console.ReadKey(false).Key;

                        } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                        if (response == ConsoleKey.Y)
                        {
                            var saveFile = new SaveFileDialog
                            {
                                Filter = "Text (*.txt)|*.txt",
                                FileName = $"SequenceTraceData_{DateTime.Now:dd-MM-yy-HH-mm-ss}"
                            };
                            if (saveFile.ShowDialog() == DialogResult.OK)
                            {
                                using (var sw = new StreamWriter(saveFile.FileName, false))
                                {
                                    foreach (var item in Tracer.RecordedSteps)
                                    {
                                        sw.Write(item.ToString() + Environment.NewLine);
                                    }
                                }      
                                Console.WriteLine("Success.");
                            }
                        }

                        Tracer.RecordedSteps.Clear();
                        Console.Clear();
                        Console.WriteLine("---------------------------------------------------------------------------");
                        Console.WriteLine("                       Sequence Tracing Mode Enabled                       ");
                        Console.WriteLine("                           [Press H for commands]                          ");
                        Console.WriteLine("---------------------------------------------------------------------------");
                        break;
                    default:
                        break;
                }
            }
        }
        private static void RunTestsParallel()
        {
            Parallel.ForEach(UITestList, item =>
            {
                UITestList[item.Key] = new List<object>();
                foreach (var method in item.Key.GetType().GetMethods((BindingFlags)Encapsulation.All))
                {
                    if (Attribute.IsDefined(method, typeof(UITestMethod)))
                    {
                        try
                        {
                            method.Invoke(item.Key, null);
                            UITestList[item.Key].Add(new UITest("Test Pass."));
                        }
                        catch (Exception ex)
                        {
                            UITestList[item.Key].Add(new UITestFailedException("Test failed.", ex));
                        }
                    }
                }
            });
        }
        private static void RunTests()
        {
            foreach (var item in UITestList)
            {
                UITestList[item.Key] = new List<object>();
                foreach (var method in item.Key.GetType().GetMethods((BindingFlags)Encapsulation.All))
                {
                    if (Attribute.IsDefined(method, typeof(UITestMethod)))
                    {
                        try
                        {
                            method.Invoke(item.Key, null);
                            UITestList[item.Key].Add(new UITest("Test Pass."));
                        }
                        catch (Exception ex)
                        {
                            UITestList[item.Key].Add(new UITestFailedException("Test failed.", ex));
                        }
                    }
                }
            }
        }
        private static int GetClassTestCount(ITester test)
        {
            int count = 0;
            foreach (var method in test.GetType().GetMethods((BindingFlags)Encapsulation.All))
            {
                if (Attribute.IsDefined(method, typeof(UITestMethod)))
                {
                    count++;
                }
            }

            return count;
        }
        private static void PrintTable()
        {
            Console.Clear();
            _totalPassedTests = 0;
            _totalFailedTests = 0;
            string header = string.Format("{0}{1}{2}{3}",
                                    "Index".PadRight(10),
                                    "Status".PadRight(15),
                                    "Tests".PadRight(10),
                                    "Test Name".PadRight(15));

            Console.WriteLine(header);
            Console.WriteLine("--------------------------------------------------------------------------");
            var index = 0;
            foreach (var item in UITestList)
            {
                UITestList[item.Key] = null;
                var output = string.Format("{0}{1}{2}{3}",
                                        index++.ToString().PadRight(10),
                                        "Waiting...".PadRight(15),
                                        GetClassTestCount(item.Key).ToString().PadRight(10),
                                        item.Key.GetType().Name.PadRight(15));

                Console.WriteLine(output);
            }
            Console.WriteLine("--------------------------------------------------------------------------");
        }
        private static void PrintUpdateTable()
        {
            Console.SetCursorPosition(0, 0);
            string header = string.Format("{0}{1}{2}",
                                    "Index".PadRight(10),
                                    "Status".PadRight(35),
                                    "Test Name".PadRight(10));

            Console.WriteLine(header);
            Console.WriteLine("--------------------------------------------------------------------------");
            var index = 0;
            foreach (var item in UITestList)
            {
                var results = "";
                if (item.Value is null)
                {
                    results = $"Waiting...";
                }
                else
                {
                    var failed = item.Value.Where(x => x.GetType().BaseType == typeof(Exception)).Count();
                    var passed = item.Value.Where(x => x.GetType() == typeof(UITest)).Count();
                    if (GetClassTestCount(item.Key) != (failed + passed))
                    {
                        results = $"Running: (Pass: {passed}, Fail: {failed})";
                    }
                    else
                    {
                        results = $"Complete: (Pass: {passed}, Fail: {failed})";
                    }
                }
                var output = string.Format("{0}{1}{2}",
                                        index++.ToString().PadRight(10),
                                        results.PadRight(35),
                                        item.Key.GetType().Name.PadRight(10));
                Console.WriteLine(output);
            }
            Console.WriteLine("--------------------------------------------------------------------------");
        }
        private static void PrintResultsTable()
        {
            Console.Clear();
            string header = string.Format("{0}{1}{2}",
                                    "Index".PadRight(10),
                                    "Results".PadRight(35),
                                    "Test Name".PadRight(10));
            Console.WriteLine(header);
            Console.WriteLine("--------------------------------------------------------------------------");
            var index = 0;
            foreach (var item in UITestList)
            {
                var failed = item.Value.Where(x => x.GetType().BaseType == typeof(Exception)).Count();
                var passed = item.Value.Where(x => x.GetType() == typeof(UITest)).Count();
                var results = $"Complete (Pass: {passed}, Fail: {failed})";
                _totalPassedTests += passed;
                _totalFailedTests += failed;
                var output = string.Format("{0}{1}{2}",
                                        index++.ToString().PadRight(10),
                                        results.PadRight(35),
                                        item.Key.GetType().Name.PadRight(10));
                Console.WriteLine(output);
            }
            Console.WriteLine("--------------------------------------------------------------------------");
            var resout = string.Format("{0}{1}",
                                        "Total:".ToString().PadRight(10),
                                        $"(Pass: {_totalPassedTests}, Fail: {_totalFailedTests})".PadRight(35));
            Console.WriteLine(resout);
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine();
        }
        #endregion
    }
}