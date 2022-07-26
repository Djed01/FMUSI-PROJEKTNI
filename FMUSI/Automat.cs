namespace FMUSI;

public abstract class Automat
{
    protected HashSet<string> finalStates = new();
    protected HashSet<string> states = new();
    protected HashSet<char> alphabet = new();
    public string StartState;
    
    public void AddSymbolToAlphabet(char symbol)
    {
        alphabet.Add(symbol);
    }
    
    public void AddState(string state)
    {
        states.Add(state);
    }
    
    public void AddFinalState(string state)
    {
        finalStates.Add(state);
    }
    
    public void printFinalStates()
    {
        foreach (var s in finalStates)
        {
            Console.WriteLine(s);
        }
    }
    
    public void PrintStates()
    {
        foreach (var s in states)
        {
            Console.WriteLine(s);
        }
    }
    
}