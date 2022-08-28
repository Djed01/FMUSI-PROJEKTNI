using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularLanguageAnalyser
{
    public class RegexLexer : Lexer
    {
        public RegexLexer(string path) : base(path) { }
        // Bool promjenljive za provjeru posljednjeg simbola
        private bool isLastSymbol = false;
        private bool isLastClosedBracket = false;
        private bool isLastOpenBracket = false;
        private bool isLastKleeneStar = false;
        private bool isLastPlus = false;
        // Broj otvorenih i zatvorenih zagrada
        // Kako bismo provjerili balansiranost zagrada
        private int numOfOpenBrackets = 0;
        private int numOfClosedBrackets = 0;

        public void analyse()
        {
            // Broj linija fajla
            int lengthOfFile = System.IO.File.ReadLines(REGEX_PATH + source).Count();

            foreach (string line in System.IO.File.ReadLines(REGEX_PATH + source))
            {
                // Provjeravamo duzinu linije koja mora biti 1
                if (line.Length != 1)
                {
                    throw new Exception("Line " + sourceLine + ": Nonvalid foramat");
                }

                // Ukoliko je simbol, postavljamo bool promjenljive na odgovarajuce vrijednosti
                if (Char.IsLetter(Char.Parse(line)))
                {
                    isLastSymbol = true;
                    isLastClosedBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                    isLastPlus = false;
                }
                else if ("+".Equals(line))
                {
                    // Provjeravamo prethodni simbol i na osnovu njega bacamo izuzetak ukoliko operator + ne smije biti posle njega
                    if (isLastPlus)
                    {
                        throw new Exception("Line " + sourceLine + ": Operator + can't be folowed by another operator +");
                    }
                    else if (isLastOpenBracket)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character");
                    }
                    // Provjeravamo da li je operator + na pocetku ili na kraju, te bacamo izuzetak ako jeste.
                    if (sourceLine == 1)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character on this position");
                    }
                    else if (sourceLine == lengthOfFile)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character on this position");
                    }

                    isLastSymbol = false;
                    isLastClosedBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                    isLastPlus = true;
                }
                else if ("*".Equals(line))
                {
                    // Provjeravamo prethodni simbol i na osnovu njega bacamo izuzetak ukoliko operator * ne smije biti posle njega
                    if (isLastPlus)
                    {
                        throw new Exception("Line " + sourceLine + ": Operator + can't be folowed by operator *");
                    }
                    else if (isLastOpenBracket)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character");
                    }
                    // Provjeravamo da li je operator * na pocetku, te bacamo izuzetak ako jeste.
                    if (sourceLine == 1)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character on this position");
                    }

                    isLastSymbol = false;
                    isLastClosedBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = true;
                    isLastPlus = false;
                }
                else if ("(".Equals(line))
                {
                    // Provjeravamo da li je ( na kraju, te bacamo izuzetak ako jeste.
                    if (sourceLine == lengthOfFile)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character on this position");
                    }
                    isLastSymbol = false;
                    isLastClosedBracket = false;
                    isLastOpenBracket = true;
                    isLastKleeneStar = false;
                    isLastPlus = false;
                    numOfOpenBrackets++;
                }
                else if (")".Equals(line))
                {
                    // Provjeravamo prethodni simbol i na osnovu njega bacamo izuzetak ukoliko ) ne smije biti posle njega
                    if (isLastPlus)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character");
                    }
                    else if (isLastOpenBracket)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character");
                    }
                    // Provjeravamo da li je ) na pocetku, te bacamo izuzetak ako jeste.
                    if (sourceLine == 1)
                    {
                        throw new Exception("Line " + sourceLine + ": Unexpected character on this position");
                    }
                    
                    isLastSymbol = false;
                    isLastClosedBracket = true;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                    isLastPlus = false;
                    numOfClosedBrackets++;
                }
                else
                {
                    // Ukoliko smo naisli ne nedefinisan simbol bacamo izuzetak
                    throw new Exception("Line " + sourceLine + ": Unexpected character");
                }
                sourceLine++;
            }
            if (numOfOpenBrackets != numOfClosedBrackets)
            {
                // Neuparene zagrade
                throw new Exception("Unpaired brackets");
            }
        }



    }
}
