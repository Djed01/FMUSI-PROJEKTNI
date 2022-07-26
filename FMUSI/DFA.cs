namespace FMUSI;

public class Dfa : Automat
{
    private Dictionary<(string, char), string> delta = new(); // F-JA PRELAZA IZMEDJU STANJA
    
    public void AddTransition(string currentState, char symbol, string nextState)
    {
        delta[(currentState, symbol)] = nextState;
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
    public Dfa SimetricnaRazlika(Dfa other)
    {
        Dfa newDfa = new();
            
        newDfa.StartState = this.StartState + other.StartState;

        foreach (var state1 in this.states)
        {
            foreach (var state2 in other.states)
            {
                string newState = state1 + state2;
                newDfa.states.Add(newState);

                if (((this.finalStates.Contains(state1) == true) &&
                     (other.finalStates.Contains(state2) == false)) ||
                    ((this.finalStates.Contains(state1) == false) && (other.finalStates.Contains(state2) == true)))
                {
                    newDfa.finalStates.Add(newState);
                }

                foreach (var symbol in alphabet)
                {
                    newDfa.delta[(newState, symbol)] = this.delta[(state1, symbol)] + other.delta[(state2, symbol)];
                }
            }
        }

        return newDfa;
    }

    public Dfa Unija(Dfa other)
    {
        Dfa newDfa = new();
        newDfa.StartState = this.StartState + other.StartState;

        foreach (var state1 in this.states)
        {
            foreach (var state2 in other.states)
            {
                string newState = state1 + state2;
                newDfa.states.Add(newState);

                if ((this.finalStates.Contains(state1) == true) || (other.finalStates.Contains(state2) == true))
                {
                    newDfa.finalStates.Add(newState);
                }

                foreach (var symbol in alphabet)
                {
                    newDfa.delta[(newState, symbol)] = this.delta[(state1, symbol)] + other.delta[(state2, symbol)];
                }
            }
        }
        return newDfa;
    }
    
    public Dfa Presjek(Dfa other)
    {
        Dfa newDfa = new();
        newDfa.StartState = this.StartState + other.StartState;

        foreach (var state1 in this.states)
        {
            foreach (var state2 in other.states)
            {
                string newState = state1 + state2;
                newDfa.states.Add(newState);

                if ((this.finalStates.Contains(state1) == true) && (other.finalStates.Contains(state2) == true))
                {
                    newDfa.finalStates.Add(newState);
                }

                foreach (var symbol in alphabet)
                {
                    newDfa.delta[(newState, symbol)] = this.delta[(state1, symbol)] + other.delta[(state2, symbol)];
                }
            }
        }
        return newDfa;
    }

    public Dfa Spajanje(Dfa other)
    {
        Dfa newDfa = new();
        newDfa.StartState = this.StartState + other.StartState;
        foreach (var state1 in this.states)
        {
            foreach (var state2 in other.states)
            {
                string newState = state1 + state2;
                newDfa.states.Add(newState);
                if (other.finalStates.Contains(state2))
                {
                    newDfa.finalStates.Add(newState);
                }

                foreach (var symbol in alphabet)
                {
                    newDfa.delta[(newState, symbol)] = this.delta[(state1, symbol)] + other.delta[(state2, symbol)];
                }
            }
        }
        return newDfa;
    }

    public Dfa Komplement()
    {
        Dfa newDfa = new();
        newDfa.StartState = this.StartState;
        foreach (var state in this.states)
        {
            newDfa.states.Add(state);
            if (!this.finalStates.Contains(state))
            {
                newDfa.finalStates.Add(state);
            }

            foreach (var symbol in alphabet)
            {
                newDfa.delta[(state, symbol)] = this.delta[(state, symbol)];
            }
        }
        return newDfa;
    }
    
    
   
}