using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMSI.Lib;

namespace ProjectTests
{
    public class RegularExpressionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void regexToNfaTest()
        {
            RegularExpression regex2 = new RegularExpression("(b((ab)*))+(a(b*)a)");
            Nfa newNfa2 = regex2.toNfa();
            Assert.IsTrue(newNfa2.Accepts("babab"));
            Assert.IsTrue(newNfa2.Accepts("abbbba"));
            Assert.IsFalse(newNfa2.Accepts("baba"));


            RegularExpression regex = new RegularExpression("(aa)b+caab");
            Nfa newNfa = regex.toNfa();
            Assert.IsTrue(newNfa.Accepts("aab"));
            Assert.IsTrue(newNfa.Accepts("caab"));

            RegularExpression regex3 = new RegularExpression("(abc)*+a*b+a*");
            Nfa newNfa3 = regex3.toNfa();
            Assert.IsTrue(newNfa3.Accepts("abcabcabc"));
            Assert.IsTrue(newNfa3.Accepts("aaaaaab"));
            Assert.IsTrue(newNfa3.Accepts("aaaaaa"));
            Assert.IsFalse(newNfa3.Accepts("abcabcab"));

            RegularExpression regex1 = new RegularExpression("ab**+(c+a)*");
            Nfa newNfa1 = regex1.toNfa();
            Assert.IsTrue(newNfa1.Accepts("abbb"));
            Assert.IsTrue(newNfa1.Accepts("acccaaccaac"));
            Assert.IsFalse(newNfa1.Accepts("abba"));
            Assert.IsFalse(newNfa1.Accepts("acb"));
        }
    }
}
