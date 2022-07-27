using FMUSI;

;

Dfa dfa = new();
dfa.StartState = "q0";
Console.WriteLine(dfa.StartState);

dfa.AddTransition("q0", 'a', "q1");
dfa.AddTransition("q1", 'b', "q2");
dfa.AddTransition("q2", 'b', "q3");
dfa.AddFinalState("q3");
Console.WriteLine(dfa.Accepts("abb"));

Nfa nfa = new();
nfa.StartState = "q0";
nfa.AddSymbolToAlphabet('a');
nfa.AddSymbolToAlphabet('b');
nfa.AddSymbolToAlphabet('$');
nfa.AddState("q0");
nfa.AddState("q1");
nfa.AddState("q2");
nfa.AddState("q3");
nfa.AddState("q4");
nfa.AddState("q5");
nfa.AddTransition("q0", 'a', "q1");
nfa.AddTransition("q1", 'b', "q2");
nfa.AddTransition("q1", '$', "q3");
nfa.AddTransition("q3", 'b', "q4");
nfa.AddTransition("q3", 'b', "q3");
nfa.AddTransition("q4", 'a', "q5");
nfa.AddTransition("q5", 'b', "q3");
nfa.AddFinalState("q3");
Console.WriteLine(nfa.Accepts("abab"));

Nfa dfa1 = new();
dfa1.StartState = "q0";
dfa1.AddFinalState("q1");
dfa1.AddSymbolToAlphabet('a');
dfa1.AddSymbolToAlphabet('b');
dfa1.AddState("q0");
dfa1.AddState("q1");

Nfa dfa2 = new();
dfa2.StartState = "p0";
dfa2.AddFinalState("p0");
dfa2.AddSymbolToAlphabet('a');
dfa2.AddSymbolToAlphabet('b');
dfa2.AddState("p0");
dfa2.AddState("p1");

dfa1.AddTransition("q0", 'b', "q0");
dfa1.AddTransition("q0", 'a', "q1");
dfa1.AddTransition("q1", 'b', "q1");
dfa1.AddTransition("q1", 'a', "q1");

dfa2.AddTransition("p0", 'b', "p0");
dfa2.AddTransition("p0", 'a', "p1");
dfa2.AddTransition("p1", 'a', "p0");
dfa2.AddTransition("p1", 'b', "p1");

var novi = dfa1.Unija(nfa).Komplement().Spajanje(dfa2);
novi.printFinalStates();
Console.Write("==============================\n");
novi.PrintStates();