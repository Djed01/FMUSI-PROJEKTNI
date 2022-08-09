namespace FMUSI;

public class Dfa : Automat
{
    private Dictionary<(string, char), string> delta = new(); // F-JA PRELAZA IZMEDJU STANJA

    public Dictionary<(string, char), string> getDelta()
    {
        return delta;
    }
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
                {
                    newDfa.AddSymbolToAlphabet(symbol);  
                    newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
                }
        }

        return newDfa;
    }

    public Dfa SimetricnaRazlika(Nfa other)
    {
        Dfa newDfa = other.toDfa();
        return this.SimetricnaRazlika((Dfa)newDfa); 
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
                {
                    newDfa.AddSymbolToAlphabet(symbol);
                    newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
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
                    newDfa.AddSymbolToAlphabet(symbol);
                    newDfa.delta[(newState, symbol)] = delta[(state1, symbol)] + other.delta[(state2, symbol)];
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
        // Pravimo novi NKA automat sa pocetnim stanjem prvog automata
        Nfa newNfa = new();
        newNfa.StartState = StartState;
        newNfa.AddSymbolToAlphabet(EPSILON);
        // Dodajemo stanja i simbole u novi automat
        foreach (var state1 in states)
        {
            newNfa.AddState(state1);
        }
        foreach (var state2 in other.states)
        {
            newNfa.AddState(state2);
        }
        foreach (var symbol in alphabet)
        {
            newNfa.AddSymbolToAlphabet(symbol);
        }
        foreach (var symbol in other.alphabet)
        {
            newNfa.AddSymbolToAlphabet(symbol);
        }
        // Popunjavamo f-ju prelaza za novi automat sa prelazima prvog automata
        foreach (var state in states)
        {
            foreach (var symbol in alphabet)
            {
                if (delta.ContainsKey((state, symbol)))
                {
                        if (!newNfa.getDelta().ContainsKey((state, symbol)))
                        {
                            HashSet<string> set = new();
                            newNfa.getDelta().Add((state, symbol), set);
                            newNfa.getDelta()[(state, symbol)].Add(this.delta[(state, symbol)]);
                        }
                        else
                        {
                            newNfa.getDelta()[(state, symbol)].Add(this.delta[(state, symbol)]);
                        }
                }

             }
        }
        // Popunjavamo f-ju prelaza za novi automat sa prelazima drugog automata
        foreach (var state in other.states)
        {
            foreach (var symbol in other.alphabet)
            {
                if (other.delta.ContainsKey((state, symbol)))
                {
                    if (!newNfa.getDelta().ContainsKey((state, symbol)))
                    {
                        HashSet<string> set = new();
                        newNfa.getDelta().Add((state, symbol), set);
                        newNfa.getDelta()[(state, symbol)].Add(other.delta[(state, symbol)]);
                    }
                    else
                    {
                        newNfa.getDelta()[(state, symbol)].Add(other.delta[(state, symbol)]);
                    }
                }

            }
        }
        // Dodajemo finalna stanja u novi automat od drugog automata
        foreach (var finalState in other.finalStates)
        {
            newNfa.AddFinalState(finalState);
        }
        // Za svako finalno stanje prvog automata dodajemo epsilon prelaz u pocetno stanje drugog automata
        foreach (var initialFinalState in this.finalStates)
        {
            newNfa.AddTransition(initialFinalState, EPSILON, other.StartState);
        }
        // Vracamo novokreirani automat
        return newNfa.toDfa();

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
                    if (!newNfa.getDelta().ContainsKey((state, symbol)))
                    {
                        HashSet<string> set = new();
                        newNfa.getDelta().Add((state, symbol), set);
                        newNfa.getDelta()[(state, symbol)].Add(this.delta[(state,symbol)]);
                    }
                    else
                    {
                        newNfa.getDelta()[(state, symbol)].Add(this.delta[(state, symbol)]);
                    }
                }
            }
        }
       // Dodajemo novo stanje q
        newNfa.AddState("q");
        HashSet<string> tempSet = new();
        newNfa.getDelta().Add(("q", EPSILON), tempSet);
        // Iz novog stanja q dodajemo epsilon prelaz u pocetno stanje trenutnog automata
        newNfa.getDelta()[("q", EPSILON)].Add(this.StartState);
        newNfa.StartState = "q";
        // Stanje q smo postavili kao pocetno stanje
        // Za svako finalno stanje automata dodajemo epsilon prelaz u novokreirano stanje q
        foreach (var state in this.finalStates)
        {
            if (!newNfa.getDelta().ContainsKey((state, EPSILON)))
            {
                HashSet<string> set = new();
                newNfa.getDelta().Add((state, EPSILON), set);
                newNfa.getDelta()[(state, EPSILON)].Add(newNfa.StartState);
            }
            else
            {
                newNfa.getDelta()[(state, EPSILON)].Add(newNfa.StartState);
            }
        }
        // Dobijeni NKA automat pretvaramo u DKA te ga kao takvog vracamo iz metode
        // Ovim smo omogucili dalje ulancavanje operacija
        // return newNfa.toDfa();
        Dfa newDfa = newNfa.toDfa();
        return newDfa;
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