namespace FMSI.Lib;

public class Dfa : Automat
{
    private Dictionary<(string, char), string> delta = new(); // F-JA PRELAZA IZMEDJU STANJA
    private static string DEADSTATE = "qd";
    public Dictionary<(string, char), string> getDelta()
    {
        return delta;
    }

    public void AddTransition(string currentState, char symbol, string nextState)
    {
        if (!(this.states.Contains(currentState)) || !(this.states.Contains(nextState)))
        {
            throw new Exception("Undefined states!"); // STANJE NIJE DODANO U SKUP
        }
        if (!this.alphabet.Contains(symbol))
        {
            throw new Exception("Undefined symbol!"); // SIMBOL NIJE DODAT U SKUP
        }
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
            catch (KeyNotFoundException)
            //Ukoliko je prosledjen neodgovarajuci string
            {
                return false;
            }

        return finalStates.Contains(currentState);
    }

    private void popuniDelta(string state1,char symbol,string state2,string newState, Dfa other, Dfa newDfa)
    {
        newDfa.AddSymbolToAlphabet(symbol);
        // Cetiri slucaja
        if (delta.ContainsKey((state1, symbol)) && !(other.delta.ContainsKey((state2, symbol)))) // Prvi ima prelaz, drugi nema
        {
            newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + DEADSTATE;
        }
        else if (!delta.ContainsKey((state1, symbol)) && (other.delta.ContainsKey((state2, symbol)))) // Drugi ima prelaz, prvi nema
        {
            newDfa.delta[(newState, symbol)] = DEADSTATE + other.delta[(state2, symbol)];
        }
        else if (!delta.ContainsKey((state1, symbol)) && (!other.delta.ContainsKey((state2, symbol)))) // Oba nemaju
        {
            newDfa.delta[(newState, symbol)] = DEADSTATE + DEADSTATE;
        }
        else // Oba imaju prelaz
        {
            newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
        }
    }

    public Dfa SimetricnaRazlika(Dfa other)
    {
        // PROVJERA DA LI SU ALFABETI JEDNAKI
        if (!this.alphabet.SetEquals(other.alphabet))
        {
            throw new Exception("Alphabet is not the same in both!");
        }

        Dfa newDfa = new();

        newDfa.StartState = StartState + other.StartState;

        foreach (var state1 in states)
            foreach (var state2 in other.states)
            {
                var newState = state1 + state2;
                newDfa.states.Add(newState);

                // Ako jedan automat sadrzi stanje a drugi ne sadrzi dodajemo novo stanje u novi automat kao finalno
                if ((finalStates.Contains(state1) &&
                     other.finalStates.Contains(state2) == false) ||
                    (finalStates.Contains(state1) == false && other.finalStates.Contains(state2)))
                    newDfa.finalStates.Add(newState);

                // Popunjavamo funkciju prelaza sa novim stanjima
                foreach (var symbol in alphabet)
                {
                    popuniDelta(state1, symbol, state2, newState, other, newDfa);
                }
            }

        return newDfa;
    }

    public Dfa SimetricnaRazlika(Nfa other)
    {
        return this.SimetricnaRazlika(other.toDfa());
    }

    public Dfa Unija(Dfa other)
    {
        // PROVJERA DA LI SU ALFABETI JEDNAKI
        if (!this.alphabet.SetEquals(other.alphabet))
        {
            throw new Exception("Alphabet is not the same in both!");
        }
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
                {
                    popuniDelta(state1, symbol, state2, newState, other, newDfa);
                }
            }

        return newDfa;
    }

    public Dfa Unija(Nfa other)
    {
        return this.Unija((Dfa)other.toDfa());
    }

    public Dfa Presjek(Dfa other)
    {
        // PROVJERA DA LI SU ALFABETI JEDNAKI
        if (!this.alphabet.SetEquals(other.alphabet))
        {
            throw new Exception("Alphabet is not the same in both!");
        }
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
                {
                    popuniDelta(state1, symbol, state2, newState, other, newDfa);
                }
            }

        return newDfa;
    }

    public Dfa Presjek(Nfa other)
    {
        return this.Presjek(other.toDfa());
    }

    public Dfa Spajanje(Dfa other)
    {
        return this.toNfa().Spajanje(other.toNfa()).toDfa();
    }

    public Dfa Spajanje(Nfa other)
    {
        return this.Spajanje(other.toDfa());
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
            foreach (var symbol in alphabet)
            {
                newDfa.AddSymbolToAlphabet(symbol);
                newDfa.delta[(state, symbol)] = delta[(state, symbol)];
            }
        }

        return newDfa;
    }

    public Dfa KleenovaZvijezda()
    {
        return this.toNfa().KleenovaZvijezda().toDfa(); // Pretvaramo u NKA vrsimo operaciju Kleenove zvijezde i vracamo DKA
    }


    public int najkracaRijec()
    {
        if (this.finalStates.Count == 0)
        {
            throw new Exception("No final state");
        }
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

    public Nfa toNfa()
    {
        // Prekopiramo sva stanja i prelaze u novokreirani NFA
        Nfa newNfa = new();
        newNfa.StartState = this.StartState;
        foreach (var symbol in this.alphabet)
        {
            newNfa.AddSymbolToAlphabet(symbol);
        }
        foreach (var state in this.states)
        {
            newNfa.AddState(state);
            if (this.finalStates.Contains(state))
            {
                newNfa.AddFinalState(state);
            }
        }
        foreach (var state in this.states)
        {
            foreach (var symbol in this.alphabet)
            {
                if (this.getDelta().ContainsKey((state, symbol)))
                {
                    newNfa.AddTransition(state, symbol, this.getDelta()[(state, symbol)]);
                }
            }
        }
        return newNfa;
    }

}