using Chizl;
using System;
using System.Drawing;
using System.Collections.Generic;

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
        static readonly List<Type> _typeList = new List<Type>() { typeof(int), typeof(string), typeof(DateTime),
                                                                  typeof(bool), typeof(RegexPatterns), typeof(Color) };

        static void Main(string[] args)
        {
            if (ShowConsoleHlper())
                return;
            if (ShowDefaults())
                return;
            if (ShowSubString())
                return;
            if (ShowRegexPatterns())
                return;
        }

        static bool ShowConsoleHlper()
        {
            var lawnGreen = Color.LawnGreen;

            var fgLawnGreen = lawnGreen.FGAscii();
            var fgYellow = Color.Yellow.FGAscii();
            var fgRed = Color.FromArgb(255, 0, 0).FGAscii();
            var bgWhite = Color.FromArgb(255, 255, 255).BGAscii();

            var resetExample1 = lawnGreen.ResetAscii();         //Same as resetExample2, just a different way.
            var resetExample2 = ConsoleHelper.GetColorReset;    //same as resetExample1, just a different way.

            Console.WriteLine($"Task 1, {fgLawnGreen}Complete{resetExample1}.");
            Console.WriteLine($"Task 2, {fgYellow}In Progress{resetExample1}.");
            Console.WriteLine($"Task 3,{bgWhite} {fgRed}Not Started {resetExample1}");
            
            ConsoleHelper.ColorReset();
            Console.WriteLine($"We are {fgYellow}getting{resetExample2} there...");

            return Finish().Equals(ConsoleKey.Escape);
        }

        static bool ShowDefaults()
        {
            foreach(var t in _typeList)
            {
                var defVal = t.GetDefaultValue();
                if (defVal == null)
                    defVal = _null;
                Console.WriteLine($"Type: {t.FullName}: Default: >>{defVal}<<");
            }

            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool ShowSubString()
        {
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

            Console.WriteLine($"{(new string('-', 30))}\n");

            double dbl = 100.51556;
            Console.WriteLine($"Setting value: {dbl}");
            dbl = dbl.SetBoundary(1.0, 100.0, 6);
            Console.WriteLine($"New Value: {dbl} - dbl.SetBoundary(1.0, 100.0, 6); - this is because max is 100.0.");
            dbl = 88.51556;
            Console.WriteLine($"Setting value: {dbl}");
            dbl = dbl.SetBoundary(1.0, 100.0, 6);
            Console.WriteLine($"Round down: {dbl} - dbl.SetBoundary(1.0, 100.0, 6); - this is because rounding internally is only 0-4, can't be 6.");
            dbl = dbl.ClampTo(90.0, 100.0);
            Console.WriteLine($"Round up: {dbl} - dbl.ClampTo(90.0, 100.0); - this is because min value is 90.0.\n" +
                              $"This ClampTo() extension works in netstandard2.0-2.1, .net4.6-net9.0");

            return Finish().Equals(ConsoleKey.Escape);
        }
        static bool ShowRegexPatterns()
        {
            //dummy data
            List<string> exList = new List<string>() 
            {
                "-Alpha_!1234 5Test.67", "-Alpha_!1234 5Test678.90",
                "~Alpha_!123-4 5Test.67", "~Alpha_!123-4 5Test6-78.90",
                "~Alpha_!($12,3)4 5Test,6-78.90", "~+1 Alpha_!(123) 45Test6-7.890",
                "~Alpha_!123-4 5-Test67.89", "~Alpha1-_!123-d45Test6-7.890",
                "A#h12C4D6", "#h12C#4D6", "#12CZ4D6",
            };

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

            //loop through Enum RegexPatterns
            foreach (RegexPatterns enumPat in Enum.GetValues(typeof(RegexPatterns)))
            {
                //display header
                ShowHeader(enumPat);

                //loop through all example strings and run it against each pattern.
                for (int i = 0; i < exList.Count; i++)
                {
                    var recNo = ((i - 1) % 5);
                    if (i > 5 && recNo.Equals(0))
                    {
                        if (Finish().Equals(ConsoleKey.Escape))
                            return true;

                        //display header
                        ShowHeader(enumPat);
                    }
                    //display each
                    DisplayMatch((i + 1), enumPat, exList[i]);
                    //Console.WriteLine();
                }

                if (Finish().Equals(ConsoleKey.Escape))
                    return true;
            }
            
            return false;
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
        static ConsoleKey Finish()
        {
            Console.WriteLine("\nPress 'Esc' to exit.  Press any other key to continue.");
            var key = Console.ReadKey(true).Key;
            ConsoleHelper.ResetConsoleBuffer();
            return key;
        }
    }
}
