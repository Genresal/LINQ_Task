using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(x => x.Orders.Select(y => y.Total).Sum() > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            var res = customers.Select(x => (x, suppliers.Where(y => y.Country == x.Country && y.City == x.City)));
            return res;
        }
        //Здесь так и не нашел решения.
        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            var s = suppliers.GroupBy(x => new {x.Country, x.City}).ToDictionary(x => x.Key, x => x.AsEnumerable());

            var res = customers.Select(x => (x, s.ContainsKey(new {x.Country, x.City })? s[new {x.Country, x.City}] : null)).ToList();
        return res;
        }
        //Find all customers with the sum of all orders that exceed a certain value. 
        //Могу ошибаться, но вроде тут что то с тестами.
        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(x => x.Orders.Length > limit);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            return customers.Where(x => x.Orders.Any()).Select(x => (x, x.Orders.Min(o => o.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            return customers.Where(x => x.Orders.Any())
                .Select(x => (x, x.Orders.Min(o => o.OrderDate)))
                .OrderBy(x => x.Item2.Date.Year)
                .ThenBy(x => x.Item2.Date.Month)
                .ThenByDescending(x => x.x.Orders.Sum(o => o.Total))
                .ThenBy(x => x.x.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers.Where(x => x.PostalCode.Any(c => !char.IsDigit(c))
                                        || !x.Phone.Contains('(')
                                        || x.Region == null).ToList();
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {

            return products.GroupBy(x => x.Category)
                .Select(x => new Linq7CategoryGroup()
                {
                    Category = x.Key,
                    UnitsInStockGroup = x.GroupBy(g => g.UnitsInStock)
                        .Select(z => new Linq7UnitsInStockGroup()
                        {
                            UnitsInStock = z.Key,
                            Prices = z.OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice)
                        })
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            var categories = new List<decimal> { cheap, middle, expensive };
            return products.GroupBy(x => categories.FirstOrDefault(c => c >= x.UnitPrice))
                .Select(g => (g.Key, g.AsEnumerable()))
                .ToList();
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            return customers.GroupBy(x => x.City)
                .Select(g => (g.Key,
                    (int)Math.Round(g.Average(c => c.Orders.Sum(o => o.Total))),
                    (int)Math.Round(g.Average(c => c.Orders.Length))))
                .ToList();
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            return String.Join("",
                suppliers.Select(x => x.Country)
                    .Distinct()
                    .OrderBy(x => x.Length)
                    .ThenBy(x => x)
                    .ToArray());
        }
    }
}