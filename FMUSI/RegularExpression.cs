using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMUSI
{
    internal class RegularExpression
    {
        public static char EPSILON = '$';
        private string regex;
        public RegularExpression(string regex)
        {
            this.regex = regex;
        }

        public int IPR(char c)
        {
            switch (c)
            {
                case '+':
                    return 2;
                    break;
                    case '-':
                    return 2;
                    break;
                case '*':
                    return 3;
                    break;
                case '/':
                    return 3;
                    break;
                case '^':
                        return 5;
                        break;      
                case '(':
                    return 6;
                    break;
                    case ')':
                    return 1;
                    break;
                    
                default:
                    return 0;
                    break;
            }
        }

        public int SPR(char c)
        {
            switch (c)
            {
                case '+':
                    return 2;
                    break;
                case '-':
                    return 2;
                    break;
                case '*':
                    return 3;
                    break;
                case '/':
                    return 3;
                    break;
                case '^':
                    return 4;
                    break;
                case '(':
                    return 0;
                    break;

                default:
                    return 0;
                    break;
            }
        }

        public int R(char c)
        {
            switch (c)
            {
                case '+':
                    return -1;
                    break;
                case '-':
                    return -1;
                    break;
                case '*':
                    return -1;
                    break;
                case '/':
                    return -1;
                    break;
                case '^':
                    return -1;
                    break;

                default:
                    return 0;
                    break;
            }
        }

        public string infixToPostfix()
        {
            Stack<char> stack = new();
            string result = "";
            int rank = 0;
            char x;
            this.regex.Replace(" ", String.Empty);
            bool isLastSymbol = false;
            bool isLastCloseBracket = false;
            bool isLastOpenBracket = false;
            int i = 0;
            foreach(char symbol in this.regex)
            {
                if (Char.IsLetter(symbol)){
                    if (isLastSymbol)
                    {
                        this.regex = this.regex.Insert(i, "-");
                        i++;
                        isLastSymbol = true;
                        isLastCloseBracket = false;
                        isLastOpenBracket = false;
                    }
                    else if(isLastCloseBracket)
                    {
                        this.regex = this.regex.Insert(i, "-");
                        i++;
                        isLastSymbol = true;
                        isLastCloseBracket = false;
                        isLastOpenBracket = false;
                    }
                    else
                    {
                        isLastSymbol = true;
                        isLastCloseBracket = false;
                        isLastOpenBracket = false;
                    }
                }
                else if (symbol == '(')
                {
                    if (isLastSymbol)
                    {
                        this.regex = this.regex.Insert(i, "-");
                        i++;
                        isLastSymbol = false;
                        isLastCloseBracket = false;
                        isLastOpenBracket = true;
                    }
                    else
                    {
                        isLastSymbol = false;
                        isLastCloseBracket = false;
                        isLastOpenBracket = true;
                    }
                }
                else if (symbol == ')')
                {
                    isLastCloseBracket = true;
                    isLastSymbol = false;
                    isLastOpenBracket = false;
                }
                else
                {
                    isLastSymbol = false;
                    isLastCloseBracket = false;
                    isLastOpenBracket = false;
                }
                i++;
            }
            foreach (char next in this.regex)
            {
                if (Char.IsLetter(next))
                {
                    result+=next;
                    rank += 1;
                }
                else
                {
                    while (stack.Count != 0 && (IPR(next) <= SPR(stack.Peek())))
                    {
                        x = stack.Pop();
                        result+=x;
                        rank += R(x);
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
                result+=x;  
                rank += R(x);
            }
            if (rank != 1)
            {
                throw new Exception("Nekorektan regex!");
            }
            return result;
        }

        public Stack<string> evaluatePostfix(string postfix)
        {
            Stack<string> result = new();
            bool first = true;
            Nfa newNfa = new();
            Nfa tempNfa = new();
            int i = 0;
           
           String rez = "";
           foreach(char x in postfix)
           { 
                if (Char.IsLetter(x))
                {
                    if (first)
                    {
                        newNfa.AddState("q" + i++);
                        newNfa.AddState("q" + i++);
                        newNfa.StartState = "q" + (i - 2);
                        newNfa.AddFinalState("q" + (i - 1));
                        newNfa.AddSymbolToAlphabet(x);
                        newNfa.AddTransition("q" + (i - 2), x, "q" + (i - 1));
                        first = false;
                    }
                    result.Push(x.ToString());
                } else if (x == '*')
                {
                    newNfa = newNfa.KleenovaZvijezda();
           
                    string oprnd = result.Pop();
                    rez = x.ToString() + oprnd.ToString();
                    result.Push(rez);
                } else if(x == '+')
                {
                    char symbol;
                    string oprnd2 = result.Pop();
                    string oprnd1 = result.Pop();
                    if(oprnd2.Count() == 1)
                    {
                        symbol = Char.Parse(oprnd2);
                    }
                    else
                    {
                        symbol = Char.Parse(oprnd1);
                    }
                    
                    rez = oprnd1.ToString() + x.ToString() + oprnd2.ToString();
                    tempNfa.AddState("q" + i++);
                    tempNfa.AddState("q" + i++);
                    tempNfa.StartState = "q" + (i - 2);
                    tempNfa.AddFinalState("q"+(i-1));
                    tempNfa.AddSymbolToAlphabet(symbol);
                    tempNfa.AddTransition("q" + (i - 2), symbol, "q" + (i - 1));
                    newNfa = newNfa.Unija(tempNfa);
                    result.Push(rez);
                    tempNfa.states.Clear();
                    tempNfa.finalStates.Clear();
                    tempNfa.getDelta().Clear();
                    tempNfa.alphabet.Clear();
                } else if(x == '-')
                {
                    char symbol;
                    string oprnd2 = result.Pop();
                    string oprnd1 = result.Pop();
                    if (oprnd1.Count() == 1)
                    {
                        symbol = Char.Parse(oprnd1);
                    }
                    else
                    {
                        symbol = Char.Parse(oprnd2);
                    }

                    rez = oprnd1.ToString() + x.ToString() + oprnd2.ToString();
                    tempNfa.AddState("q" + ++i);
                    tempNfa.AddState("q" + ++i);
                    tempNfa.StartState = "q" + (i - 2);
                    tempNfa.AddFinalState("q" + (i - 1));
                    tempNfa.AddSymbolToAlphabet(symbol);
                    tempNfa.AddTransition("q" + (i - 2), symbol, "q" + (i - 1));
                    newNfa = newNfa.Spajanje(tempNfa);
                    result.Push(rez);
                    tempNfa.states.Clear();
                    tempNfa.finalStates.Clear();
                    tempNfa.getDelta().Clear();
                    tempNfa.alphabet.Clear();
                }
                else
                {
                    throw new Exception("NEKOREKTAN REGEX");
                }
            }
            rez = result.Pop().ToString();
            if(result.Count != 0)
            {
                throw new Exception();
            }
            return result;

        }


        public Dfa toDfa()
        {
            Dfa newDfa = new();
            string postfix = this.infixToPostfix();
            this.evaluatePostfix(postfix);
            return newDfa; 
        }
    }

}
