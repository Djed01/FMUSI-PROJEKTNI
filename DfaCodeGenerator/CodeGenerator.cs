using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMSI.Lib;

namespace DfaCodeGenerator
{
    internal class CodeGenerator
    {
        
        private static string deadState = "DEADSTATE";
        public void generate(Dfa dfa)
        {
            // Dodajemo dead state u automat i za svako stanje, ukoliko nema prelaz za neki simbol
            // iz alfabeta, dodajemo prelaz u dead state
            // Ovo radimo kako bismo imali uvijek isti broj argumenata u funkcijama (svako stanje za svaki simbol iz alfabeta ima prelaz u drugo stanje) 
            dfa.AddState(deadState);
            foreach(var state in dfa.states)
            {
                foreach(var symbol in dfa.alphabet)
                {
                    if (!dfa.getDelta().ContainsKey((state, symbol)))
                    {
                        dfa.AddTransition(state, symbol, deadState);
                    }
                }
            }

            // Kreiramo klasu Specification koja kao atribut ima set akcija (funkcija)
            // I nekoliko pomocnih funkcija za dodavanje, uklanjanje i izvrsavanje akcija
            // Action je ekvivalentan (ekapsulira) metodu sa jednim parametrom koja ne vraca vrijednost
            StringBuilder code = new StringBuilder();
            code.Append("using System;\n" +
                "using System.Collections.Generic;\n\n" +
                "public class Specification{\n\n" +
                "private HashSet<Action<string>> actions = new HashSet<Action<string>>();\n\n" +
                "public void addAction(Action<string> action){\n" +
                "actions.Add(action);\n}\n\n" +
                "public void removeAction(Action<string> action){\n" +
                "actions.Remove(action);\n}\n\n" +
                "public void clear(){\n" +
                "actions.Clear();\n}\n\n" +
                "public void doStateActions(string state){\n" +
                "foreach(var action in actions){\n" +
                "action.Invoke(state);\n}\n}\n}\n\n");

            code.Append("public class Automat{\n\n");

            foreach(var state in dfa.states)
            {
                // Kreiramo funkcije prelaza za svako stanje automata
                int j = 0;
                code.Append("private string switch" + state + "(");
                for(int i=0; i<dfa.states.Count(); i++) { code.Append("Specification spec" + i + ", "); }
                code.Append("char symbol){\nstring currentState = null;\n" +
                    "switch(symbol){\n");
                foreach(var symbol in dfa.alphabet)
                {
                    if (dfa.getDelta().ContainsKey((state, symbol)))
                    {
                        code.Append("case '" + symbol + "':\n" +
                            "currentState = \"" + dfa.getDelta()[(state, symbol)] + "\";\n");
                    }
                    else
                    {
                        throw new Exception();
                    }
                    code.Append("spec" + j + ".doStateActions(currentState);\n" +
                        "break;\n");
                    j++;
                }
                code.Append("default:\nthrow new Exception();\n}\nreturn currentState;\n}\n\n");
            }

            // Ulancavanje operacija podrazumjeva prolazak petljom, za neku rijec (alphabet), kroz stanja
            // Pri cemu se pozivaju f-je pri ulasku i izlasku iz stanja
            code.Append("public void chainReaction(Specification input, Specification output,");
            for(int i=0; i < dfa.states.Count(); i++) { code.Append("Specification spec" + i + ", "); }
            code.Append("HashSet<char> alphabet){\n");
            code.Append("string initState = \"" + dfa.StartState + "\";\n");
            code.Append("foreach(var symbol in alphabet){\n\n");
            foreach(var state in dfa.states)
            {
                code.Append("if(initState == \"" + state + "\"){\n");
                code.Append("output.doStateActions(initState);\n");
                code.Append("initState = switch" + state + "(");
                for(int i = 0; i < dfa.states.Count(); i++) { code.Append("spec" + i + ", "); }
                code.Append("symbol);\n");
                code.Append("input.doStateActions(initState);\n}\n\n");
            }
            code.Append("}\n}\n}\n");

           File.WriteAllTextAsync("GeneratedCode.cs", code.ToString());
        }
    }
}
