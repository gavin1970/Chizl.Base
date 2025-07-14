using System.Drawing;
using Chizl.Base.Utils;

namespace Chizl.ConsoleSupport
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns the Ascii Escape characters for Foreground color within a Console.Window.<br/>
        /// Color.<any>.Reset() will clear all foreground and background console colors.
        /// </summary>
        /// <returns>Ascii Escape characters</returns>
        public static string FGAscii(this Color @this) => GetAsciiEscape(@this, true);
        /// <summary>
        /// Returns the Ascii Escape characters for Background color within a Console.Window.<br/>
        /// Color.<any>.Reset() will clear all foreground and background console colors.
        /// </summary>
        /// <returns>Ascii Escape characters</returns>
        public static string BGAscii(this Color @this) => GetAsciiEscape(@this, false);
        /// <summary>
        /// Color.<any>.ResetAscii() will clear all foreground and background console colors from this point after.
        /// </summary>
        /// <returns>Ascii Escape characters</returns>
        public static string ResetAscii(this Color _) => ConsoleHelper.GetColorReset;

        #region Private Support Methods
        private static string GetAsciiEscape(Color clr, bool isForeground)
        {
            //Get code for Foreground or Background based on selection.
            var present = isForeground ? AsciiColorType.FGClr : AsciiColorType.BGClr;
            //Display it all using 24bit, because 4, 8, and 16 bit color exists in the 24bit layer.
            var palette = AsciiColorPalette.AEC_24Bit;

            //return of the 24bit ascii code
            return $"\x1b[{present.Value()};{palette.Value()};{clr.R};{clr.G};{clr.B}m";
        }
        #endregion
    }
}
