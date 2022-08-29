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
            Assert.IsFalse(dfa.Accepts(""));
            Assert.IsFalse(dfa.Accepts("a"));
            Assert.IsFalse(dfa.Accepts("ab"));      
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
            Assert.IsFalse(dfa.Accepts(""));
            Assert.IsFalse(dfa.Accepts("a"));
            Assert.IsFalse(dfa.Accepts("ab"));
            Assert.IsFalse(dfa.Accepts("abab"));
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

            dfa2.AddSymbolToAlphabet('c');
            Dfa newDfa = new();
            Assert.Throws<Exception>(() => newDfa = dfa1.Unija(dfa2));
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

            dfa2.AddSymbolToAlphabet('c');
            Dfa newDfa2 = new();
            Assert.Throws<Exception>(() => newDfa2 = dfa1.Presjek(dfa2));
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

            dfa2.AddSymbolToAlphabet('c');
            Dfa newDfa2 = new();
            Assert.Throws<Exception>(() => newDfa2 = dfa1.SimetricnaRazlika(dfa2));
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

            Assert.IsFalse(dfa1.Accepts("ababb"));
            Assert.IsFalse(dfa1.Accepts("aaaab"));
            Assert.IsTrue(dfa1.Accepts("babb"));
            Assert.IsTrue(dfa1.Accepts("bbbaab"));

            Dfa newDfa = dfa1.Komplement();

            Assert.IsTrue(newDfa.Accepts("ababb"));
            Assert.IsTrue(newDfa.Accepts("aaaab"));
            Assert.IsFalse(newDfa.Accepts("babb"));
            Assert.IsFalse(newDfa.Accepts("bbbaab"));
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

            Assert.Throws<Exception>(() => dfa.najkracaRijec());

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
            Assert.IsTrue(dfa.Accepts("aa"));
            Assert.IsTrue(dfa.Accepts("ababa"));
            Assert.IsFalse(dfa.Accepts("ababb"));
            Assert.IsFalse(dfa.Accepts("ba"));
            Assert.IsFalse(dfa.Accepts(""));


            Nfa newNfa = dfa.toNfa();
            Assert.IsTrue(newNfa.Accepts("aba"));
            Assert.IsTrue(dfa.Accepts("aa"));
            Assert.IsTrue(dfa.Accepts("ababa"));
            Assert.IsFalse(newNfa.Accepts("ababb"));
            Assert.IsFalse(dfa.Accepts("ba"));
            Assert.IsFalse(dfa.Accepts(""));
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
            Assert.IsTrue(newDfa.Accepts("abbaa"));
            Assert.IsFalse(newDfa.Accepts("aba"));
            Assert.IsFalse(newDfa.Accepts(""));
            Assert.IsFalse(newDfa.Accepts("bbbbabbb"));
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
            Assert.IsFalse(newDfa.Accepts(""));
            Assert.IsFalse(newDfa.Accepts("bababb"));
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
            Assert.IsTrue(newDfa.Accepts("ababba"));
            Assert.IsFalse(newDfa.Accepts(""));
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
            Assert.IsFalse(newDfa.Accepts("aba"));
            Assert.IsFalse(newDfa.Accepts("ababba"));
        }

        [Test]
        public void multipleTests()
        {
            Dfa dfa_1 = new();
            Dfa dfa_2 = new();

            dfa_1.StartState = "q0";
            dfa_1.AddState("q0");
            dfa_1.AddState("q1");
            dfa_1.AddState("q2");
            dfa_1.AddSymbolToAlphabet('a');
            dfa_1.AddSymbolToAlphabet('b');
            dfa_1.AddTransition("q0", 'a', "q0");
            dfa_1.AddTransition("q0", 'b', "q1");
            dfa_1.AddTransition("q1", 'a', "q2");
            dfa_1.AddTransition("q1", 'b', "q2");
            dfa_1.AddTransition("q2", 'a', "q2");
            dfa_1.AddTransition("q2", 'a', "q2");
            dfa_1.AddFinalState("q1");

            dfa_2.StartState = "q3";
            dfa_2.AddState("q3");
            dfa_2.AddState("q4");
            dfa_2.AddSymbolToAlphabet('a');
            dfa_2.AddSymbolToAlphabet('b');
            dfa_2.AddTransition("q3", 'a', "q4");
            dfa_2.AddTransition("q3", 'b', "q3");
            dfa_2.AddTransition("q4", 'a', "q3");
            dfa_2.AddTransition("q4", 'b', "q4");
            dfa_2.AddFinalState("q4");

            Dfa unija = dfa_1.Unija(dfa_2);
            Dfa presjek = dfa_1.Presjek(dfa_2);
            Dfa simetricnaRazlika = dfa_1.SimetricnaRazlika(dfa_2);
            Dfa spajanje = dfa_1.Spajanje(dfa_2);
            Dfa komplement2 = dfa_2.Komplement();
            Dfa kleenovaZvijezda = dfa_1.KleenovaZvijezda();
            Dfa ulancavanje = dfa_1.Presjek(dfa_2).KleenovaZvijezda();

            Assert.IsTrue(unija.Accepts("abb"));
            Assert.IsTrue(unija.Accepts("babbb"));
            Assert.IsTrue(unija.Accepts("aaaaa"));
            Assert.IsFalse(unija.Accepts("ababb"));
            Assert.IsFalse(unija.Accepts("baabb"));

            Assert.IsTrue(presjek.Accepts("aaab"));
            Assert.IsTrue(presjek.Accepts("ab"));
            Assert.IsFalse(presjek.Accepts("aba"));
            Assert.IsFalse(presjek.Accepts("bba"));

            Assert.IsTrue(simetricnaRazlika.Accepts("aaa"));
            Assert.IsTrue(simetricnaRazlika.Accepts("babb"));
            Assert.IsFalse(simetricnaRazlika.Accepts("ab"));
            Assert.IsFalse(simetricnaRazlika.Accepts("bbb"));

            Assert.IsTrue(spajanje.Accepts("abbba"));
            Assert.IsTrue(spajanje.Accepts("bba"));
            Assert.IsFalse(spajanje.Accepts("bbbb"));
            Assert.IsFalse(spajanje.Accepts("ababa"));

            Assert.IsTrue(komplement2.Accepts("baba"));
            Assert.IsFalse(komplement2.Accepts("bbabb"));

            Assert.IsTrue(kleenovaZvijezda.Accepts("babab"));
            Assert.IsFalse(kleenovaZvijezda.Accepts("baba"));

            Assert.That(dfa_1.najkracaRijec(), Is.EqualTo(1));
            Assert.That(dfa_2.najkracaRijec(), Is.EqualTo(1));


            Assert.IsTrue(ulancavanje.Accepts("aaabaaab"));
            Assert.IsTrue(ulancavanje.Accepts("ababab"));
            Assert.IsFalse(ulancavanje.Accepts("abaaba"));
            Assert.IsFalse(ulancavanje.Accepts("aba"));
   
        }
    }
}