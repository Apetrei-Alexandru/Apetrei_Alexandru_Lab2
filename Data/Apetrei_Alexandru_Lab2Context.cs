using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apetrei_Alexandru_Lab2.Models;

namespace Apetrei_Alexandru_Lab2.Data
{
    public class Apetrei_Alexandru_Lab2Context : DbContext
    {
        public Apetrei_Alexandru_Lab2Context (DbContextOptions<Apetrei_Alexandru_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Apetrei_Alexandru_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Apetrei_Alexandru_Lab2.Models.Customer> Customer { get; set; } = default!;
        public DbSet<Apetrei_Alexandru_Lab2.Models.Genre> Genre { get; set; } = default!;
    }
}

