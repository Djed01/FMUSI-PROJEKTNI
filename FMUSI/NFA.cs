namespace FMUSI;

public class Nfa
{
    public string StartState;
    private List<string> finalStates = new List<string>();
    private Dictionary<(string, char), HashSet<string>> delta = new();
    public void AddTransition(string currentState, char symbol, string nextState)
    {
        // Popunjavanje Dictionary-a
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
        //Kreira se pomocni graf na osnovu kojeg pronalazimo sve epsilon prelaze
        AutomatGraph automatGraph = new AutomatGraph(StartState, delta);
        automatGraph.dfs(automatGraph.start);
        HashSet<string> eStates = automatGraph.getEStates();
        HashSet<string> tempSet = new HashSet<string>();
        //Za svaki simbol iz ulaza provjeravamo u koja stanja mozemo preci pocevsi od dobijenih epsilon stanja
        //U prethodnom koraku
        foreach (var symbol in input)
        {
            foreach (var state in eStates)
            {
                if (delta.ContainsKey((state, symbol)))
                {
                    foreach (var pom in delta[(state,symbol)])
                    {
                        //Dodajemo stanja u pomocni set
                        tempSet.Add(pom);
                    }
                }
            }
            eStates.Clear();
            //Za svako stanje iz pomocnog seta provjeravamo u koja stanja mozemo preci epsilon prelazima
            foreach (var var in tempSet)
            {
                automatGraph.setStart(var);
                automatGraph.dfs(automatGraph.start);
                eStates = automatGraph.getEStates();
            }
            tempSet.Clear();
        }
        
        //Nakon sto prodjemo kroz sve simbole provjeravamo da li smo na kraju stigli do finalnih stanja
        foreach (var var in eStates)
        {
            if (finalStates.Contains(var))
            {
                return true;
            }
        }

        return false;
    }

 
    
    
    
    
}