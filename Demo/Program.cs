using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using Chizl.Extensions;
using Chizl.ConsoleSupport;
using Chizl.RegexSupport;
using Chizl.ThreadSupport;
using System.Threading;

namespace Demo
{
    internal class Program
    {
        const string _replaceWith = " ";
        static readonly string _reset = ConsoleHelper.GetColorReset;
        static readonly string _values = Color.FromArgb(192, 192, 0).FGAscii();
        static readonly string _success = Color.LawnGreen.FGAscii();
        static readonly string _fail = Color.FromArgb(255, 128, 128).FGAscii();
        static readonly string _function = Color.FromArgb(255, 255, 0).FGAscii();
        static readonly string _background = Color.FromArgb(48, 48, 48).BGAscii();
        static readonly string _null = $"{_fail}null{_reset}";
        static readonly int _len = $"{_success}{_reset}".Length;
        static readonly Array _allRegValues = Enum.GetValues(typeof(RegexPatterns));

        //help prevent duplicates in the test array.
        static readonly List<string> _matchPattern = new List<string>();
        static readonly List<string> _sanitizePattern = new List<string>();
        static readonly Guid _inThread = Guid.NewGuid();

        //demo types
        static readonly List<Type> _typeList = 
        [ 
            typeof(int), typeof(string), typeof(DateTime), 
            typeof(bool), typeof(RegexPatterns), typeof(Color) 
        ];

        //dummy data
        static readonly List<string> _exList =
        [
            "Alpha_!1234 5Test.67", "-Alpha_!1234 5Test678.90",
            "~Alpha_!123-4 5Test.67", "Alpha_!123-4 5Test6-78.90",
            "a!123-.1 5@Test.67.a5", "~!123-.1 5Test.67.a1!123-.1 5Test.67.a5",
            "Aza!123+bbb@Test.cc.aa",
            "~Alpha_!($12,3)4 5Test,6-78.90", "~+1 Alpha_!(123) 45Test6-7.890",
            "~Alpha_!123-4 5-Test67.89", "~Alpha1-_!123-d45Test6-7.890",
            "A#h12C4D6", "#h12C#4D6", "#12CZ4D6",
        ];

        static void Main(string[] args)
        {
            if (ShowThreadLock())
                return;
            //if (BuildTests())
            //    return;
            if (ShowConsoleHlper())
                return;
            if (ShowDefaults())
                return;
            if (ShowSubString())
                return;
            if (ShowClamp())
                return;
            if (ShowRegexPatterns())
                return;
        }

        static bool ShowConsoleHlper()
        {
            DemoTitle("Showing Console Helper for setting 24 bit color within the console.");
            var lawnGreen = Color.LawnGreen;

            var fgLawnGreen = lawnGreen.FGAscii();
            var fgYellow = Color.Yellow.FGAscii();
            var fgRed = Color.FromArgb(255, 0, 0).FGAscii();
            var bgWhite = Color.FromArgb(255, 255, 255).BGAscii();

            var resetExample1 = lawnGreen.ResetAscii();         //Same as resetExample2, just a different way.
            var resetExample2 = ConsoleHelper.GetColorReset;    //same as resetExample1, just a different way.

            Console.WriteLine($"Task 1, {fgLawnGreen}Complete{resetExample1}.");
            Console.WriteLine($"Task 2, {fgYellow}In Progress{resetExample1}.");
            Console.WriteLine($"Task 3,{bgWhite} {fgRed}Not Started ");

            //The following is another example of GetColorReset, but the following does the Console.Write for you.
            //Only useful when your not within a Console.Write/WriteLine yourself.
            //ALSO NOTE: It doesn't take up any space on the screen.
            ConsoleHelper.ColorReset();

            Console.WriteLine($"We are {fgYellow}getting{resetExample2} there...");

            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool ShowDefaults()
        {
            DemoTitle("Showing Types and their default values");
            foreach (var t in _typeList)
            {
                var defVal = t.GetDefaultValue();
                if (defVal == null)
                    defVal = _null;
                Console.WriteLine($"Type: {t.FullName}: Default: >>{defVal}<<");
            }

            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool ShowClamp()
        {
            DemoTitle("Example of Clamp().\nThis is a lot like Math.Clamp, but work in netstandard 2.0 and allows for rounding all in one call.");

            double dbl = 100.51556;
            Console.WriteLine($"Setting value: {dbl}");
            dbl = dbl.Clamp(1.0, 100.0, 6);
            Console.WriteLine($"New Value: {dbl} - dbl.Clamp(1.0, 100.0, 6); - this is because max is 100.0.");
            dbl = 88.51556;
            Console.WriteLine($"Setting value: {dbl}");
            dbl = dbl.Clamp(1.0, 100.0, 6);
            Console.WriteLine($"Round down: {dbl} - dbl.Clamp(1.0, 100.0, 6); - this is because rounding internally is only 0-4, can't be 6.");
            dbl = dbl.Clamp(90.0, 100.0);
            Console.WriteLine($"Round up: {dbl} - dbl.ClampTo(90.0, 100.0); - this is because min value is 90.0.\n" +
                              $"This ClampTo() extension works in netstandard2.0-2.1, .net4.6-net9.0");

            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool ShowThreadLock()
        {
            ClearScreen();
            //Example #1
            var thread1 = new Thread(new ThreadStart(() => { ThreadTest(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)); return; }));
            thread1.Start();

            //cycle the thread.  Without this, thread3 might start before thread1, just as an example.
            Thread.Sleep(1);

            //Example #2
            var thread2 = new Thread(new ThreadStart(() => { ThreadTest(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)); return; }));
            thread2.Start();

            //cycle the thread.  Without this, thread3 might start before thread1, just as an example.
            Thread.Sleep(1);

            //Example #3
            var thread3 = new Thread(new ThreadStart(() => { ThreadTest(TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(7)); return; }));
            thread3.Start();

            //cycle the thread.  Without this, thread3 might start before thread1, just as an example.
            Thread.Sleep(1);

            while (thread1.IsAlive || thread2.IsAlive || thread3.IsAlive) Thread.Sleep(100);

            Console.WriteLine("All threads have stopped...");
            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool ShowSubString()
        {
            DemoTitle("Showing a more controled version of Substring.\nThis validates and correct obvious issues.\n\nAuto Fixes:\nIf startindex is less than 0:\n - startindex will be moved to 0.\nIf startindex + length is greater than string.length:\n - length will be audo adjusted length to get rest of string.");
            var size = 6;
            
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Console.WriteLine($"{alphabet}:");
            var test = alphabet.SubstringEx(-1, size);
            test = test == null ? _null : test;
            Console.WriteLine($"alphabet.SubstringEx(-1, {size}) = {test}");

            test = alphabet.SubstringEx(0, -1);
            test = test == null ? _null : test;
            Console.WriteLine($"alphabet.SubstringEx(0, -1) = {test}");

            for (int i = 0; i < alphabet.Length; i+= size) {
                test = alphabet.SubstringEx(i, size);
                test = test == null ? _null : test;
                Console.WriteLine($"alphabet.SubstringEx({i}, {size}) = {test}");
            }

            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool BuildTests()
        {
            if (Prompt("Do you want to build unit tests for Regex Patterns right now?", new ConsoleKey[] { ConsoleKey.Y, ConsoleKey.N }) == ConsoleKey.N)
                return false;

            bool[] bools = [true, false];
            // I want the tests grouped
            foreach (var b in bools) 
            {
                DemoTitle($"Creating UnitTests {(b?"Match":"Sanitize")} array for RegexPatterns.  Pattern Count: '{_allRegValues.Length}`,  Data Count: `{_exList.Count}`.");
                //loop through Enum RegexPatterns
                foreach (RegexPatterns enumPat in _allRegValues)
                {
                    //loop through all example strings and run it against each pattern.
                    for (int i = 0; i < _exList.Count; i++)
                    {
                        //display each
                        RegExUnitTestArray((i + 1), enumPat, _exList[i], b);
                    }
                }

                var arrUsed = b ? _matchPattern : _sanitizePattern;
                foreach(var s in arrUsed)
                    Console.WriteLine(s);

                if (Finish().Equals(ConsoleKey.Escape))
                    return true;
            }
            return false;
        }
        static bool ShowRegexPatterns()
        {
            DemoTitle("Demoing Hex IsMatch() and Sanitize() methods");

            Console.WriteLine($"Example of usage:\n" +
                             $"bool isHex = RegexPatterns.Hex.IsMatch(\"#FF00FF\");\n" +
                             $"Response: {RegexPatterns.Hex.IsMatch("#FF00FF")}\n\n" +
                             $"bool isHex = RegexPatterns.Hex.IsMatch(\"#ZFF00FF\");\n" +
                             $"Response: {RegexPatterns.Hex.IsMatch("#ZFF00FF")}\n\n" +
                             $"string hex = RegexPatterns.Hex.Sanitize(\"#ZFF00FF\")\n" +
                             $"Response: {RegexPatterns.Hex.Sanitize("#ZFF00FF")}\n\n" +
                             $"string hex = RegexPatterns.Hex.Sanitize(\"#ZFF#00FF\");\n" +
                             $"Response: {RegexPatterns.Hex.Sanitize("#ZFF#00FF")}");

            if (Finish().Equals(ConsoleKey.Escape))
                return true;


            DemoTitle($"Showing all `{_allRegValues.Length}` Regex Patterns against `{_exList.Count}` dummy strings.");

            var loopWaitCnt = (int)(_exList.Count / 3);
            //loop through Enum RegexPatterns
            foreach (RegexPatterns enumPat in _allRegValues)
            {
                //display header
                ShowHeader(enumPat);

                //loop through all example strings and run it against each pattern.
                for (int i = 0; i < _exList.Count; i++)
                {
                    var recNo = ((i - 1) % loopWaitCnt);
                    if (i > 1 && recNo.Equals(0))
                    {
                        if (Finish().Equals(ConsoleKey.Escape))
                            return true;

                        //display header
                        ShowHeader(enumPat);
                    }
                    //display each
                    DisplayMatch((i + 1), enumPat, _exList[i]);
                    //Console.WriteLine();
                }

                if (Finish().Equals(ConsoleKey.Escape))
                    return true;
            }
            
            return false;
        }
        static void RegExUnitTestArray(int i, RegexPatterns regPat, string str, bool matches)
        {
            //oddly enough, is printing out with "True" or "False", so we have to convert to string and lower case it for it to write compilable code.
            var match1 = regPat.IsMatch(str).ToString().ToLower();
            var newStr = regPat.Sanitize(str);
            var match2 = regPat.IsMatch(newStr).ToString().ToLower();
            var allowInsert = true;
            var allowInsert2 = true;
            var arrEle = string.Empty;

            var foundPat = (matches ? _matchPattern : _sanitizePattern).Where(w => w.Contains($".{regPat.Name()},"));

            if (matches)
            {
                //if patterns already saved
                if (foundPat.Count() > 0)
                {
                    //if less than 2 success for this pattern, add it.  If less than 2 fails, add it.
                    allowInsert = foundPat.Where(w => w.Contains($", {match1} ")).Count() < 2;
                    //if not same string after conversion or response came back differently, verify
                    if (!str.Equals(newStr) || !match1.Equals(match2))
                        //if less than 2 success for this pattern, add it.  If less than 2 fails, add it.
                        allowInsert2 = foundPat.Where(w => w.Contains($", {match2} ")).Count() < 2;
                    else
                        allowInsert2 = false;
                }

                if (allowInsert)
                {
                    arrEle = $"new object[] {{ RegexPatterns.{regPat.Name()}, \"{str}\", {match1} }},";
                    if (!_matchPattern.Contains(arrEle))
                        _matchPattern.Add(arrEle);
                }

                if (allowInsert2)
                {
                    arrEle = $"new object[] {{ RegexPatterns.{regPat.Name()}, \"{newStr}\", {match2} }},";
                    if (!_matchPattern.Contains(arrEle))
                        _matchPattern.Add(arrEle);
                }
            }
            else
            {
                if (foundPat.Count() < 4)
                {
                    arrEle = $"new object[] {{ RegexPatterns.{regPat.Name()}, \"{str}\", \"{newStr}\" }},";
                    if (!_sanitizePattern.Contains(arrEle))
                        _sanitizePattern.Add(arrEle);
                }
            }
        }
        static void DisplayMatch(int i, RegexPatterns regPat, string str)
        {
            //run match
            var match = regPat.IsMatch(str);
            //set color based on match
            var fgClr = match ? _success : _fail;

            //run match on original data
            Console.WriteLine($"{i}: IsMatch(\"{str}\") -> {fgClr}{match}{_reset}");

            //option 1, default replace with is "".
            var newStr = regPat.Sanitize(str);
            Console.WriteLine($"{i}: {_function}Sanitize{_reset}(\"{_values}{str}{_reset}\") -> >>{_values}{newStr}{_reset}<<");
            //run match
            match = regPat.IsMatch(newStr);
            //set color based on match
            fgClr = match ? _success : _fail;
            Console.WriteLine($"{i}: IsMatch(\"{newStr}\") -> {fgClr}{match}{_reset}");

            //show Replace with 2 args and replace all invalid with space " ".
            if (regPat.Name().Contains("Alpha"))
            {
                //option 2, change default to replace with "X"
                var newStr2 = regPat.Sanitize(str, _replaceWith);
                Console.WriteLine($"{i}: {_function}Sanitize{_reset}(\"{_values}{str}{_reset}\", \"{_background}{_replaceWith}{_reset}\") -> >>{_background}{newStr2}{_reset}<<");
                //run match
                match = regPat.IsMatch(newStr2);
                //set color based on match
                fgClr = match ? _success : _fail;
                Console.WriteLine($"{i}: IsMatch(\"{newStr2}\") -> {fgClr}{match}{_reset}");
            }
        }
        static void ShowHeader(RegexPatterns eNum)
        {
            var name = $"------======[ {_success}{eNum.Name()}{_reset} ]======------";
            Console.WriteLine(name);
            Console.WriteLine($"Match Pattern   : {eNum.GetInfo(RegexPatternType.Match, false)}");
            Console.WriteLine($"Sanitize Pattern: {eNum.GetInfo(RegexPatternType.Sanitize, false)}");
            Console.WriteLine($"Strategy        : {eNum.GetInfo(RegexPatternType.SanitizeStrategy, false)}");
            Console.WriteLine($"Custom Method   : {eNum.GetInfo(RegexPatternType.CustomMethodName, false)}");
            Console.WriteLine($"Example(s)      : {eNum.GetInfo(RegexPatternType.Examples, false)}");
            Console.WriteLine(new string('-', name.Length - _len));
        }
        static ConsoleKey Prompt(string msg, ConsoleKey[] validKeys)
        {
            var key = ConsoleKey.ExSel;

            Console.WriteLine(msg);
            if (validKeys != null && validKeys.Length > 0)
            {
                while (!validKeys.Contains(key))
                    key = Console.ReadKey(true).Key;
            }
            else
                key = Console.ReadKey(true).Key;

            return key;
        }
        static ConsoleKey Finish()
        {
            var key = Prompt("\nPress 'Esc' to exit.  Press any other key to continue.", null);
            ClearScreen();
            return key;
        }
        static void DemoTitle(string title)
        {
            ClearScreen();
            var vTitle = title.Split('\n');
            var maxLen = 0;
            foreach (var v in vTitle)
                maxLen = Math.Max(v.Length, maxLen);

            var bar = new string('=', maxLen);

            Console.WriteLine(bar);
            foreach (var v in vTitle) 
                Console.WriteLine(v);
            Console.WriteLine(bar);
        }
        static void ThreadTest(TimeSpan simulatedWorkDuration, TimeSpan lockTimeout)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var requestTime = DateTime.Now;

            Console.WriteLine($"\n***************************\n" +
                              $"{threadId}: Start ThreadTest({simulatedWorkDuration})\n" +
                              $"{threadId}: Lock requested at {requestTime:HH:mm:ss.fffffff}");

            DateTime lockAcquiredAt;
            bool lockAcquired;

            using (var lockHandle = TLock.Acquire(_inThread, new TLockOptions(lockTimeout)))
            {
                lockAcquiredAt = DateTime.Now;
                lockAcquired = lockHandle.Acquired;

                if (lockAcquired)
                {
                    Console.WriteLine($"{threadId}: Lock acquired at {lockAcquiredAt:HH:mm:ss.fffffff}");
                    Thread.Sleep(simulatedWorkDuration);
                }
                else
                {
                    Console.WriteLine($"{threadId}: Failed to acquire lock (timed out) at {lockAcquiredAt:HH:mm:ss.fffffff}");
                }
            }

            var waited = lockAcquiredAt - requestTime;

            Console.WriteLine($"{threadId}: Waited for: {waited.TotalMilliseconds:N0} ms (Expected max: {lockTimeout.TotalMilliseconds:N0} ms)");
            Console.WriteLine($"{threadId}: Ended at: {DateTime.Now:HH:mm:ss.fffffff}");
            Console.WriteLine($"{threadId}: Exiting ThreadTest({simulatedWorkDuration})\n***************************\n");
        }

        static void ClearScreen() => ConsoleHelper.ResetConsoleBuffer();
    }
}
