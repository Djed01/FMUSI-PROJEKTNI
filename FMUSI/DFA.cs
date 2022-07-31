namespace FMUSI;

public class Dfa : Automat
{
    public Dictionary<(string, char), string> delta = new(); // F-JA PRELAZA IZMEDJU STANJA

    public void AddTransition(string currentState, char symbol, string nextState)
    {
        delta[(currentState, symbol)] = nextState;
    }

    public bool Accepts(string input)
    {
        var currentState = StartState;
        foreach (var symbol in input)
            try
            {
                currentState = delta[(currentState, symbol)];
            }
            catch (KeyNotFoundException e)
                //Ukoliko je prosledjen neodgovarajuci string
            {
                return false;
            }

        return finalStates.Contains(currentState);
    }

    public Dfa SimetricnaRazlika(Dfa other)
    {
        Dfa newDfa = new();

        newDfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newDfa.states.Add(newState);
            
            // Ako jedan automat sadrzi stanje a drugi ne sadrzi dodajemo novo stanje u novi automat
            if ((finalStates.Contains(state1) &&
                 other.finalStates.Contains(state2) == false) ||
                (finalStates.Contains(state1) == false && other.finalStates.Contains(state2)))
                newDfa.finalStates.Add(newState);

            // Popunjavamo funkciju prelaza sa novim stanjima
            foreach (var symbol in alphabet)
                newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
        }

        return newDfa;
    }

    public Dfa Unija(Dfa other)
    {
        Dfa newDfa = new();
        newDfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newDfa.states.Add(newState);

            // Ako je jedno od stanja automata finalno dodajemo novo stanje kao finalno u novom automatu
            if (finalStates.Contains(state1) || other.finalStates.Contains(state2)) newDfa.finalStates.Add(newState);

            // Popunjavamo funkciju prelaza sa novim stanjima
            foreach (var symbol in alphabet)
                newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
        }

        return newDfa;
    }

    public Dfa Presjek(Dfa other)
    {
        Dfa newDfa = new();
        newDfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newDfa.states.Add(newState);

            //Ako su oba stanja finalna dodajemo novo stanje kao finalno u novi automat
            if (finalStates.Contains(state1) && other.finalStates.Contains(state2)) newDfa.finalStates.Add(newState);

            // Popunjavamo funkciju prelaza sa novim stanjima
            foreach (var symbol in alphabet)
                newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
        }

        return newDfa;
    }

    public Dfa Spajanje(Dfa other)
    {
        Dfa newDfa = new();
        newDfa.StartState = StartState + other.StartState;
        foreach (var state1 in states)
        foreach (var state2 in other.states)
        {
            var newState = state1 + state2;
            newDfa.states.Add(newState);
            
            // Provjeravamo da li su stanja finalna za drugi automat cija smo stanja konkatenirali na prvi
            // I ukoliko je stanje finalno dodajemo novo stanje kao finalno za novi automat
            if (other.finalStates.Contains(state2)) newDfa.finalStates.Add(newState);

            // Popunjavamo funkciju prelaza sa novim stanjima
            foreach (var symbol in alphabet)
                newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
        }

        return newDfa;
    }

    public Dfa Komplement()
    {
        Dfa newDfa = new();
        newDfa.StartState = StartState;
        foreach (var state in states)
        {
            newDfa.states.Add(state);
            // Ako stanje nije finalno dodajemo ga kao finalno u novi automat
            if (!finalStates.Contains(state)) newDfa.finalStates.Add(state);
            
            // Popunjavamo funkciju prelaza sa novim stanjima
            foreach (var symbol in alphabet) newDfa.delta[(state, symbol)] = delta[(state, symbol)];
        }

        return newDfa;
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
        foreach(var state in this.finalStates)
        {
            newNfa.AddFinalState(state);
        }
        // Dodavanje stanja
        foreach(var state in this.states)
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
                        newNfa.delta[(state, symbol)].Add(this.delta[(state,symbol)]);
                    }
                    else
                    {
                        newNfa.delta[(state, symbol)].Add(this.delta[(state, symbol)]);
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
        // Dobijeni NKA automat pretvaramo u DKA te ga kao takvog vracamo iz metode
        // Ovim smo omogucili dalje ulancavanje operacija
        // return newNfa.toDfa();
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
            foreach(char symbol in alphabet)
            {
                string nextState = delta[(temp, symbol)];
                if (finalStates.Contains(nextState))
                {
                    // Ako je finalno povecavamo brojac i vracamo vrijednost brojaca
                    shortestPath++;
                    return shortestPath;
                }
                else
                {
                    // Inace stanje dodajemo na kraj reda
                    queue.Enqueue(nextState);
                }
            }
            // Povecavamo brojac
            shortestPath++;
        }
        return -1;
    }
}