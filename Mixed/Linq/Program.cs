using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    public class Program
    {
        static void Main(string[] args)
        {



            Console.ReadLine();
        }

        static void LoanAmounts()
        {
            decimal[] loanAmounts = { 303m, 1000m };

            IEnumerable<decimal> loanQuery =
                (from amount in loanAmounts
                 where amount % 2 == 0
                 orderby amount ascending
                 select amount).Distinct();
        }

        private Product[] _products = new[]
        {
            new Product() {CategoryId = 0}, new Product(){CategoryId = 1}, new Product() {CategoryId = 2},
        };

        private ProductCategory[] _categories = new[] { new ProductCategory() { CategoryId = 0 }, new ProductCategory() { CategoryId = 1 }, new ProductCategory() { CategoryId = 2 }, };
        public void Join01()
        {
            var joined =
                from product in _products
                join category in _categories
                    on product.CategoryId equals category.CategoryId
                select new { product, category };
            
        }

        public void Join02Froms()
        {
            var joined =
                from product in _products
                from category in _categories
                where category.CategoryId == product.CategoryId //usage of the keyword equals is not allowed
                select new { product, category };


        }
    }
}
