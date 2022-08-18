using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMSI.Lib;

namespace ProjectTests
{
    public class AutomatGraphTest
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void eStatesTest()
        {
            Nfa nfa = new();

            nfa.StartState = "q0";

            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddState("q3");

            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');

            nfa.AddTransition("q0", 'a', "q1");
            nfa.AddTransition("q0", '$', "q1");
            nfa.AddTransition("q0", 'b', "q2");
            nfa.AddTransition("q1", 'b', "q3");
            nfa.AddTransition("q1", '$', "q2");
            nfa.AddTransition("q2", 'a', "q3");

            nfa.AddFinalState("q3");

            var automatGraph = new AutomatGraph(nfa.StartState, nfa.getDelta());
            automatGraph.dfs(automatGraph.getStart());
            var eStates = automatGraph.getEStates();
            Assert.AreEqual(3, eStates.Count);

            var automatGraph2 = new AutomatGraph("q1", nfa.getDelta());
            automatGraph2.dfs(automatGraph2.getStart());
            var eStates2 = automatGraph2.getEStates();
            Assert.AreEqual(2, eStates2.Count);

        }
    }
}
