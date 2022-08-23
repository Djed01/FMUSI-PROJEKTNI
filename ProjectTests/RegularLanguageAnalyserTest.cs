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
        private static string REGEX_PATH = "./regex/";
        private static string AUTOMAT_PATH = "./automat/";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AutomatLexerTest()
        {
            AutomatLexer automatLexer1 = new(AUTOMAT_PATH + "automat1.txt"); // 8 linija simbol duzi od jedan
            Assert.Throws<Exception>(()=> automatLexer1.analyse());

            AutomatLexer automatLexer2 = new(AUTOMAT_PATH + "automat2.txt"); // 3 linija nekorektan format stanja
            Assert.Throws<Exception>(() => automatLexer2.analyse());

            AutomatLexer automatLexer3 = new(AUTOMAT_PATH + "automat3.txt"); // 11 linija nepostojeci simbol
            Assert.Throws<Exception>(() => automatLexer3.analyse());

            AutomatLexer automatLexer4 = new(AUTOMAT_PATH + "automat4.txt"); // 16 nepostojece stanje u  finalnim
            Assert.Throws<Exception>(() => automatLexer4.analyse());

            AutomatLexer automatLexer5 = new(AUTOMAT_PATH + "automat5.txt"); // 12 linija nepostojece stanje u tranziciji
            Assert.Throws<Exception>(() => automatLexer5.analyse());

            AutomatLexer automatLexer6 = new(AUTOMAT_PATH + "automat6.txt"); // 12 nekorektan format tranzicije
            Assert.Throws<Exception>(() => automatLexer6.analyse());

            AutomatLexer automatLexer7 = new(AUTOMAT_PATH + "automat7.txt"); // 11 stanje umjesto simbola u tranziciji
            Assert.Throws<Exception>(() => automatLexer7.analyse());

            AutomatLexer automatLexer = new(AUTOMAT_PATH + "automat.txt");
            Assert.DoesNotThrow(() => automatLexer.analyse());
        }

        [Test]
        public void RegexLexerTest()
        {
            RegexLexer regexLexer1 = new(REGEX_PATH+"regex1.txt"); // 3: nekorektan format
            Assert.Throws<Exception>(() => regexLexer1.analyse());

            RegexLexer regexLexer2 = new(REGEX_PATH + "regex2.txt"); // 3: neocekivani simbol * na tom mjestu
            Assert.Throws<Exception>(() => regexLexer2.analyse());

            RegexLexer regexLexer3 = new(REGEX_PATH + "regex3.txt"); // 13: nebalansirane zagrade
            Assert.Throws<Exception>(() => regexLexer3.analyse());

            RegexLexer regexLexer4 = new(REGEX_PATH + "regex4.txt"); // 9: * nakon +
            Assert.Throws<Exception>(() => regexLexer4.analyse());

            RegexLexer regexLexer5 = new(REGEX_PATH + "regex5.txt"); // + na kraju
            Assert.Throws<Exception>(() => regexLexer5.analyse());

            RegexLexer regexLexer6 = new(REGEX_PATH + "regex6.txt"); // 12: nedefinisani simbol
            Assert.Throws<Exception>(() => regexLexer6.analyse());

            RegexLexer regexLexer7 = new(REGEX_PATH + "regex7.txt"); // * na pocetku
            Assert.Throws<Exception>(() => regexLexer7.analyse());

            RegexLexer regexLexer = new(REGEX_PATH + "regex.txt");
            Assert.DoesNotThrow(() => regexLexer.analyse());
        }

    }
}
