using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplementDataAccess
{
    [TestClass]
    public class LINQ
    {
        private IEnumerable<int> _ints = new[] { 1, 2, 3, 4, 5 };

        [TestMethod]
        public void TestMethod1()
        {
            var funcs = new List<Func<int>>();

            foreach (var i in Enumerable.Range(0,5))
            {
                funcs.Add((() => i));
            }

            foreach (var f in funcs)
            {
                Console.WriteLine(f());
            }

            funcs.Clear();

            for (int i = 0; i < _ints.Count(); i++)
            {
                var i1 = i;
                funcs.Add((() => i1));
            }

            foreach (var f in funcs)
            {
                Console.WriteLine(f());
            }

            funcs.Clear();

            for (int i = 0; i < _ints.Count(); i++)
            {
                var i1 = i;
                funcs.Add((() => i1));
            }

            foreach (var f in funcs)
            {
                Console.WriteLine(f());
            }

        }
    }
}
