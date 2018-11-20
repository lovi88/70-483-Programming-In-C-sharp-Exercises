using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndUseTypes
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    class PrivatenessForMyself
    {
        private int _a;

        public PrivatenessForMyself(int a)
        {
            _a = a;
        }

        public void InitFromOther(PrivatenessForMyself other)
        {
            _a = other._a;
        }

        public override string ToString()
        {
            return _a.ToString();
        }
    }
}
