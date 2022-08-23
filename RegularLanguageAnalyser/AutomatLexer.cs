using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularLanguageAnalyser
{
    public class AutomatLexer : Lexer
    {
        // Bool promjenljive kako bi znali sta citamo
        private bool isStates = false;
        private bool isAlphabet = false;
        private bool isTransition = false;
        private bool isFinalStates = false;
        // Set za stanja i alfabet za provjeru sintakse
        private HashSet<string> states = new HashSet<string>();
        private HashSet<string> alphabet = new HashSet<string>();

        public AutomatLexer(string path) : base(path) { }

        public void analyse()
        {
            foreach(string line in System.IO.File.ReadLines(source))
            {
                // Ako naidjemo na neku od kljucnih rijeci postavljamo odgovarajucu bool promjenljivu na true
                if ("States:".Equals(line))
                {
                    isStates = true;
                    isAlphabet = false;
                    isTransition = false;
                    isFinalStates = false;
                    sourceLine++;
                } else if("Alphabet:".Equals(line))
                {
                    isStates = false;
                    isAlphabet = true;
                    isTransition = false;
                    isFinalStates = false;
                    sourceLine++;
                }
                else if ("Transitions:".Equals(line))
                {
                    isStates = false;
                    isAlphabet = false;
                    isTransition = true;
                    isFinalStates = false;
                    sourceLine++;
                } else if ("Final states:".Equals(line))
                {
                    isStates = false;
                    isAlphabet = false;
                    isTransition = false;
                    isFinalStates = true;
                    sourceLine++;
                }
                else
                {
                    if (isStates)
                    {
                        // Ukoliko stanje sadrzi neki od neocekivanih simbola bacamo izuzetak
                        if (!line.All(Char.IsLetterOrDigit))
                        {
                            throw new Exception("Nonvalid state format");
                        }
                        //Dodajemo stanje u set
                        states.Add(line);
                    } else if (isAlphabet)
                    {
                        // Ako simbol alfabeta nije simbol, nego rijec (duzi od jednog karaktera), bacamo izuzetak
                        if (line.Length != 1)
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Symbol needs to be a single character!");
                        }
                        // Ako je simbol neocekivan bacamo izuzetak
                        else if (!line.All(Char.IsLetterOrDigit))
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Nonvalid symbol format");
                        }
                        // Dodajemo simbol u alfabet
                        alphabet.Add(line);
                    }
                    else if (isTransition)
                    {
                        // Provjeravamo format prelaza (state1,symbol,state2)
                        if (line[0] != '(')
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Nonvalid transition format");
                        } else if (line[line.Length - 1] != ')')
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Nonvalid transition format");
                        }
                        string newLine = line.Substring(1, line.Length - 2);
                        string[] words = newLine.Split(','); // Smjestamo stanja i simbol u niz
                        if (words.Count() != 3)
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Nonvalid transition format");
                        }
                        foreach (var word in words)
                        {
                            // Provjeravamo format stanja/simbola
                            if (!word.All(Char.IsLetterOrDigit))
                            {
                                throw new Exception("Line " + sourceLine + ": " + "Nonvalid symbol or state format");
                            }
                        }
                        // Provjeravamo ispravnost simbola
                        if (words[1].Length != 1)
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Nonvalid symbol format");
                        }

                        // Provjeravamo da li stanja u funkciji prelaza postoje u definisanim stanjima
                        if (!states.Contains(words[0]) || !states.Contains(words[2]))
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Not defined state in states");
                        }
                        // Provjeravamo da li simbol u funkciji prelaza postoji u alfabetu
                        if (!alphabet.Contains(words[1]))
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Not defined symbol in alphabet");
                        }
                        
                    }
                    else if (isFinalStates)
                    {
                        // Provjeravamo foramt finalnog stanja
                        if (!line.All(Char.IsLetterOrDigit))
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Nonvalid final state format");
                        }
                        // Provjeravamo da li finalno stanje postoji u definisanim stanjima
                        if (!states.Contains(line))
                        {
                            throw new Exception("Line " + sourceLine + ": " + "Not defined final state in states");
                        }       
                    }
                    sourceLine++;
                }
            }
        }
    }
}
