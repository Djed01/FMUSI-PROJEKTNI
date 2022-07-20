namespace FMUSI;

public class Nfa
{
    private string StartState;
    private List<string> finalStates = new List<string>();
    private Dictionary<(string, char), HashSet<string>> delta = new();
    public void AddTransition(string currentState, char symbol, string nextState)
    {
        if (!delta.ContainsKey((currentState, symbol)))
        {
            HashSet<string> set = new();
            delta.Add((currentState,symbol),set);
            delta[(currentState, symbol)].Add(nextState);
        }
        else
        {
            delta[(currentState, symbol)].Add(nextState);
        }
    }
    
    public void AddFinalState(string state)
    {
        finalStates.Add(state);
    }

    public bool Accepts(string input)
    {
        int n = delta.Count;
        Console.WriteLine(n);
        string[,] matricaSusjednosti=new string[n,n];
        return true;
    }

 
    
    
    
    
}