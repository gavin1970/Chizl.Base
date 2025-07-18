namespace Chizl.ConsoleSupport
{
    internal enum AsciiColorType
    {
        /// <summary>
        /// Foreground Identifier
        /// </summary>
        FGClr = 38,
        /// <summary>
        /// Background Identifier
        /// </summary>
        BGClr = 48,
    }
    internal enum AsciiColorPalette
    {
        /// <summary>
        /// 4-bit: 16 colors.
        /// </summary>
        AEC_4Bit = 5,
        /// <summary>
        /// 8-bit: 256 colors
        /// </summary>
        AEC_8Bit = 5,
        /// <summary>
        /// 16-bit: 65,000 colors
        /// </summary>
        AEC_16Bit = 2,
        /// <summary>
        /// 24-bit: 16,777,216 colors
        /// </summary>
        AEC_24Bit = 2,
    }
}
