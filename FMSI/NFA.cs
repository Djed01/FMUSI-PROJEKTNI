namespace FMSI.Lib;

public class Nfa : Automat
{
    private Dictionary<(string, char), HashSet<string>> delta = new();
    private static int i = 0;
    private static int j = 0;
    public Dictionary<(string, char), HashSet<string>> getDelta()
    {
        return delta;
    }

    public Nfa() { }
    public Nfa(Nfa other)
    {
        this.StartState = other.StartState;
        foreach (var state1 in other.states)
        {
            this.AddState(state1);
            if (other.finalStates.Contains(state1))
            {
                this.finalStates.Add(state1);
            }
        }
        foreach (var symbol in other.alphabet)
        {
            this.AddSymbolToAlphabet(symbol);
        }
        // Popunjavamo f-ju prelaza za novi automat sa prelazima prvog automata
        foreach (var state in other.states)
        {
            foreach (var symbol in other.alphabet)
            {
                if (other.delta.ContainsKey((state, symbol)))
                {
                    foreach (var temp in other.delta[(state, symbol)])
                    {
                        if (!this.delta.ContainsKey((state, symbol)))
                        {
                            HashSet<string> set = new();
                            this.delta.Add((state, symbol), set);
                            this.delta[(state, symbol)].Add(temp);
                        }
                        else
                        {
                            this.delta[(state, symbol)].Add(temp);
                        }
                    }

                }
            }
        }
    }

    public void AddTransition(string currentState, char symbol, string nextState)
    {
        if(!this.states.Contains(currentState) || !this.states.Contains(nextState))
        {
            throw new Exception("Undefined state!");
        }
        if (!this.alphabet.Contains(symbol))
        {
            throw new Exception("Undefinded symbol!");
        }

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
        automatGraph.dfs(automatGraph.getStart());
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
                automatGraph.dfs(automatGraph.getStart());
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

    private void popuniStanjaIPrelazeAutomata(Nfa newNfa, Nfa other)
    {
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
                    foreach (var temp in delta[(state, symbol)])
                    {
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
                    foreach (var temp in other.delta[(state, symbol)])
                    {
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

                }
            }
        }
    }

    public Nfa Unija(Nfa other)
    {
        // Dodajemo jedno novo pocetno stanje i jedno zavrsno stanje
        // Te iz novokreiranog pocetnog stanja dodamo epsilon prelaze na pocetna stanja automata
        // A iz finalnih stanja automata dodamo epsilon prelaze u novo finalno stanje
        Nfa newNfa = new();
        newNfa.AddState("r" + i++);
        newNfa.AddState("f" + (i - 1));
        newNfa.StartState = "r" + (i - 1);
        newNfa.AddFinalState("f" + (i - 1));
        newNfa.AddSymbolToAlphabet(EPSILON);
        this.popuniStanjaIPrelazeAutomata(newNfa, other);

        newNfa.AddTransition("r" + (i - 1), EPSILON, this.StartState);
        newNfa.AddTransition("r" + (i - 1), EPSILON, other.StartState);
        foreach(var finalState in this.finalStates)
        {
            newNfa.AddTransition(finalState, EPSILON, "f"+(i-1));
        }
        foreach(var finalState in other.finalStates)
        {
            newNfa.AddTransition(finalState, EPSILON, "f" + (i - 1));
        }

        return newNfa;
    }

    public Nfa Unija(Dfa dfa)
    {
        return this.Unija(dfa.toNfa());
    }

    public Nfa Presjek(Nfa other)
    {
        // Pretvaramo u DKA te vrsimo operaciju
        Dfa newDfa = this.toDfa().Presjek(other.toDfa());
       // Vracamo u eNKA zboh ulancavanja operacija
        return newDfa.toNfa();
    }
    
    public Nfa Presjek(Dfa other)
    {
        Dfa newDfa = this.toDfa().Presjek(other);
        return newDfa.toNfa();
    }

    public Nfa SimetricnaRazlika(Nfa other)
    {
        // Pretvaramo u DKA te vrsimo operaciju
        Dfa newDfa = this.toDfa().SimetricnaRazlika(other.toDfa());
        // Vracamo u eNKA zbog ulancavanja operacija
        return newDfa.toNfa();
    }
    public Nfa SimetricnaRazlika(Dfa other)
    {
        Dfa newDfa = this.toDfa().SimetricnaRazlika(other);
        return newDfa.toNfa();
    }

    public Nfa Spajanje(Nfa other)
    {
        // Pravimo novi NKA automat sa pocetnim stanjem prvog automata
        Nfa newNfa = new();
        newNfa.StartState = StartState;
        newNfa.AddSymbolToAlphabet(EPSILON);
        this.popuniStanjaIPrelazeAutomata(newNfa, other);
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
        return newNfa;
    }

    public Nfa Spajanje(Dfa other)
    {
        return this.Spajanje(other.toNfa());
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
                            newNfa.AddSymbolToAlphabet(symbol);
                            HashSet<string> set = new();
                            newNfa.delta.Add((state, symbol), set);
                            newNfa.delta[(state, symbol)].Add(temp);
                        }
                        else
                        {
                            newNfa.AddSymbolToAlphabet(symbol);
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
        // Dodavanje stanja
        foreach (var state in this.states)
        {
            newNfa.AddState(state);
        }
        // Popunjavanje finalnih stanja
        foreach (var state in this.finalStates)
        {
            newNfa.AddFinalState(state);
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
                        foreach (var temp in this.delta[(state, symbol)])
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
        // Dodajemo novo stanje r
        newNfa.AddState("r"+i++);
        HashSet<string> tempSet = new();
        newNfa.delta.Add(("r"+(i-1), EPSILON), tempSet);
        // Iz novog stanja r dodajemo epsilon prelaz u pocetno stanje trenutnog automata
        newNfa.delta[("r"+(i-1), EPSILON)].Add(this.StartState);
        newNfa.StartState = "r"+(i-1);
        // Stanje r smo postavili kao pocetno stanje
        // Za svako finalno stanje automata dodajemo epsilon prelaz u novokreirano stanje r
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
                {
                    if (symbol == EPSILON) shortestPath--;
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
            }
            // Povecavamo brojac
            shortestPath++;
        }
        return -1;
    }


    public Dfa toDfa()
    {
        // Kreiramo novi DKA
        Dfa newDfa = new Dfa();
        // Dodajemo sve simbole alfabeta u novokreirani automat osim EPSILON-a
        foreach (char symbol in this.alphabet)
        {
            if (symbol != EPSILON)
                newDfa.AddSymbolToAlphabet(symbol);
        }

        // Kreiramo pomocni graf na osnovu kojeg pronalazimo
        // stanja do kojih dolazimo epsilon prelazima
        // Pocetno stanje DKA je epsilon clousure pocetnog stanja e-NKA
        var automatGraph = new AutomatGraph(this.StartState, delta);
        automatGraph.dfs(automatGraph.getStart());
        var eStates = automatGraph.getEStates();
        newDfa.StartState = String.Join("", eStates); // Spajamo HashSet stanja u string
        newDfa.AddState(String.Join("", eStates));
        HashSet<String> tempSet = new();  // Pomocni set
        HashSet<String> newEStates = new(); // Epsilon prelazi za nova stanja
        bool check = false;

        do
        {
            foreach (char symbol in newDfa.alphabet)
            {
                foreach (var state in this.states)
                {
                    if (this.delta.ContainsKey((state, symbol)))
                    {
                        // Ako dobijena stanja u DKA sadrze neko od stanja eNKA
                        // Dodajemo stanje u pomocni set za dalje trazenje epsilon prelaza
                        if (String.Join("", eStates).Contains(state))
                            foreach (var temp in this.delta[(state, symbol)])
                            {
                                tempSet.Add(temp);
                            }
                    }
                }


                foreach (var tempState in tempSet)
                {
                    // Nadjemo stanja epsilon prelaza za uniju stanja iz pomocnog seta tempSet
                    // I dodamo ih u set novih epsilon stanja
                    var tempAutomatGraph = new AutomatGraph(tempState, delta);
                    tempAutomatGraph.setStart(tempState);
                    tempAutomatGraph.dfs(tempAutomatGraph.getStart());
                    var tempEStates = tempAutomatGraph.getEStates();
                    foreach (var temp in tempEStates)
                    {
                        newEStates.Add(temp);
                    }
                }
                foreach (var finalState in this.finalStates)
                {
                    // Od seta pravimo jedno stanje konkatenacijom stringa
                    // Ako novo stanje sadrzi jedno od finalnih stanja iz eNKA
                    // Dodajemo novo stanje kao finalno u DKA
                    if (String.Join("", eStates).Contains(finalState))
                    {
                        newDfa.AddFinalState(String.Join("", eStates));
                    }
                }
                // Vrsimo konkatenaciju stanja u jedan string i dodajemo novo stanje
                newDfa.AddState(String.Join("", eStates));
                // Ukoliko je novo stanje prazan string dakle nemamo prelaza u novo stanje
                // Pravimo novo stanje qd (dead state) i vrsimo prelaz u novo stanje qd
                if ("".Equals(String.Join("", newEStates)))
                {
                    newDfa.AddState("qd");
                    newDfa.AddTransition(String.Join("", eStates), symbol, "qd");
                }
                else
                {
                    // Inace dodajemo novo stanje dobijeno konkatenacijom i dodajemo prelaz za dati simbol
                    newDfa.AddState(String.Join("", newEStates));
                    newDfa.AddTransition(String.Join("", eStates), symbol, String.Join("", newEStates));
                }
                // Brisemo sadrzaj novih stanja i pomocnog seta
                // Da bi bili prazni u narednoj iteraciji za sljedeci simbol alfabeta
                newEStates.Clear();
                tempSet.Clear();

            }
            // Brisemo sadrzaj stanja
            eStates.Clear();

            bool tempBool = false; // Pomocni bool
            // Sljedece stanje je ono za koje nismo definisali prelaz,
            // a nalazi se u skupu stanja DKA
            foreach (var key in newDfa.states)
            {
                foreach (var symbol in newDfa.alphabet)
                    if (!newDfa.getDelta().ContainsKey((key, symbol)))
                    {
                        eStates.Add(key);
                        tempBool = true;
                        break;
                    }

                if (tempBool) break;
            }
            // Ako nemamo vise novih stanja za koje nismo definisali prelaze
            // Prekidamo sa izvrsavanjem do while petlje i vracamo novokreirani DKA
            if ("".Equals(String.Join("", eStates))) { check = true; }

        } while (!check);


        return newDfa;
    }

}