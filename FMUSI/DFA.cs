namespace FMUSI;

public class Dfa : Automat
{
    private readonly Dictionary<(string, char), string> delta = new(); // F-JA PRELAZA IZMEDJU STANJA

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
}