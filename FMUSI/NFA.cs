namespace FMUSI;

public class Nfa : Automat
{
    public Dictionary<(string, char), HashSet<string>> delta = new();

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
            // Ako je jedno od stanja automata finalno dodajemo novo stanje kao finalno u novom automatu
            if (finalStates.Contains(state1) || other.finalStates.Contains(state2)) newNfa.finalStates.Add(newState);

            // Popunjavamo funkciju prelaza sa novim stanjima
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
            
            //Ako su oba stanja finalna dodajemo novo stanje kao finalno u novi automat
            if (finalStates.Contains(state1) && other.finalStates.Contains(state2)) newNfa.finalStates.Add(newState);
            
            // Popunjavamo funkciju prelaza sa novim stanjima
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

            // Ako jedan automat sadrzi stanje a drugi ne sadrzi dodajemo novo stanje u novi automat
            if ((finalStates.Contains(state1) &&
                 other.finalStates.Contains(state2) == false) ||
                (finalStates.Contains(state1) == false && other.finalStates.Contains(state2)))
                newNfa.finalStates.Add(newState);
            
            // Popunjavamo funkciju prelaza sa novim stanjima
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

            // Provjeravamo da li su stanja finalna za drugi automat cija smo stanja konkatenirali na prvi
            // I ukoliko je stanje finalno dodajemo novo stanje kao finalno za novi automat
            if (other.finalStates.Contains(state2)) newNfa.finalStates.Add(newState);
            
            // Popunjavamo funkciju prelaza sa novim stanjima
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
            // Ako stanje nije finalno dodajemo ga kao finalno u novi automat
            if (!finalStates.Contains(state)) newNfa.finalStates.Add(state);
            
            // Popunjavamo funkciju prelaza sa novim stanjima
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

    public Nfa KleenovaZvijezda()
    {
        // Kreiramo novi NFA automat koji ce imati ista stanja i prelaze kao trenutni automat
        Nfa newNfa = new();
        newNfa.StartState = this.StartState;
        newNfa.AddSymbolToAlphabet(EPSILON);
        // Popunjavanje alfabeta
        foreach (var symbol in this.alphabet)
        {
            newNfa.AddSymbolToAlphabet(symbol);
        }
        // Popunjavanje finalnih stanja
        foreach (var state in this.finalStates)
        {
            newNfa.AddFinalState(state);
        }
        // Dodavanje stanja
        foreach (var state in this.states)
        {
            newNfa.AddState(state);
        }
        // Popunjavanje funkcije prelaza (delta)
        foreach (var state in this.states)
        {
            foreach (var symbol in this.alphabet)
            {
                if (this.delta.ContainsKey((state, symbol)))
                {
                    if (!newNfa.delta.ContainsKey((state, symbol)))
                    {
                        HashSet<string> set = new();
                        newNfa.delta.Add((state, symbol), set);
                        foreach(var temp in this.delta[(state, symbol)])
                        newNfa.delta[(state, symbol)].Add(temp);
                    }
                    else
                    {
                        foreach (var temp in this.delta[(state, symbol)])
                        newNfa.delta[(state, symbol)].Add(temp);
                    }
                }
            }
        }
        // Dodajemo novo stanje q
        newNfa.AddState("q");
        HashSet<string> tempSet = new();
        newNfa.delta.Add(("q", EPSILON), tempSet);
        // Iz novog stanja q dodajemo epsilon prelaz u pocetno stanje trenutnog automata
        newNfa.delta[("q", EPSILON)].Add(this.StartState);
        newNfa.StartState = "q";
        // Stanje q smo postavili kao pocetno stanje
        // Za svako finalno stanje automata dodajemo epsilon prelaz u novokreirano stanje q
        foreach (var state in this.finalStates)
        {
            if (!newNfa.delta.ContainsKey((state, EPSILON)))
            {
                HashSet<string> set = new();
                newNfa.delta.Add((state, EPSILON), set);
                newNfa.delta[(state, EPSILON)].Add(newNfa.StartState);
            }
            else
            {
                newNfa.delta[(state, EPSILON)].Add(newNfa.StartState);
            }
        }
        // Vracamo novokreirani automat
        return newNfa;
    }

    public int najkracaRijec()
    {
        int shortestPath = 0;
        Queue<string> queue = new Queue<string>();
        // Dodajemo pocetno stanje na kraj reda
        queue.Enqueue(StartState);

        while (queue.Count != 0)
        {
            // Uklanjamo stanje sa pocetka reda i dodjeljujemo ga pomocnoj promjenljivoj temp
            string temp = queue.Dequeue();

            // Za svaki simbol alfabeta automata citamo sljedece stanje i provjeravamo da li je finalno
            foreach (char symbol in alphabet)
            {
                if (delta.ContainsKey((temp, symbol)))
                    foreach (string state in delta[(temp, symbol)])
                    {
                    if (finalStates.Contains(state))
                    {
                            // Ako je finalno povecavamo brojac i vracamo vrijednost brojaca
                            shortestPath++;
                            return shortestPath;
                    }
                    else
                    {
                            // Inace stanje dodajemo na kraj reda
                            queue.Enqueue(state);
                    }
                }
            }
            // Povecavamo brojac
            shortestPath++;
        }
        return -1;
    }
}