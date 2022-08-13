using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularLanguageAnalyser
{
    internal abstract class Lexer
    { 
        protected string source;
        protected int sourceLine;

        public Lexer(string path)
        {
            source = path;
            sourceLine = 1;
        }
       
    }
}
