using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegularLanguageAnalyser;

namespace ProjectTests
{
    public class RegularLanguageAnalyserTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AutomatLexerTest()
        {
            AutomatLexer automatLexer1 = new("automat1.txt"); // 8 linija simbol duzi od jedan
            Assert.Throws<Exception>(()=> automatLexer1.analyse());

            AutomatLexer automatLexer2 = new("automat2.txt"); // 3 linija nekorektan format stanja
            Assert.Throws<Exception>(() => automatLexer2.analyse());

            AutomatLexer automatLexer3 = new("automat3.txt"); // 11 linija nepostojeci simbol
            Assert.Throws<Exception>(() => automatLexer3.analyse());

            AutomatLexer automatLexer4 = new("automat4.txt"); // 16 nepostojece stanje u  finalnim
            Assert.Throws<Exception>(() => automatLexer4.analyse());

            AutomatLexer automatLexer5 = new("automat5.txt"); // 12 linija nepostojece stanje u tranziciji
            Assert.Throws<Exception>(() => automatLexer5.analyse());

            AutomatLexer automatLexer6 = new("automat6.txt"); // 12 nekorektan format tranzicije
            Assert.Throws<Exception>(() => automatLexer6.analyse());

            AutomatLexer automatLexer7 = new("automat7.txt"); // 11 stanje umjesto simbola u tranziciji
            Assert.Throws<Exception>(() => automatLexer7.analyse());

            AutomatLexer automatLexer = new("automat.txt");
            Assert.DoesNotThrow(() => automatLexer.analyse());
        }

        [Test]
        public void RegexLexerTest()
        {
            RegexLexer regexLexer1 = new("regex1.txt"); // 3: nekorektan format
            Assert.Throws<Exception>(() => regexLexer1.analyse());

            RegexLexer regexLexer2 = new("regex2.txt"); // 3: neocekivani simbol * na tom mjestu
            Assert.Throws<Exception>(() => regexLexer2.analyse());

            RegexLexer regexLexer3 = new("regex3.txt"); // nebalansirane zagrade
            Assert.Throws<Exception>(() => regexLexer3.analyse());

            RegexLexer regexLexer4 = new("regex4.txt"); // 9: * nakon +
            Assert.Throws<Exception>(() => regexLexer4.analyse());

            RegexLexer regexLexer5 = new("regex5.txt"); // + na kraju
            Assert.Throws<Exception>(() => regexLexer5.analyse());

            RegexLexer regexLexer6 = new("regex6.txt"); // 12: nedefinisani simbol
            Assert.Throws<Exception>(() => regexLexer6.analyse());

            RegexLexer regexLexer7 = new("regex7.txt"); // * na pocetku
            Assert.Throws<Exception>(() => regexLexer7.analyse());

            RegexLexer regexLexer = new("regex.txt");
            Assert.DoesNotThrow(() => regexLexer.analyse());
        }

    }
}
