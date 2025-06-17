using Chizl;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Demo
{
    internal class Program
    {
        const string _replaceWith = " ";
        static readonly string _reset = ConsoleHelper.ResetColor;
        static readonly string _values = Color.FromArgb(192, 192, 0).FGAscii();
        static readonly string _success = Color.LawnGreen.FGAscii();
        static readonly string _fail = Color.FromArgb(255, 128, 128).FGAscii();
        static readonly string _function = Color.FromArgb(255, 255, 0).FGAscii();
        static readonly string _background = Color.FromArgb(48, 48, 48).BGAscii();
        static readonly string _null = $"{_fail}null{_reset}";
        static readonly List<Type> _typeList = new List<Type>() { typeof(int), typeof(string), typeof(DateTime), 
                                                                  typeof(bool), typeof(RegexPatterns), typeof(Color) };


        static void Main(string[] args)
        {
            ShowDefaults();
            ShowSubString();
            ShowRegexPatterns();
        }

        static void ShowDefaults()
        {
            foreach(var t in _typeList)
            {
                var defVal = t.GetDefaultValue();
                if (defVal == null)
                    defVal = _null;
                Console.WriteLine($"Type: {t.FullName}: Default: >>{defVal}<<");
            }

            Finish();
        }
        static void ShowSubString()
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

            Finish();
        }
        static void ShowRegexPatterns()
        {
            List<string> testStrings = new List<string>() 
            {
                "-Alpha_!1234 5Test.67", "-Alpha_!1234 5Test678.90",
                "~Alpha_!123-4 5Test.67", "~Alpha_!123-4 5Test6-78.90",
                "~Alpha_!(123)4 5Test6-78.90", "~+1 Alpha_!(123) 45Test6-7.890", 
                "~Alpha_!123-4 5-Test67.89", "~Alpha1-_!123-d45Test6-7.890"
            };

            foreach (var enumValue in Enum.GetValues(typeof(RegexPatterns)))
            {
                var eNum = (RegexPatterns)enumValue;
                Console.WriteLine($"------======[ {eNum.Name()} ]======------");
                Console.WriteLine($"Match Pattern   : {eNum.GetPattern(RegexPatternType.Match, false)}");
                Console.WriteLine($"Sanitize Pattern: {eNum.GetPattern(RegexPatternType.Sanitize, false)}");
                Console.WriteLine("---------------------------------------------");

                var i = 1;
                foreach (var str in testStrings)
                {
                    DisplayMatch(i++, eNum, str);
                    Console.WriteLine();
                }

                Finish();
            }
        }
        static void DisplayMatch(int i, RegexPatterns regPat, string str)
        {
            //run match
            var match = regPat.IsMatch(str);
            //set color based on match
            var fgClr = match ? _success : _fail;

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
        static void Finish()
        {
            Console.ReadKey(true);
            ConsoleHelper.ResetBuffer();
        }
    }
}
