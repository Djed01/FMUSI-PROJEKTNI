namespace FMUSI;

public class Nfa : Automat
{
    private readonly Dictionary<(string, char), HashSet<string>> delta = new();

    public void AddTransition(string currentState, char symbol, string nextState)
    {
        // Popunjavanje Dictionary-a
        if (!delta.ContainsKey((currentState, symbol)))
        {
            HashSet<string> set = new();
            delta.Add((currentState, symbol), set);
            delta[(currentState, symbol)].Add(nextState);
        }
        else
        {
            delta[(currentState, symbol)].Add(nextState);
        }
    }

    public bool Accepts(string input)
    {
        //Kreira se pomocni graf na osnovu kojeg pronalazimo sve epsilon prelaze
        var automatGraph = new AutomatGraph(StartState, delta);
        automatGraph.dfs(automatGraph.start);
        var eStates = automatGraph.getEStates();
        var tempSet = new HashSet<string>();
        //Za svaki simbol iz ulaza provjeravamo u koja stanja mozemo preci pocevsi od dobijenih epsilon stanja
        //U prethodnom koraku
        foreach (var symbol in input)
        {
            foreach (var state in eStates)
                if (delta.ContainsKey((state, symbol)))
                    foreach (var pom in delta[(state, symbol)])
                        //Dodajemo stanja u pomocni set
                        tempSet.Add(pom);
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
            if (finalStates.Contains(var))
                return true;

        return false;
    }

    public Nfa Unija(Nfa other)
    {
        Nfa newNfa = new();
        newNfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newNfa.states.Add(newState);

            if (finalStates.Contains(state1) || other.finalStates.Contains(state2)) newNfa.finalStates.Add(newState);

            foreach (var symbol in alphabet)
                if (delta.ContainsKey((state1, symbol)))
                    foreach (var temp1 in delta[(state1, symbol)])
                        if (other.delta.ContainsKey((state2, symbol)))
                            foreach (var temp2 in other.delta[(state2, symbol)])
                                if (!newNfa.delta.ContainsKey((newState, symbol)))
                                {
                                    HashSet<string> set = new();
                                    newNfa.delta.Add((newState, symbol), set);
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
                                else
                                {
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
        }

        return newNfa;
    }

    public Nfa Presjek(Nfa other)
    {
        Nfa newNfa = new();
        newNfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newNfa.states.Add(newState);

            if (finalStates.Contains(state1) && other.finalStates.Contains(state2)) newNfa.finalStates.Add(newState);

            foreach (var symbol in alphabet)
                if (delta.ContainsKey((state1, symbol)))
                    foreach (var temp1 in delta[(state1, symbol)])
                        if (other.delta.ContainsKey((state2, symbol)))
                            foreach (var temp2 in other.delta[(state2, symbol)])
                                if (!newNfa.delta.ContainsKey((newState, symbol)))
                                {
                                    HashSet<string> set = new();
                                    newNfa.delta.Add((newState, symbol), set);
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
                                else
                                {
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
        }

        return newNfa;
    }

    public Nfa SimetricnaRazlika(Nfa other)
    {
        Nfa newNfa = new();
        newNfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newNfa.states.Add(newState);

            if ((finalStates.Contains(state1) &&
                 other.finalStates.Contains(state2) == false) ||
                (finalStates.Contains(state1) == false && other.finalStates.Contains(state2)))
                newNfa.finalStates.Add(newState);

            foreach (var symbol in alphabet)
                if (delta.ContainsKey((state1, symbol)))
                    foreach (var temp1 in delta[(state1, symbol)])
                        if (other.delta.ContainsKey((state2, symbol)))
                            foreach (var temp2 in other.delta[(state2, symbol)])
                                if (!newNfa.delta.ContainsKey((newState, symbol)))
                                {
                                    HashSet<string> set = new();
                                    newNfa.delta.Add((newState, symbol), set);
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
                                else
                                {
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
        }

        return newNfa;
    }

    public Nfa Spajanje(Nfa other)
    {
        Nfa newNfa = new();
        newNfa.StartState = StartState + other.StartState;
        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newNfa.states.Add(newState);

            if (other.finalStates.Contains(state2)) newNfa.finalStates.Add(newState);
            foreach (var symbol in alphabet)
                if (delta.ContainsKey((state1, symbol)))
                    foreach (var temp1 in delta[(state1, symbol)])
                        if (other.delta.ContainsKey((state2, symbol)))
                            foreach (var temp2 in other.delta[(state2, symbol)])
                                if (!newNfa.delta.ContainsKey((newState, symbol)))
                                {
                                    HashSet<string> set = new();
                                    newNfa.delta.Add((newState, symbol), set);
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
                                else
                                {
                                    newNfa.delta[(newState, symbol)].Add(temp1 + temp2);
                                }
        }

        return newNfa;
    }

    public Nfa Komplement()
    {
        Nfa newNfa = new();
        newNfa.StartState = StartState;
        foreach (var state in states)
        {
            newNfa.states.Add(state);
            if (!finalStates.Contains(state)) newNfa.finalStates.Add(state);

            foreach (var symbol in alphabet)
                if (delta.ContainsKey((state, symbol)))
                    foreach (var temp in delta[(state, symbol)])
                        if (!newNfa.delta.ContainsKey((state, symbol)))
                        {
                            HashSet<string> set = new();
                            newNfa.delta.Add((state, symbol), set);
                            newNfa.delta[(state, symbol)].Add(temp);
                        }
                        else
                        {
                            newNfa.delta[(state, symbol)].Add(temp);
                        }
        }

        return newNfa;
    }
}