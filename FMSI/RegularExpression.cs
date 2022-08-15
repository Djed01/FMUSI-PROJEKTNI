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
            // Brisemo nepotrebne spejsove
            this.regex = this.regex.Replace(" ", String.Empty);
            bool isLastSymbol = false;
            bool isLastCloseBracket = false;
            bool isLastOpenBracket = false;
            bool isLastKleeneStar = false;
            int i = 0;

            // Dodajemo specijalni znak konkatenacije -
            // Na odgovarajuca mjesta
            foreach (char symbol in this.regex)
            {
                if (Char.IsLetter(symbol))
                {
                    if (isLastSymbol || isLastCloseBracket || isLastKleeneStar)
                    {
                        this.regex = this.regex.Insert(i, "-");
                        i++;
                    }

                    isLastSymbol = true;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;   
                }
                else if (symbol == '(')
                {
                    if (isLastSymbol)
                    {
                        this.regex = this.regex.Insert(i, "-");
                    }
                    else if(isLastKleeneStar)
                    {
                        this.regex = this.regex.Insert(i, "-");
                    }
                    i++;
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = true;
                    isLastKleeneStar = false;
                }
                else if (symbol == ')')
                {
                    isLastCloseBracket = true;
                    isLastSymbol = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                }
                else if (symbol == '*')
                {
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = true;
                }
                else
                {
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                    isLastKleeneStar = false;
                }
                i++;
            }
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
            Dictionary<string, Nfa> set = new();
            Stack<string> result = new();
            Nfa newNfa = new();
            int i = 0;

            String rez = "";
            foreach (char x in postfix)
            {
                if (Char.IsLetter(x))
                {

                    newNfa.AddState("q" + i++);
                    newNfa.AddState("q" + i++);
                    newNfa.StartState = "q" + (i - 2);
                    newNfa.AddFinalState("q" + (i - 1));
                    newNfa.AddSymbolToAlphabet(x);
                    newNfa.AddTransition("q" + (i - 2), x, "q" + (i - 1));
                    set.Add(x.ToString(), new Nfa(newNfa));

                    newNfa.states.Clear();
                    newNfa.finalStates.Clear();
                    newNfa.getDelta().Clear();
                    newNfa.alphabet.Clear();

                    result.Push(x.ToString());
                }
                else if (x == '*')
                {

                    string oprnd = result.Pop();
                    newNfa = set[(oprnd)].KleenovaZvijezda();
                    rez = oprnd.ToString() + x.ToString();
                    set.Add(oprnd.ToString()+ x.ToString(), new Nfa(newNfa));
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
                    newNfa = set[(oprnd2)].Unija(set[(oprnd1)]);
                    set.Add(rez, new Nfa(newNfa));
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
                    newNfa = set[(oprnd1)].Spajanje(set[(oprnd2)]);
                    set.Add(rez, new Nfa(newNfa));
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
                throw new Exception();
            }
            return set[(this.regex.Replace("(",String.Empty).Replace(")",String.Empty))];

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
