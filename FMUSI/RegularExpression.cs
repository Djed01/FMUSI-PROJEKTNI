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
            foreach (char next in this.regex.Replace(" ",String.Empty))
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
            Nfa newNfa = new();
            Nfa tempNfa = new();
            newNfa.StartState = "q0";
            newNfa.AddTransition("q0", EPSILON, "q0");
            newNfa.AddSymbolToAlphabet(EPSILON);
            int i = 0;
            bool lastIsLetter = false;
           
           String rez = "";
           foreach(char x in postfix)
            { 
                if (Char.IsLetter(x))
                {
                    tempNfa.AddState("q" + ++i);
                    tempNfa.AddState("q" + ++i);
                    tempNfa.AddTransition("q" + (i - 2), x, "q" + (i - 1));
                    tempNfa.AddSymbolToAlphabet(x);
                    newNfa.Spajanje(tempNfa);
                    result.Push(x.ToString());
                } else if (x == '*')
                {
                    newNfa.KleenovaZvijezda();
                    tempNfa = newNfa; // !!!
                    string oprnd = result.Pop();
                    rez = x.ToString() + oprnd.ToString();
                    result.Push(rez);
                } else if( x == '+')
                {
                    
                    string oprnd2 = result.Pop();
                    string oprnd1 = result.Pop();
                    rez = oprnd1.ToString() + x.ToString() + oprnd2.ToString();
                    tempNfa.AddState("q" + ++i);
                    tempNfa.AddState("q" + ++i);
                    tempNfa.AddTransition("q" + (i - 2), x, "q" + (i - 1));
                    newNfa.Unija(tempNfa);
                    result.Push(rez);
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
