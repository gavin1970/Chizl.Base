using System;

namespace Chizl
{
    public static class ConsoleHelper
    {
        public static string ResetColor { get { return "\u001b[0m"; } }
        public static void ResetBuffer() { Console.Write("\u001bc\x1b[3J"); }
    }
}
