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
            Assert.IsFalse(newNfa.Accepts(""));
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
            Assert.IsTrue(newNfa.Accepts(""));
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
            Assert.IsTrue(newNfa.Accepts("babab"));
            Assert.IsFalse(newNfa.Accepts("abba"));
            Assert.IsFalse(newNfa.Accepts("baba"));
        }

        [Test]
        public void najkracaRijecTest()
        {
            Nfa nfa = new();

            nfa.StartState = "q0";
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('$');

            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddState("q3");

            nfa.AddTransition("q0", 'a', "q1");
            nfa.AddTransition("q0", '$', "q1");
            nfa.AddTransition("q1", '$', "q2");
            nfa.AddTransition("q2", 'a', "q3");

            Assert.Throws<Exception>(() => nfa.najkracaRijec());

            nfa.AddFinalState("q3");
            Assert.That(nfa.najkracaRijec(), Is.EqualTo(1));

            Nfa nfa2 = new();
            nfa2.StartState = "q0";
            nfa2.AddSymbolToAlphabet('a');
            nfa2.AddSymbolToAlphabet('$');

            nfa2.AddState("q0");
            nfa2.AddState("q1");
            nfa2.AddState("q2");
            nfa2.AddState("q3");
            nfa2.AddState("q4");

            nfa2.AddTransition("q0", '$', "q1");
            nfa2.AddTransition("q1", '$', "q2");
            nfa2.AddTransition("q2", '$', "q3");
            nfa2.AddTransition("q0", 'a', "q4");
            nfa2.AddTransition("q4", 'a', "q3");

            nfa2.AddFinalState("q3");
            Assert.That(nfa2.najkracaRijec(), Is.EqualTo(0));

            Nfa nfa3 = new();
            nfa3.StartState = "q0";
            nfa3.AddSymbolToAlphabet('a');
            nfa3.AddSymbolToAlphabet('$');

            nfa3.AddState("q0");
            nfa3.AddState("q1");
            nfa3.AddState("q2");
            nfa3.AddState("q3");
            nfa3.AddState("q4");

            nfa3.AddTransition("q0", '$', "q1");
            nfa3.AddTransition("q1", '$', "q2");
            nfa3.AddTransition("q2", 'a', "q3");
            nfa3.AddTransition("q0", 'a', "q4");
            nfa3.AddTransition("q4", 'a', "q3");

            nfa3.AddFinalState("q3");
            Assert.That(nfa3.najkracaRijec(), Is.EqualTo(1));
            
            Nfa nfa4 = new();
            nfa4.StartState = "q0";
            nfa4.AddSymbolToAlphabet('a');
            nfa4.AddSymbolToAlphabet('$');

            nfa4.AddState("q0");
            nfa4.AddState("q1");
            nfa4.AddState("q2");
            nfa4.AddState("q3");
            nfa4.AddState("q4");
            nfa4.AddState("q5");

            nfa4.AddTransition("q0", '$', "q1");
            nfa4.AddTransition("q1", '$', "q2");
            nfa4.AddTransition("q2", 'a', "q3");
            nfa4.AddTransition("q0", 'a', "q4");
            nfa4.AddTransition("q4", 'a', "q5");
            nfa4.AddTransition("q5", 'a', "q3");

            nfa4.AddFinalState("q3");
            Assert.That(nfa4.najkracaRijec(), Is.EqualTo(1));

            nfa4.AddFinalState("q2");
            Assert.That(nfa4.najkracaRijec(), Is.EqualTo(0));

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

            Assert.IsTrue(nfa.Accepts("a"));
            Assert.IsTrue(nfa.Accepts("baba"));
            Assert.IsFalse(nfa.Accepts("aab"));

            Dfa newDfa = nfa.toDfa();

            Assert.IsTrue(newDfa.Accepts("a"));
            Assert.IsTrue(newDfa.Accepts("baba"));
            Assert.IsFalse(newDfa.Accepts("aab"));

            Nfa nfa2 = new();
            nfa2.StartState = "q0";
            nfa2.AddState("q0");
            nfa2.AddState("q1");
            nfa2.AddState("q2");
            nfa2.AddState("q3");
            nfa2.AddState("q4");
            nfa2.AddState("q5");
            nfa2.AddSymbolToAlphabet('a');
            nfa2.AddSymbolToAlphabet('b');
            nfa2.AddSymbolToAlphabet('$');
            nfa2.AddTransition("q0",'a',"q1");
            nfa2.AddTransition("q0", 'b', "q0");
            nfa2.AddTransition("q1", 'b', "q1");
            nfa2.AddTransition("q1", '$', "q2");
            nfa2.AddTransition("q1", '$', "q4");
            nfa2.AddTransition("q2", 'a', "q2");
            nfa2.AddTransition("q2", 'b', "q5");
            nfa2.AddTransition("q4", 'b', "q3");
            nfa2.AddTransition("q3", 'a', "q4");
            nfa2.AddTransition("q4", 'a', "q5");
            nfa2.AddFinalState("q5");

            Assert.IsTrue(nfa2.Accepts("abbab"));
            Assert.IsTrue(nfa2.Accepts("baaaaab"));
            Assert.IsFalse(nfa2.Accepts("abbababa"));
            Assert.IsFalse(nfa2.Accepts("baabab"));

            Dfa newDfa2 = nfa2.toDfa();

            Assert.IsTrue(newDfa2.Accepts("abbab"));
            Assert.IsTrue(newDfa2.Accepts("baaaaab"));
            Assert.IsFalse(newDfa2.Accepts("abbababa"));
            Assert.IsFalse(newDfa2.Accepts("baabab"));
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
            Assert.IsTrue(newNfa.Accepts("c"));
            Assert.IsFalse(newNfa.Accepts("baa"));
            Assert.IsFalse(newNfa.Accepts("ababba"));

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
            Assert.IsFalse(newNfa.Accepts("caab"));
        }

        [Test]
        public void presjekTest()
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

            Nfa nfa2 = new();
            nfa2.StartState = "p0"; 
            nfa2.AddState("p0");
            nfa2.AddState("p1");
            nfa2.AddSymbolToAlphabet('a');
            nfa2.AddSymbolToAlphabet('b');
            nfa2.AddSymbolToAlphabet('$');
            nfa2.AddTransition("p0", 'a', "p0");
            nfa2.AddTransition("p0", '$', "p1");
            nfa2.AddTransition("p1", 'b', "p1");
            nfa2.AddFinalState("p0");

            Nfa newNfa = nfa.Presjek(nfa2);

            Assert.IsTrue(newNfa.Accepts("aaaaaa"));
            Assert.IsFalse(newNfa.Accepts("aaab"));
            Assert.IsFalse(newNfa.Accepts("ba"));
        }

        [Test]
        public void simetricnaRazlikaTest()
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

            Nfa nfa2 = new();
            nfa2.StartState = "p0";
            nfa2.AddState("p0");
            nfa2.AddState("p1");
            nfa2.AddSymbolToAlphabet('a');
            nfa2.AddSymbolToAlphabet('b');
            nfa2.AddSymbolToAlphabet('$');
            nfa2.AddTransition("p0", 'a', "p0");
            nfa2.AddTransition("p0", '$', "p1");
            nfa2.AddTransition("p1", 'b', "p1");
            nfa2.AddFinalState("p0");

            Nfa newNfa = nfa.SimetricnaRazlika(nfa2);

            Assert.IsTrue(newNfa.Accepts("baa"));
            Assert.IsFalse(newNfa.Accepts("aaaaaa"));
            Assert.IsFalse(newNfa.Accepts("aaab"));
            Assert.IsFalse(newNfa.Accepts("ba"));
        }

        [Test]
        public void presjekSaDfaTest()
        {
                Dfa dfa = new();
                dfa.StartState = "q0";

                dfa.AddSymbolToAlphabet('a');
                dfa.AddSymbolToAlphabet('b');

                dfa.AddState("q0");
                dfa.AddState("q1");
                dfa.AddState("q2");

                dfa.AddTransition("q0", 'a', "q1");
                dfa.AddTransition("q0", 'b', "q0");
                dfa.AddTransition("q1", 'b', "q1");
                dfa.AddTransition("q1", 'a', "q2");
                dfa.AddTransition("q2", 'b', "q1");
                dfa.AddTransition("q2", 'a', "q2");

                dfa.AddFinalState("q2");

                Nfa nfa = new();
                nfa.StartState = "p0";
                nfa.AddSymbolToAlphabet('a');
                nfa.AddSymbolToAlphabet('b');
                nfa.AddSymbolToAlphabet('$');
                nfa.AddState("p0");
                nfa.AddState("p1");
                nfa.AddState("p2");

                nfa.AddTransition("p0", 'a', "p1");
                nfa.AddTransition("p0", 'b', "p1");
                nfa.AddTransition("p1", '$', "p2");
                nfa.AddTransition("p1", 'a', "p0");
                nfa.AddTransition("p1", 'b', "p2");
                nfa.AddTransition("p2", 'a', "p2");
                nfa.AddTransition("p2", 'b', "p2");
                nfa.AddFinalState("p2");

                Nfa newNfa = nfa.Presjek(dfa);

                Assert.IsTrue(newNfa.Accepts("ababaabbabba"));
                Assert.IsFalse(newNfa.Accepts(""));
                Assert.IsTrue(newNfa.Accepts("babba"));
                Assert.IsFalse(newNfa.Accepts("abbbb"));
        }

        [Test]
        public void simetricnaRazlikaSaDfa()
        {
            Dfa dfa = new();
            dfa.StartState = "q0";

            dfa.AddSymbolToAlphabet('a');
            dfa.AddSymbolToAlphabet('b');

            dfa.AddState("q0");
            dfa.AddState("q1");
            dfa.AddState("q2");

            dfa.AddTransition("q0", 'a', "q1");
            dfa.AddTransition("q0", 'b', "q0");
            dfa.AddTransition("q1", 'b', "q1");
            dfa.AddTransition("q1", 'a', "q2");
            dfa.AddTransition("q2", 'b', "q1");
            dfa.AddTransition("q2", 'a', "q2");

            dfa.AddFinalState("q2");

            Nfa nfa = new();
            nfa.StartState = "p0";
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('b');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddState("p0");
            nfa.AddState("p1");
            nfa.AddState("p2");

            nfa.AddTransition("p0", 'a', "p1");
            nfa.AddTransition("p0", 'b', "p1");
            nfa.AddTransition("p1", '$', "p2");
            nfa.AddTransition("p1", 'a', "p0");
            nfa.AddTransition("p1", 'b', "p2");
            nfa.AddTransition("p2", 'a', "p2");
            nfa.AddTransition("p2", 'b', "p2");
            nfa.AddFinalState("p2");

            Nfa newNfa = nfa.SimetricnaRazlika(dfa);

            Assert.IsTrue(newNfa.Accepts("ab"));
            Assert.IsFalse(newNfa.Accepts("ababaabaa"));
        }

        [Test]
        public void ulancavanjeTest()
        {
            Nfa nfa1 = new();
            Nfa nfa2 = new();
            Nfa nfa3 = new();

            nfa1.StartState = "q0";
            nfa1.AddState("q0");
            nfa1.AddState("q1");
            nfa1.AddState("q2");
            nfa1.AddSymbolToAlphabet('a');
            nfa1.AddSymbolToAlphabet('b');
            nfa1.AddSymbolToAlphabet('$');
            nfa1.AddTransition("q0",'$',"q2");
            nfa1.AddTransition("q0", 'b', "q1");
            nfa1.AddTransition("q1", 'a', "q1");
            nfa1.AddTransition("q1", 'b', "q2");
            nfa1.AddTransition("q1", 'a', "q2");
            nfa1.AddTransition("q2", 'a', "q0");
            nfa1.AddFinalState("q2");

            nfa2.StartState = "p0";
            nfa2.AddState("p0");
            nfa2.AddState("p1");
            nfa2.AddState("p2");
            nfa2.AddSymbolToAlphabet('a');
            nfa2.AddSymbolToAlphabet('b');
            nfa2.AddSymbolToAlphabet('$');
            nfa2.AddTransition("p0", '$', "p1");
            nfa2.AddTransition("p0", 'a', "p1");
            nfa2.AddTransition("p1", 'b', "p2");
            nfa2.AddFinalState("p2");

            nfa3.StartState = "k0";
            nfa3.AddState("k0");
            nfa3.AddState("k1");
            nfa3.AddSymbolToAlphabet('c');
            nfa3.AddTransition("k0", 'c', "k1");
            nfa3.AddFinalState("k1");

            Nfa ulancavanje = nfa1.Unija(nfa2).Spajanje(nfa3).KleenovaZvijezda();

            Assert.IsTrue(ulancavanje.Accepts("baaabcbc"));
            Assert.IsTrue(ulancavanje.Accepts("abcabc"));
            Assert.IsFalse(ulancavanje.Accepts("bab"));
            Assert.IsFalse(ulancavanje.Accepts("ab"));

        }
    }
}
