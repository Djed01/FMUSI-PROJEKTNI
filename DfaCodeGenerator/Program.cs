using FMSI.Lib;
using DfaCodeGenerator;

Dfa dfa = new();

dfa.StartState = "q0";

dfa.AddState("q0");
dfa.AddState("q1");

dfa.AddSymbolToAlphabet('a');
dfa.AddSymbolToAlphabet('b');

dfa.AddTransition("q0", 'a', "q1");
dfa.AddTransition("q1", 'b', "q0");
dfa.AddFinalState("q1");

CodeGenerator codeGenerator = new();
codeGenerator.generate(dfa);