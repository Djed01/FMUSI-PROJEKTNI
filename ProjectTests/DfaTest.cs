using FMSI.Lib;

namespace ProjectTests
{
    public class DfaTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AcceptsTest()
        {
            Dfa dfa = new();
            dfa.StartState = "q0";

            dfa.AddSymbolToAlphabet('a');
            dfa.AddSymbolToAlphabet('b');

            dfa.AddState("q0");
            dfa.AddState("q1");
            dfa.AddState("q2");
            dfa.AddState("q3");

            dfa.AddTransition("q0", 'a', "q1");
            dfa.AddTransition("q1", 'b', "q2");
            dfa.AddTransition("q2", 'b', "q3");

            dfa.AddFinalState("q3");

            Assert.IsTrue(dfa.Accepts("abb"));
            Assert.IsFalse(dfa.Accepts("aaaabb"));
        }

        [Test]
        public void DeclinesTest()
        {
            Dfa dfa = new();
            dfa.StartState = "q0";

            dfa.AddSymbolToAlphabet('a');
            dfa.AddSymbolToAlphabet('b');

            dfa.AddState("q0");
            dfa.AddState("q1");
            dfa.AddState("q2");

            dfa.AddTransition("q0", 'a', "q1");
            dfa.AddTransition("q1", 'b', "q1");
            dfa.AddTransition("q1", 'a', "q2");
            dfa.AddTransition("q2", 'a', "q2");
            dfa.AddTransition("q2", 'b', "q1");

            dfa.AddFinalState("q2");

            Assert.IsFalse(dfa.Accepts("aaaaaaaaabb"));
            Assert.IsTrue(dfa.Accepts("abaabba"));
        }

        [Test]
        public void UnijaTest()
        {
            Dfa dfa1 = new();
            Dfa dfa2 = new();

            dfa1.StartState = "q0";

            dfa1.AddSymbolToAlphabet('a');
            dfa1.AddSymbolToAlphabet('b');

            dfa1.AddState("q0");
            dfa1.AddState("q1");

            dfa1.AddTransition("q0", 'a', "q0");
            dfa1.AddTransition("q0", 'b', "q1");
            dfa1.AddTransition("q1", 'a', "q1");
            dfa1.AddTransition("q1", 'b', "q1");

            dfa1.AddFinalState("q1");


            dfa2.StartState = "p0";

            dfa2.AddSymbolToAlphabet('a');
            dfa2.AddSymbolToAlphabet('b');

            dfa2.AddState("p0");
            dfa2.AddState("p1");
            dfa2.AddState("p2");

            dfa2.AddTransition("p0", 'a', "p1");
            dfa2.AddTransition("p0", 'b', "p2");
            dfa2.AddTransition("p1", 'a', "p1");
            dfa2.AddTransition("p1", 'b', "p1");
            dfa2.AddTransition("p2", 'a', "p2");
            dfa2.AddTransition("p2", 'b', "p2");

            dfa2.AddFinalState("p2");

            Dfa unija = dfa1.Unija(dfa2);

            Assert.IsTrue(unija.Accepts("aab"));
            Assert.IsFalse(unija.Accepts("aaaaaaa"));
        }

        [Test]
        public void PresjekTest()
        {
            Dfa dfa1 = new();
            Dfa dfa2 = new();

            dfa1.StartState = "q0";

            dfa1.AddState("q0");
            dfa1.AddState("q1");
            dfa1.AddState("q2");
            dfa1.AddState("q3");
            dfa1.AddState("q4");

            dfa1.AddSymbolToAlphabet('a');
            dfa1.AddSymbolToAlphabet('b');

            dfa1.AddTransition("q0", 'a', "q1");
            dfa1.AddTransition("q0", 'b', "q2");
            dfa1.AddTransition("q1", 'a', "q3");
            dfa1.AddTransition("q1", 'b', "q1");
            dfa1.AddTransition("q3", 'a', "q3");
            dfa1.AddTransition("q3", 'b', "q1");
            dfa1.AddTransition("q2", 'a', "q2");
            dfa1.AddTransition("q2", 'b', "q4");
            dfa1.AddTransition("q4", 'a', "q2");
            dfa1.AddTransition("q4", 'b', "q4");

            dfa1.AddFinalState("q3");
            dfa1.AddFinalState("q4");


            dfa2.StartState = "p0";
            dfa2.AddState("p0");
            dfa2.AddState("p1");
            dfa2.AddState("p2");
            dfa2.AddState("p3");
            dfa2.AddSymbolToAlphabet('a');
            dfa2.AddSymbolToAlphabet('b');

            dfa2.AddTransition("p0", 'a', "p0");
            dfa2.AddTransition("p0", 'b', "p1");
            dfa2.AddTransition("p1", 'a', "p0");
            dfa2.AddTransition("p1", 'b', "p2");
            dfa2.AddTransition("p2", 'a', "p0");
            dfa2.AddTransition("p2", 'b', "p3");
            dfa2.AddTransition("p3", 'a', "p3");
            dfa2.AddTransition("p3", 'b', "p3");

            dfa2.AddFinalState("p0");
            dfa2.AddFinalState("p1");
            dfa2.AddFinalState("p2");

            Dfa newDfa = dfa1.Presjek(dfa2);

            Assert.IsTrue(newDfa.Accepts("bbaab"));
            Assert.IsFalse(newDfa.Accepts("abbbaabba"));
            Assert.IsTrue(newDfa.Accepts("abaa"));
            Assert.IsFalse(newDfa.Accepts("babbba"));
        }

        [Test]
        public void SimetricnaRazlikaTest()
        {
            Dfa dfa1 = new();
            Dfa dfa2 = new();

            dfa1.StartState = "q0";

            dfa1.AddState("q0");
            dfa1.AddState("q1");
            dfa1.AddState("q2");
            dfa1.AddState("q3");
            dfa1.AddState("q4");

            dfa1.AddSymbolToAlphabet('a');
            dfa1.AddSymbolToAlphabet('b');

            dfa1.AddTransition("q0", 'a', "q1");
            dfa1.AddTransition("q0", 'b', "q2");
            dfa1.AddTransition("q1", 'a', "q3");
            dfa1.AddTransition("q1", 'b', "q1");
            dfa1.AddTransition("q3", 'a', "q3");
            dfa1.AddTransition("q3", 'b', "q1");
            dfa1.AddTransition("q2", 'a', "q2");
            dfa1.AddTransition("q2", 'b', "q4");
            dfa1.AddTransition("q4", 'a', "q2");
            dfa1.AddTransition("q4", 'b', "q4");

            dfa1.AddFinalState("q3");
            dfa1.AddFinalState("q4");


            dfa2.StartState = "p0";
            dfa2.AddState("p0");
            dfa2.AddState("p1");
            dfa2.AddState("p2");
            dfa2.AddState("p3");
            dfa2.AddSymbolToAlphabet('a');
            dfa2.AddSymbolToAlphabet('b');

            dfa2.AddTransition("p0", 'a', "p0");
            dfa2.AddTransition("p0", 'b', "p1");
            dfa2.AddTransition("p1", 'a', "p0");
            dfa2.AddTransition("p1", 'b', "p2");
            dfa2.AddTransition("p2", 'a', "p0");
            dfa2.AddTransition("p2", 'b', "p3");
            dfa2.AddTransition("p3", 'a', "p3");
            dfa2.AddTransition("p3", 'b', "p3");

            dfa2.AddFinalState("p0");
            dfa2.AddFinalState("p1");
            dfa2.AddFinalState("p2");

            Dfa newDfa = dfa1.SimetricnaRazlika(dfa2);

            Assert.IsTrue(newDfa.Accepts("babba"));
            Assert.IsFalse(newDfa.Accepts("babbba"));
            Assert.IsTrue(newDfa.Accepts("aabbab"));
            Assert.IsFalse(newDfa.Accepts("abbbaab"));
        }

        [Test]
        public void spajanjeTest()
        {
            Dfa dfa1 = new();
            Dfa dfa2 = new();

            dfa1.StartState = "q0";

            dfa1.AddState("q0");
            dfa1.AddState("q1");

            dfa1.AddSymbolToAlphabet('a');
            dfa1.AddTransition("q0", 'a', "q1");
            dfa1.AddFinalState("q1");

            dfa2.StartState = "p0";
            dfa2.AddState("p0");
            dfa2.AddState("p1");

            dfa2.AddSymbolToAlphabet('b');
            dfa2.AddTransition("p0", 'b', "p1");
            dfa2.AddFinalState("p1");

            Dfa newDfa = dfa1.Spajanje(dfa2);

            Assert.IsTrue(newDfa.Accepts("ab"));
            Assert.IsFalse(newDfa.Accepts("aa"));

        }

        [Test]
        public void komplementTest()
        {
            Dfa dfa1 = new();

            dfa1.StartState = "q0";

            dfa1.AddState("q0");
            dfa1.AddState("q1");
            dfa1.AddState("q2");
            dfa1.AddState("q3");
            dfa1.AddState("q4");

            dfa1.AddSymbolToAlphabet('a');
            dfa1.AddSymbolToAlphabet('b');

            dfa1.AddTransition("q0", 'a', "q1");
            dfa1.AddTransition("q0", 'b', "q2");
            dfa1.AddTransition("q1", 'a', "q3");
            dfa1.AddTransition("q1", 'b', "q1");
            dfa1.AddTransition("q3", 'a', "q3");
            dfa1.AddTransition("q3", 'b', "q1");
            dfa1.AddTransition("q2", 'a', "q2");
            dfa1.AddTransition("q2", 'b', "q4");
            dfa1.AddTransition("q4", 'a', "q2");
            dfa1.AddTransition("q4", 'b', "q4");

            dfa1.AddFinalState("q3");
            dfa1.AddFinalState("q4");

            Dfa newDfa = dfa1.Komplement();

            Assert.IsTrue(newDfa.Accepts("ababb"));
            Assert.IsFalse(newDfa.Accepts("babb"));
        }

        [Test]
        public void kleeneTest()
        {
            Dfa dfa = new();
            dfa.StartState = "q0";

            dfa.AddSymbolToAlphabet('a');
            dfa.AddSymbolToAlphabet('b');

            dfa.AddState("q0");
            dfa.AddState("q1");
            dfa.AddState("q2");

            dfa.AddTransition("q0", 'a', "q1");
            dfa.AddTransition("q1", 'b', "q1");
            dfa.AddTransition("q1", 'a', "q2");
            dfa.AddTransition("q2", 'b', "q1");

            dfa.AddFinalState("q2");

            Dfa newDfa = dfa.KleenovaZvijezda();

            Assert.IsTrue(newDfa.Accepts("abaaba"));
            Assert.IsFalse(newDfa.Accepts("abaabb"));
        }

        [Test]
        public void najkracaRijecTest()
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

            dfa.AddFinalState("q2");

            Assert.That(dfa.najkracaRijec(), Is.EqualTo(2));

            Dfa dfa2 = new();
            dfa2.StartState = "p0";
            dfa2.AddState("p0");
            dfa2.AddState("p1");
            dfa2.AddState("p2");
            dfa2.AddState("p3");
            dfa2.AddSymbolToAlphabet('a');
            dfa2.AddSymbolToAlphabet('b');

            dfa2.AddTransition("p0", 'a', "p0");
            dfa2.AddTransition("p0", 'b', "p1");
            dfa2.AddTransition("p1", 'a', "p0");
            dfa2.AddTransition("p1", 'b', "p2");
            dfa2.AddTransition("p2", 'a', "p0");
            dfa2.AddTransition("p2", 'b', "p3");
            dfa2.AddTransition("p3", 'a', "p3");
            dfa2.AddTransition("p3", 'b', "p3");

            dfa2.AddFinalState("p0");
            dfa2.AddFinalState("p1");
            dfa2.AddFinalState("p2");

            Assert.That(dfa2.najkracaRijec(), Is.EqualTo(1));
        }

        [Test]
        public void toNfaTest()
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

            dfa.AddFinalState("q2");

            Assert.IsTrue(dfa.Accepts("aba"));
            Assert.IsFalse(dfa.Accepts("ababb"));

            Nfa newNfa = dfa.toNfa();
            Assert.IsTrue(newNfa.Accepts("aba"));
            Assert.IsFalse(newNfa.Accepts("ababb"));
        }

        [Test]
        public void spajanjeSaNfaTest()
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

            dfa.AddFinalState("q2");

            Nfa nfa = new();
            nfa.StartState = "p0";
            nfa.AddSymbolToAlphabet('a');
            nfa.AddSymbolToAlphabet('$');
            nfa.AddState("p0");
            nfa.AddState("p1");
            nfa.AddState("p2");

            nfa.AddTransition("p0", 'a', "p1");
            nfa.AddTransition("p1", '$', "p2");
            nfa.AddFinalState("p2");

            Dfa newDfa = dfa.Spajanje(nfa);
            Assert.IsTrue(newDfa.Accepts("ababbaa"));
            Assert.IsFalse(newDfa.Accepts("aba"));
        }

        [Test]
        public void presjekSaNfa()
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

            Dfa newDfa = dfa.Presjek(nfa);

            Assert.IsTrue(newDfa.Accepts("ababaabbabba"));
        }

        [Test]
        public void UnijaSaNfa()
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

            Dfa newDfa = dfa.Unija(nfa);

            Assert.IsTrue(newDfa.Accepts("aba"));
        }

        [Test]
        public void simetricnaRazlikaSaNfa()
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

            Dfa newDfa = dfa.SimetricnaRazlika(nfa);

            Assert.IsTrue(newDfa.Accepts("ab"));
        }
    }
}