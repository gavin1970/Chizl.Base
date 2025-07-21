using System.Drawing;
using Chizl.Extensions;
using System.Collections.Generic;

namespace Chizl.ConsoleSupport
{
    public class CBoxOptions
    {
        const int tlCorner = 0;
        const int trCorner = 1;
        const int blCorner = 2;
        const int brCorner = 3;
        const int tbBar = 4;
        const int lrBar = 5;

        private readonly static List<string[]> _borderTypes = new List<string[]>()
        {
            // In the works1
            // "╔","╗","╚","╝","═","║","╠","╣","╦","╩","╬"

            //None
            new string[] { "","","","","","" },
            //Thin Single
            new string[] { "┌","┐","└","┘","─","│" },
            //Thick Single
            new string[] { "┏","┓","┗","┛","━","┃" },
            //Double - special chars - ╠, ╣, ╦, ╩, ╬ (future)
            new string[] { "╔","╗","╚","╝","═","║" },
            //Single, Double
            new string[] { "╒","╕","╘","╛","═","│" },
            //Double, Single
            new string[] { "╓","╖","╙","╜","─","║" },
            //Light Solid
            new string[] { "░", "░", "░", "░", "░", "░" },
            //Medium Solid
            new string[] { "▒", "▒", "▒", "▒", "▒", "▒" },
            //Hard Solid
            new string[] { "▓", "▓", "▓", "▓", "▓", "▓" },
        };
        private CBoxOptions() => IsEmpty = true;
        public CBoxOptions(CBoxBorderType borderType, Color borderColor, Color fgColor, Color bgColor)
        {
            BorderType = borderType;

            BorderColor = borderColor;
            FgColor = fgColor;
            BgColor = bgColor;
            BorderColorStr = borderColor.FGAscii();
            FgColorStr = fgColor.FGAscii();
            BgColorStr = bgColor.BGAscii();

            AllBorders = _borderTypes[borderType.Value()];

            TlCorner = AllBorders[tlCorner];
            TrCorner = AllBorders[trCorner];
            BlCorner = AllBorders[blCorner];
            BrCorner = AllBorders[brCorner];

            LBorder = AllBorders[lrBar];
            RBorder = AllBorders[lrBar];
            TBorder = AllBorders[tbBar];
            BBorder = AllBorders[tbBar];

            HasBorder = borderType.Value() > 0;
            IsEmpty = false;
        }

        public static CBoxOptions NewFgColor(CBoxOptions currCBOptions, Color fgColor) 
            => new CBoxOptions(currCBOptions.BorderType, currCBOptions.BorderColor, fgColor, currCBOptions.BgColor);
        public static CBoxOptions Empty { get { return new CBoxOptions(); } }
        public bool IsEmpty { get; }
        public string[] AllBorders { get; }
        public bool HasBorder { get; }
        public CBoxBorderType BorderType { get; }

        public string TlCorner { get; }
        public string TrCorner { get; }
        public string BlCorner { get; }
        public string BrCorner { get; }

        public string LBorder { get; }
        public string RBorder { get; }
        public string TBorder { get; }
        public string BBorder { get; }

        public Color BorderColor { get; }
        public string BorderColorStr { get; }
        public Color FgColor { get; }
        public string FgColorStr { get; }
        public Color BgColor { get; }
        public string BgColorStr { get; }
    }
}
