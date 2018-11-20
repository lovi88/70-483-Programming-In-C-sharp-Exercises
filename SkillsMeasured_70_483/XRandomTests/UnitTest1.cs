using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XRandomTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void StringReferenceEquality()
        {
            var s0 = "str";
            var s1 = new string(new[] { 's', 't', 'r' });
            var s2 = new string(new[] { 's', 't', 'r' });
            var s3 = string.Intern("str");
            var s4 = string.Intern(s1);

            Assert.AreEqual(s0, s1);
            Assert.IsFalse(ReferenceEquals(s0, s1));


            Assert.AreEqual(s1, s2);
            Assert.IsFalse(ReferenceEquals(s1, s2));

            //Using Intern
            Assert.AreEqual(s0, s3);
            Assert.IsTrue(ReferenceEquals(s0, s3));

            Assert.AreEqual(s0, s4);
            Assert.IsTrue(ReferenceEquals(s0, s4));
        }


    }
}
