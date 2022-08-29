using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMSI.Lib
{
    public class RegularExpression
    {
        public static char EPSILON = '$';
        private string regex;
        public RegularExpression(string regex)
        {
            this.regex = regex;
        }

        private static readonly Dictionary<char, int> IPR = new()
        {
            { '+', 2 },
            { '-', 3 },
            { '*', 4 },
            { '(', 6 },
            { ')', 1 },
        };

        private static readonly Dictionary<char, int> SPR = new()
        {
            { '+', 2 },
            { '-', 3 },
            { '*', 4 },
            { '(', 0 }
        };

        private static readonly Dictionary<char, int> R = new()
        {
            { '+', -1 },
            { '-', -1 },
            { '*', 0 },
        };

        public string infixToPostfix()
        {
            Stack<char> stack = new();
            string result = "";
            int rank = 0;
            char x;
            int openBrackets = 0;
            int closedBrackets = 0;
            // Brisemo nepotrebne spejsove
            this.regex = this.regex.Replace(" ", String.Empty);
            bool isLastSymbol = false;
            bool isLastCloseBracket = false;
            bool isLastOpenBracket = false;
            bool isLastKleeneStar = false;
            bool isLastPlus = false;
            int i = -1;

            // Dodajemo specijalni znak konkatenacije -
            // Na odgovarajuca mjesta
            foreach (char symbol in this.regex)
            {
                if (Char.IsLetter(symbol))
                {
                    if (isLastSymbol || isLastCloseBracket || isLastKleeneStar)
                    {
                        this.regex = this.regex.Insert(i, "-");
                        this.regex = this.regex.Replace(" ", String.Empty);
                        i++;
                    }

                    isLastSymbol = true;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                    isLastPlus = false;
                }
                else if (symbol == '(')
                {
                    if (isLastSymbol)
                    {
                        this.regex = this.regex.Insert(i, "-");
                    }
                    else if (isLastKleeneStar)
                    {
                        this.regex = this.regex.Insert(i, "-");
                    }
                    i++;
                    openBrackets++;
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = true;
                    isLastKleeneStar = false;
                    isLastPlus = false;
                }
                else if (symbol == ')')
                {
                    if (isLastPlus)
                    {
                        throw new Exception("Unexpected character");
                    }
                    else if (isLastOpenBracket)
                    {
                        throw new Exception("Unexpected character");
                    }
                    closedBrackets++;
                    isLastCloseBracket = true;
                    isLastSymbol = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                    isLastPlus = false;
                }
                else if (symbol == '*')
                {
                    if (isLastOpenBracket)
                    {
                        throw new Exception("Unexpected character on this position!");
                    }
                    else if (isLastPlus)
                    {
                        throw new Exception("Operator + can't be followed by operator *");
                    }
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = true;
                    isLastPlus = false;
                }
                else if (symbol == '+')
                {
                    if (isLastPlus)
                    {
                        throw new Exception("Operator + can't be followed by another operator +");
                    }
                    else if (isLastOpenBracket)
                    {
                        throw new Exception("Unexpected character");
                    }
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                    isLastPlus = true;
                }
                else
                {
                    throw new Exception("Undefined symbol");
                }
                i++;
            }
            if (openBrackets != closedBrackets) { throw new Exception("Unpaired brackets!"); }
            // Vrsimo konverziju iz infiksa u postfiks
            foreach (char next in this.regex)
            {
                if (Char.IsLetter(next))
                {
                    result += next;
                    rank += 1;
                }
                else
                {
                    while (stack.Count != 0 && (IPR[next] <= SPR[stack.Peek()]))
                    {
                        x = stack.Pop();
                        result += x;
                        rank += R[x];
                        if (rank < 1)
                        {
                            throw new Exception("Nekorektan regex!");
                        }

                    }

                    if (next != ')')
                    {
                        stack.Push(next);
                    }
                    else
                    {
                        x = stack.Pop();
                    }
                }
            }
            while (stack.Count != 0)
            {
                x = stack.Pop();
                result += x;
                rank += R[x];
            }
            if (rank != 1)
            {
                throw new Exception("Nekorektan regex!");
            }
            return result;
        }

        // Izracunavanje postfiksnog izraza u automat
        // Logika se zasniva na tome da cuvamo svaki automat koji konstruisemo u mapi
        // Pri cemu je kljuc jednak vrijednosti regexa automata
        // Dalje kada naidjemo na operaciju skidamo sa steka stringove i pomocu njih pretrazimo 
        // Automate u mapi i nad njima vrsimo neku od operacija (konkatenacija, unija, Kleenov operator)
        // Nakon izvrsene operacije, novodobijeni automat smjestamo u mapu, a njegov regex vracamo na stek
        // Postupak se ponavlja skroz dok ne iscitamo citav postfix
        public Nfa evaluatePostfix(string postfix)
        {
            string pomRegex = "";
            Dictionary<string, Nfa> map = new();
            Stack<string> result = new();
            Nfa newNfa = new();
            int i = 0;

            String rez = "";
            foreach (char x in postfix)
            {
                if (Char.IsLetter(x))
                {
                    // Novi automat x
                    newNfa.AddState("q" + i++);
                    newNfa.AddState("q" + i++);
                    newNfa.StartState = "q" + (i - 2);
                    newNfa.AddFinalState("q" + (i - 1));
                    newNfa.AddSymbolToAlphabet(x);
                    newNfa.AddTransition("q" + (i - 2), x, "q" + (i - 1));
                    if (map.ContainsKey(x.ToString())) // Uklanjamo ako postoji automat sa isitim imenom i dodajemo novi (zbog stanja)
                    {
                        map.Remove(x.ToString());
                        map.Add(x.ToString(), new Nfa(newNfa));
                        pomRegex = x.ToString();
                    }
                    else
                    {
                        map.Add(x.ToString(), new Nfa(newNfa));
                        pomRegex = x.ToString();
                    }

                    newNfa.states.Clear();
                    newNfa.finalStates.Clear();
                    newNfa.getDelta().Clear();
                    newNfa.alphabet.Clear();

                    result.Push(x.ToString());
                }
                else if (x == '*')
                {

                    string oprnd = result.Pop();
                    newNfa = map[(oprnd)].KleenovaZvijezda();
                    rez = oprnd.ToString() + x.ToString();
                    if (map.ContainsKey(oprnd.ToString() + x.ToString())) // Uklanjamo ako postoji automat sa isitim imenom i dodajemo novi (zbog stanja)
                    {
                        map.Remove(oprnd.ToString() + x.ToString());
                        map.Add(oprnd.ToString() + x.ToString(), new Nfa(newNfa));
                        pomRegex = oprnd.ToString() + x.ToString();
                    }
                    else
                    {
                        map.Add(oprnd.ToString() + x.ToString(), new Nfa(newNfa));
                        pomRegex = oprnd.ToString() + x.ToString();
                    }

                    pomRegex = oprnd.ToString() + x.ToString();
                    result.Push(rez);
                    newNfa.states.Clear();
                    newNfa.finalStates.Clear();
                    newNfa.getDelta().Clear();
                    newNfa.alphabet.Clear();


                }
                else if (x == '+')
                {

                    string oprnd2 = result.Pop();
                    string oprnd1 = result.Pop();

                    rez = oprnd1.ToString() + x.ToString() + oprnd2.ToString();
                    newNfa = map[(oprnd2)].Unija(map[(oprnd1)]);
                    map.Add(rez, new Nfa(newNfa));
                    pomRegex = rez;
                    result.Push(rez);
                    newNfa.states.Clear();
                    newNfa.finalStates.Clear();
                    newNfa.getDelta().Clear();
                    newNfa.alphabet.Clear();
                }
                else if (x == '-')
                {

                    string oprnd2 = result.Pop();
                    string oprnd1 = result.Pop();

                    rez = oprnd1.ToString() + x.ToString() + oprnd2.ToString();
                    newNfa = map[(oprnd1)].Spajanje(map[(oprnd2)]);
                    if (map.ContainsKey(rez)) // Uklanjamo ako postoji automat sa isitim imenom i dodajemo novi (zbog stanja)
                    {
                        map.Remove(rez);
                        map.Add(rez, new Nfa(newNfa));
                        pomRegex = rez;
                    }
                    else
                    {
                        map.Add(rez, new Nfa(newNfa));
                        pomRegex = rez;
                    }
                    result.Push(rez);
                    newNfa.states.Clear();
                    newNfa.finalStates.Clear();
                    newNfa.getDelta().Clear();
                    newNfa.alphabet.Clear();

                }
                else
                {
                    throw new Exception("NEKOREKTAN REGEX");
                }
            }
            rez = result.Pop().ToString();
            if (result.Count != 0)
            {
                throw new Exception("NEKOREKTAN REGEX");
            }
            return map[pomRegex];

        }


        public Nfa toNfa()
        {
            Nfa newNfa = new();
            string postfix = this.infixToPostfix();
            return this.evaluatePostfix(postfix);
        }

        public Dfa toDfa()
        {
            Nfa newNfa = new();
            string postfix = this.infixToPostfix();
            return this.evaluatePostfix(postfix).toDfa();
        }
    }

}
