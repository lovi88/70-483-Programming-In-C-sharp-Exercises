using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndUseTypes
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MyClass ms = new MyClass(2);
            ms._asd = 10;
            Console.WriteLine(ms._asd);

            MyDerived md = new MyDerived(2f);
            Console.WriteLine(md._asd);
        }

        class MyClass
        {
            public int _asd;

            public MyClass()
            {
                
            }

            public MyClass(int asd)
            {
                _asd = asd;
            }
        }

        class MyDerived : MyClass
        {
            protected float _asd;

            public MyDerived(float asd) : base(3)
            {
                _asd = asd;
            }

            public MyDerived()
            {
                
            }
        }
    }
}
