using FMUSI;;

Dfa dfa = new();
dfa.StartState = "q0";
Console.WriteLine(dfa.StartState);

dfa.AddTransition("q0",'a',"q1");
dfa.AddTransition("q1",'b',"q2");
dfa.AddTransition("q2",'b',"q3");
dfa.AddFinalState("q3");
Console.WriteLine(dfa.Accepts("abb"));

Nfa nfa = new();
nfa.AddTransition("q0",'a',"q1");
nfa.AddTransition("q0",'a',"q2");
nfa.AddTransition("q1",'b',"q2");
nfa.AddTransition("q0",'b',"q3");
nfa.AddTransition("q1",'b',"q3");
nfa.AddTransition("q3",'c',"q1");
nfa.Accepts("test");