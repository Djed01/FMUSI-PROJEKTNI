namespace FMUSI;

public abstract class Automat
{
    protected HashSet<char> alphabet = new();
    protected HashSet<string> finalStates = new();
    public string StartState;
    protected HashSet<string> states = new();

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
        foreach (var s in finalStates) Console.WriteLine(s);
    }

    public void PrintStates()
    {
        foreach (var s in states) Console.WriteLine(s);
    }
}