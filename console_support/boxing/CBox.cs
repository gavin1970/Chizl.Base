using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chizl.ConsoleSupport
{
    public class CBox
    {
        static readonly string _reset = ConsoleHelper.GetColorReset;
        private CBoxOptions _boxOptions;
        private int _padCnt;
        private int _maxSize;

        #region Public Methods
        /// <summary>
        /// Builds a box around the console text you want to display.
        /// </summary>
        /// <param name="boxOptions"></param>
        /// <param name="padCnt"></param>
        /// <param name="maxSize"></param>
        public CBox(CBoxOptions boxOptions, int padCnt = 2, int maxSize = -1)
        {
            _boxOptions =boxOptions;
            _padCnt=padCnt;
            _maxSize=maxSize;
        }
        /// <summary>
        /// Builds a Box around the text, with specific colors specified.<br/>
        /// </summary>
        /// <param name="multiLineStr">Single string, but can have CrLf that will determine end of each line.</param>
        public void Show(string[] multiLineStr) => Show(multiLineStr, _boxOptions, _padCnt, _maxSize);
        /// <summary>
        /// Builds a Box around the text, with specific colors specified.<br/>
        /// </summary>
        /// <param name="multiLineStr">Single string, but can have CrLf that will determine end of each line.</param>
        public void Show(string multiLineStr) => Show(multiLineStr, _boxOptions, _padCnt, _maxSize);
        /// <summary>
        /// Used to only tempararly change the text color, but use the same options.  Color will revert back to original on next call.
        /// </summary>
        /// <param name="multiLineStr"></param>
        /// <param name="fgColor"></param>
        public void Show(string multiLineStr, Color fgColor) => Show(multiLineStr, CBoxOptions.NewFgColor(_boxOptions, fgColor), _padCnt, _maxSize);
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Polymorphism to Show(string[] multiLineStr, Color fgColor, Color bgColor, ...)<br/>
        /// Setting Defaults: fgColor-Color.White, bgColor-Color.Empty<br/>
        /// This method, will remove all \r and split by \n to create the string needed for Show(string[] multiLineStr, ...)
        /// </summary>
        /// <param name="multiLineStr">Single string, but can have CrLf that will determine end of each line.</param>
        /// <param name="fgColor">Foreground color in 24-bit System.Color.</param>
        /// <param name="bgColor"></param>
        /// <param name="padCnt"></param>
        /// <param name="maxSize"></param>
        /// <param name="borderType">Singe, Double, Right/Left Border Single /w Top/Bottom Double, or Right/Left Border Double /w Top/Bottom Single</param>
        public static void Show(string multiLineStr, int padCnt = 2, int maxSize = -1)
                        => Show(multiLineStr.Replace("\r", "").Split('\n'), CBoxOptions.Empty, padCnt, maxSize);
        /// <summary>
        /// Polymorphism to Show(string[] multiLineStr, Color fgColor, Color bgColor, ...)<br/>
        /// Setting Defaults: fgColor-Color.White, bgColor-Color.Empty<br/>
        /// </summary>
        /// <param name="multiLineStr">Single string, but can have CrLf that will determine end of each line.</param>
        /// <param name="fgColor">Foreground color in 24-bit System.Color.</param>
        /// <param name="bgColor"></param>
        /// <param name="padCnt"></param>
        /// <param name="maxSize"></param>
        /// <param name="borderType">Singe, Double, Right/Left Border Single /w Top/Bottom Double, or Right/Left Border Double /w Top/Bottom Single</param>
        public static void Show(string[] multiLineStr, int padCnt = 2, int maxSize = -1) 
                        => Show(multiLineStr, CBoxOptions.Empty, padCnt, maxSize);
        /// <summary>
        /// Polymorphism to Show(string[] multiLineStr, ...)<br/>
        /// This method, will remove all \r and split by \n to create the string needed for Show(string[] multiLineStr, ...)
        /// </summary>
        /// <param name="multiLineStr">Single string, but can have CrLf that will determine end of each line.</param>
        /// <param name="fgColor">Foreground color in 24-bit System.Color.</param>
        /// <param name="bgColor"></param>
        /// <param name="padCnt"></param>
        /// <param name="maxSize"></param>
        /// <param name="borderType">Singe, Double, Right/Left Border Single /w Top/Bottom Double, or Right/Left Border Double /w Top/Bottom Single</param>
        public static void Show(string multiLineStr, CBoxOptions boxOptions, int padCnt = 2, int maxSize = -1)
                        => Show(multiLineStr.Replace("\r", "").Split('\n'), boxOptions, padCnt, maxSize);
        /// <summary>
        /// Builds a Box around the text, with specific colors specified.<br/>
        /// </summary>
        /// <param name="multiLineStr">Single string, but can have CrLf that will determine end of each line.</param>
        /// <param name="fgColor">Foreground color in 24-bit System.Color.</param>
        /// <param name="bgColor">Background color in 24-bit System.Color.</param>
        /// <param name="padCnt">Padding preceeding and post each line</param>
        /// <param name="maxSize">Max the box longer than the text, you can put it here.  Will auto adjust if less than a line to display.</param>
        /// <param name="borderType">Singe, Double, Right/Left Border Single /w Top/Bottom Double, or Right/Left Border Double /w Top/Bottom Single</param>
        public static void Show(string[] multiLineStr, CBoxOptions boxOptions, int padCnt = 2, int maxSize = -1)
        {
            var maxLen = GetMaxLength(multiLineStr, maxSize);
            if (maxLen < maxSize)
                maxLen = maxSize;

            var borders = boxOptions.AllBorders;
            if (boxOptions.HasBorder)
                maxLen += 2;    //left and right border

            var borderColor = $"{boxOptions.BgColorStr}{boxOptions.BorderColorStr}";
            var innerColor = $"{boxOptions.BgColorStr}{boxOptions.FgColorStr}";
            var padding = new string(' ', padCnt);

            var tB = boxOptions.HasBorder ? $"{padding}{borderColor}{boxOptions.TlCorner}{new string(boxOptions.TBorder[0], maxLen)}{boxOptions.TrCorner}{_reset}" : "";
            var lB = $"{_reset}{padding}{borderColor}{boxOptions.LBorder}{innerColor} ";
            var rB = $"{borderColor}{boxOptions.RBorder}{_reset}";
            var bB = boxOptions.HasBorder ? $"{padding}{borderColor}{boxOptions.BlCorner}{new string(boxOptions.BBorder[0], maxLen)}{boxOptions.BrCorner}{_reset}" : "";

            //using this array so all text can be written at the same time with one
            //Console.WriteLine to not get cut into by another threads console.write().
            var writeLines = new List<string>();
            if (boxOptions.HasBorder)
                writeLines.Add(tB);

            if (boxOptions.HasBorder)
                maxLen -= 1;

            foreach (var line in multiLineStr)
            {
                var addSpace = line.Length < maxLen ? new string(' ', maxLen - line.Length) : "";
                writeLines.Add($"{lB}{line}{addSpace}{rB}");
            }

            if (boxOptions.HasBorder)
                writeLines.Add(bB);

            Console.WriteLine($"{string.Join("\n", writeLines.ToArray())}");
        }
        #endregion

        #region Private Helper
        private static int GetMaxLength(string[] multiLineStr, int maxSize)
        {
            foreach (var line in multiLineStr)
            {
                var len = line.Length;
                if (len > maxSize)
                    maxSize = len;
            }

            return maxSize;
        }
        #endregion
    }

}
