using System;

namespace Chizl.ConsoleSupport
{
    public static class ConsoleHelper
    {
        /// <summary>
        /// Returns the Ascii Escape Codes to use within your Console.Write/WriteLine to reset colors from that point after.
        /// <code>
        /// Example:
        ///     var fgLawnGreen = Color.LawnGreen.FGAscii();
        ///     var resetClr = ConsoleHelper.GetColorReset;
        ///     Console.WriteLine($"We are {fgLawnGreen}Good{resetClr} to go.");
        /// </code>
        /// </summary>
        /// <returns>Ascii Escape Code: \u001b[0m</returns>
        public static string GetColorReset { get { return "\u001b[0m"; } }
        /// <summary>
        /// Escutes Ascii Escape Codes to reset colors after that point in time. Previous colors will not be erased. <br/>
        /// This will not take up bytes on the screen.
        /// <code>
        /// Example:
        ///     var fgLawnGreen = Color.LawnGreen.FGAscii();
        ///     var fgYellow = Color.Yellow.FGAscii();
        ///     var fgRed = Color.FromArgb(255, 0, 0).FGAscii();
        ///     Console.WriteLine($"{fgLawnGreen}Task 1, Complete.");
        ///     Console.WriteLine($"{fgYellow}Task 2, In Progress.");
        ///     Console.WriteLine($"{fgRed}Task 3, Not Started.");
        ///     ConsoleHelper.ColorReset();
        /// </code>
        /// </summary>
        public static void ColorReset() { Console.Write(GetColorReset); }
        /// <summary>
        /// Will clear the console screen and the entire console buffer.
        /// <code>
        /// \x1b or \u001b: by itself is the Escape character. (ASCII code 27).
        /// The following 'c' is a literal character within the sequence and in this case means: 
        ///   clean or clear.
        ///   
        /// In full: \u001bc
        /// The follow might not be required in most cases as the above can clear the buffer, 
        /// but in some cases the following is required.
        /// 
        /// Appending the escape \x1b followed by:
        /// [3J: This is an ANSI escape sequence that forces clear for the entire terminal screen.
        /// - [ : Introduces the control sequence.
        /// - 3 : Specifies the type of screen clearing.
        /// - J : Indicates the action(clear screen).
        /// 
        /// Console.Write("\u001bc");           //works most the time.
        /// vs
        /// Console.Write("\u001bc\x1b[3J");    //works all the time.
        /// 
        /// Cursor will be sitting at X: 0, Y: 0
        /// </code>
        /// </summary>
        public static void ResetConsoleBuffer() { Console.Write("\u001bc\x1b[3J"); }
    }
}
