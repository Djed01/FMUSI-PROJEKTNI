namespace FMUSI;

public class Dfa
{
    private Dictionary<(string, char), string> delta = new(); // F-JA PRELAZA IZMEDJU STANJA
    private HashSet<string> finalStates = new();

    public string StartState;

    public void AddTransition(string currentState, char symbol, string nextState)
    {
        delta[(currentState, symbol)] = nextState;
    }

    public void AddFinalState(string state)
    {
        finalStates.Add(state);
    }

    public bool Accepts(string input)
    {
        var currentState = StartState;
        foreach (var symbol in input)
        {
            try
            {
                currentState = delta[(currentState, symbol)];
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            //Ukoliko je prosledjen neodgovarajuci string
            {
                return false;
            }
        }

        return finalStates.Contains(currentState);
    }
    
}