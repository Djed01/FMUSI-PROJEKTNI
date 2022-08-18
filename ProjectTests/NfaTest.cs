using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMSI.Lib;

namespace ProjectTests
{
    public class NfaTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AcceptsTest()
        {
            Nfa nfa = new();
            nfa.StartState = "q0";
            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddTransition("q0", 'b', "q1");
            nfa.AddTransition("q0", '$', "q2");
            nfa.AddTransition("q1", 'a', "q1");
            nfa.AddTransition("q1", 'a', "q2");
            nfa.AddTransition("q1", 'b', "q2");
            nfa.AddTransition("q2", 'a', "q0");

            nfa.AddFinalState("q0");

            Assert.IsTrue(nfa.Accepts("a"));
            Assert.IsTrue(nfa.Accepts("baaba"));
            Assert.IsFalse(nfa.Accepts("ba"));
            Assert.IsFalse(nfa.Accepts("aba"));
        }

        [Test]
        public void UnijaTest()
        {
            Nfa nfa1 = new();
            Nfa nfa2 = new();

            nfa1.StartState = "q0";
            nfa1.AddState("q0");
            nfa1.AddState("q1");
            nfa1.AddSymbolToAlphabet('a');
            nfa1.AddSymbolToAlphabet('$');
            nfa1.AddTransition("q0", 'a', "q1");
            nfa1.AddTransition("q0", '$', "q1");
            nfa1.AddFinalState("q1");

            nfa2.StartState = "p0";
            nfa2.AddState("p0");
            nfa2.AddState("p1");
            nfa2.AddSymbolToAlphabet('b');
            nfa2.AddTransition("p0", 'b', "p1");
            nfa2.AddFinalState("p0");

            Nfa newNfa = nfa1.Unija(nfa2);

            Assert.IsTrue(newNfa.Accepts(""));
            Assert.IsTrue(newNfa.Accepts("a"));
            Assert.IsFalse(newNfa.Accepts("b"));
        }

        [Test]
        public void SpajanjeTest()
        {
            Nfa nfa1 = new();
            Nfa nfa2 = new();

            nfa1.StartState = "q0";
            nfa1.AddState("q0");
            nfa1.AddState("q1");
            nfa1.AddSymbolToAlphabet('a');
            nfa1.AddTransition("q0", 'a', "q1");
            nfa1.AddFinalState("q1");

            nfa2.StartState = "p0";
            nfa2.AddState("p0");
            nfa2.AddState("p1");
            nfa2.AddSymbolToAlphabet('b');
            nfa2.AddTransition("p0", 'b', "p1");
            nfa2.AddFinalState("p1");

            Nfa newNfa = nfa1.Spajanje(nfa2);
            Assert.IsTrue(newNfa.Accepts("ab"));
            Assert.IsFalse(newNfa.Accepts("a"));
        }

        [Test]
        public void KomplementTest()
        {
            Nfa nfa = new();
            nfa.StartState = "q0";
            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddTransition("q0", 'b', "q1");
            nfa.AddTransition("q0", '$', "q2");
            nfa.AddTransition("q1", 'a', "q1");
            nfa.AddTransition("q1", 'a', "q2");
            nfa.AddTransition("q1", 'b', "q2");
            nfa.AddTransition("q2", 'a', "q0");

            nfa.AddFinalState("q0");

            Nfa newNfa = nfa.Komplement();

            Assert.IsTrue(newNfa.Accepts("b"));
            Assert.IsTrue(newNfa.Accepts("ba"));
            Assert.IsTrue(newNfa.Accepts("aba"));
        }

        [Test]
        public void KleeneTest()
        {
            Nfa nfa = new();

            nfa.StartState = "q0";
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');

            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");

            nfa.AddTransition("q0", 'a', "q1");
            nfa.AddTransition("q0", '$', "q1");
            nfa.AddTransition("q1", 'b', "q2");

            nfa.AddFinalState("q2");

            Nfa newNfa = nfa.KleenovaZvijezda();
            Assert.IsTrue(newNfa.Accepts("abbabbab"));
            Assert.IsFalse(newNfa.Accepts("abba"));
        }

        [Test]
        public void najkracaRijecTest()
        {
            Nfa nfa = new();

            nfa.StartState = "q0";
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');

            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddState("q3");

            nfa.AddTransition("q0", 'a', "q1");
            nfa.AddTransition("q0", '$', "q1");
            nfa.AddTransition("q1", '$', "q2");
            nfa.AddTransition("q2", 'a', "q3");

            nfa.AddFinalState("q3");
            Assert.That(nfa.najkracaRijec(), Is.EqualTo(1));
        }

        [Test]
        public void toDfaTest()
        {
            Nfa nfa = new();
            nfa.StartState = "q0";
            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddTransition("q0", 'b', "q1");
            nfa.AddTransition("q0", '$', "q2");
            nfa.AddTransition("q1", 'a', "q1");
            nfa.AddTransition("q1", 'a', "q2");
            nfa.AddTransition("q1", 'b', "q2");
            nfa.AddTransition("q2", 'a', "q0");

            nfa.AddFinalState("q0");

            Dfa newDfa = nfa.toDfa();

            Assert.IsTrue(newDfa.Accepts("a"));
            Assert.IsTrue(newDfa.Accepts("baba"));
            Assert.IsFalse(newDfa.Accepts("aab"));
        }

        [Test]
        public void spajanjeSaDfaTest()
        {
            Nfa nfa = new();
            nfa.StartState = "q0";
            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddTransition("q0", 'b', "q1");
            nfa.AddTransition("q0", '$', "q2");
            nfa.AddTransition("q1", 'a', "q1");
            nfa.AddTransition("q1", 'a', "q2");
            nfa.AddTransition("q1", 'b', "q2");
            nfa.AddTransition("q2", 'a', "q0");
            nfa.AddFinalState("q0");

            Dfa dfa = new();
            dfa.StartState = "p0";
            dfa.AddState("p0");
            dfa.AddState("p1");
            dfa.AddSymbolToAlphabet('c');
            dfa.AddTransition("p0", 'c', "p1");
            dfa.AddFinalState("p1");

            Nfa newNfa = nfa.Spajanje(dfa);

            Assert.IsTrue(newNfa.Accepts("baac"));
            Assert.IsFalse(newNfa.Accepts("baa"));

        }

        [Test]
        public void unijaSaDfaTest()
        {
            Nfa nfa = new();
            nfa.StartState = "q0";
            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddTransition("q0", 'b', "q1");
            nfa.AddTransition("q0", '$', "q2");
            nfa.AddTransition("q1", 'a', "q1");
            nfa.AddTransition("q1", 'a', "q2");
            nfa.AddTransition("q1", 'b', "q2");
            nfa.AddTransition("q2", 'a', "q0");
            nfa.AddFinalState("q0");

            Dfa dfa = new();
            dfa.StartState = "p0";
            dfa.AddState("p0");
            dfa.AddState("p1");
            dfa.AddSymbolToAlphabet('c');
            dfa.AddTransition("p0", 'c', "p1");
            dfa.AddFinalState("p1");

            Nfa newNfa = nfa.Unija(dfa);

            Assert.IsTrue(newNfa.Accepts("baa"));
            Assert.IsFalse(newNfa.Accepts("baac"));
            Assert.IsTrue(newNfa.Accepts("c"));
        }
    }
}
