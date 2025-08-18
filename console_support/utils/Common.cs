using System.Collections.Generic;

namespace Chizl.ConsoleSupport
{
    internal class Common
    {
        public readonly static List<string[]> BorderTypes = new List<string[]>()
        {
            // In the works1
            // "╔","╗","╚","╝","═","║","╠","╣","╦","╩","╬"
            //Double - special chars -  ╠,  ╣,  ╦,  ╩,  ╬ (future)

            //None
            new string[] { "","","","","","" },
            //Thin Single
            new string[] { "┌","┐","└","┘","─","│" },
            //Thick Single
            new string[] { "┏","┓","┗","┛","━","┃" },
            //Double
            new string[] { "╔","╗","╚","╝","═","║" },
            //Single, Double
            new string[] { "╒","╕","╘","╛","═","│" },
            //Double, Single
            new string[] { "╓","╖","╙","╜","─","║" },
            //Light Solid
            new string[] { "░","░","░","░","░","░" },
            //Medium Solid
            new string[] { "▒","▒","▒","▒","▒","▒" },
            //Hard Solid
            new string[] { "▓","▓","▓","▓","▓","▓" },
        };
    }
}
