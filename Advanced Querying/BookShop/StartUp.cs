using System;
using System.Linq;
using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;
using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.VisualBasic;
using Z.EntityFramework.Plus;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            IncreasePrices(db);
        }

        //15 with BULK
        public static void IncreasePricesWithBULK(BookShopContext context)
        {
            context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .Update(e => new Book(){Price = e.Price + 5});
        }


        //15
            public static void IncreasePrices(BookShopContext context)
        {
            IQueryable<Book> bookPrices = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);
               

            foreach (var price in bookPrices)
            {
                price.Price += 5;
            }
        }

        //14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categories = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate.Value)
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            BookReleaseYear = cb.Book.ReleaseDate.Value.Year
                        })
                        .Take(3)
                        .ToArray()
                })
                .OrderBy(n => n.Name)
                .ToArray();

            foreach (var nameYearTitle in categories)
            {
                sb.AppendLine(nameYearTitle.Name);

                foreach (var titBookYear in nameYearTitle.MostRecentBooks)
                {
                    sb.AppendLine($"{titBookYear.BookTitle} ({titBookYear.BookReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();


        }

        //12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
           
            string[] categoriesProfits = context
                .Categories
                .Select(x => new
                {
                    x.Name,
                    Profit = x.CategoryBooks
                        .Sum(x => x.Book.Copies * x.Book.Price)
                })
                .OrderBy(e => e.Name)
                .OrderByDescending(e=>e.Profit)
                .Select(e=>$"{e.Name} ${e.Profit:f2}")
                .ToArray();


            return String.Join(Environment.NewLine, categoriesProfits);

        }

        //12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            string[] authorCopies = context
                .Authors
                .Select(m => new
                {
                    m.FirstName,
                    m.LastName,
                    cpiess = m
                        .Books
                        .Sum(s => s.Copies)
                })
                .OrderByDescending(c=>c.cpiess)
                .Select(e => $"{e.FirstName} {e.LastName} - {e.cpiess}")
                .ToArray();

            return String.Join(Environment.NewLine, authorCopies);

        }

        //8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] names = context
                .Authors
                .Where(f => f.FirstName.EndsWith(input))
                .Select(e => $"{e.FirstName} {e.LastName}")
                .ToArray()
                .OrderBy(e => e)
                .ToArray();


            return String.Join(Environment.NewLine, names);
         


           
           
        }

        //5
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();


            var notReleasedBook = context
                .Books
                .Where(r => r.ReleaseDate.Value.Year != year)
                .Select(t => t)
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach (var book in notReleasedBook)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //3
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var prBooks = context
                .Books
                .Where(s => s.Price > 40)
                .Select(t =>t)
                .OrderByDescending(p=>p.Price)
                .ToArray();

            foreach (var bk in prBooks)
            {
                sb.AppendLine($"{bk.Title} - ${bk.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //1
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            AgeRestriction ageRestriction;
            bool hasParsed = Enum.TryParse<AgeRestriction>(command, true, out ageRestriction);

            if (!hasParsed)
            {
                return String.Empty;
            }

            string[] books = context
                .Books
                .Where(a => a.AgeRestriction == ageRestriction)
                .Select(t => t.Title)
                .OrderBy(t => t)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //2
        public static string GetGoldenBooks(BookShopContext context)
        {


            var goldenBooks = context
                .Books
                .Where(e => e.EditionType == EditionType.Gold && e.Copies < 5000)
                .OrderBy(e => e.BookId)
                .Select(r => r.Title)
                .ToArray();

            return String.Join(Environment.NewLine, goldenBooks);

        }
    }
}
