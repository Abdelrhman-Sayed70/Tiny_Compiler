using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JASON_Compiler
{
    public static class JASON_Compiler
    {
        public static Scanner Jason_Scanner = new Scanner();
        public static Parser Jason_Parser = new Parser();
        public static List<Token> TokenStream = new List<Token>();
        public static Node treeroot;

        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            Jason_Scanner.StartScanning(SourceCode);
            //Parser
            Jason_Parser.StartParsing(TokenStream);
            treeroot = Jason_Parser.root;
        } 
    }
}
