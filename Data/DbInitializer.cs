using Apetrei_Alexandru_Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Apetrei_Alexandru_Lab2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Apetrei_Alexandru_Lab2Context(
                serviceProvider.GetRequiredService<DbContextOptions<Apetrei_Alexandru_Lab2Context>>()))
            {
                if (context.Book.Any())
                {
                    return; // BD a fost creata anterior
                }

                // ⭐ 1. Adăugăm autori
                context.Author.AddRange(
                    new Author { FirstName = "Mihail", LastName = "Sadoveanu" },
                    new Author { FirstName = "George", LastName = "Calinescu" },
                    new Author { FirstName = "Mircea", LastName = "Eliade" }
                );

                context.SaveChanges(); // salvăm pentru a primi ID-urile autorilor

                // Preluăm ID-urile generate
                var sadoveanu = context.Author.Single(a => a.LastName == "Sadoveanu").ID;
                var calinescu = context.Author.Single(a => a.LastName == "Calinescu").ID;
                var eliade = context.Author.Single(a => a.LastName == "Eliade").ID;

                // ⭐ 2. Adăugăm cărți cu AuthorID, nu string Author
                context.Book.AddRange(
                    new Book
                    {
                        Title = "Baltagul",
                        Price = Decimal.Parse("22"),
                        AuthorID = sadoveanu
                    },
                    new Book
                    {
                        Title = "Enigma Otiliei",
                        Price = Decimal.Parse("18"),
                        AuthorID = calinescu
                    },
                    new Book
                    {
                        Title = "Maytrei",
                        Price = Decimal.Parse("27"),
                        AuthorID = eliade
                    }
                );

                
                context.Genre.AddRange(
                    new Genre { Name = "Roman" },
                    new Genre { Name = "Nuvela" },
                    new Genre { Name = "Poezie" }
                );

                
                context.Customer.AddRange(
                    new Customer
                    {
                        Name = "Popescu Marcela",
                        Adress = "Str. Plopilor, nr. 24",
                        BirthDate = DateTime.Parse("1979-09-01")
                    },
                    new Customer
                    {
                        Name = "Mihailescu Cornel",
                        Adress = "Str. Bucuresti, nr.45,ap. 2",
                        BirthDate = DateTime.Parse("1969-07-08")
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
