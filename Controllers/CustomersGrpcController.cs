using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using GrpcCustomersService;
using Apetrei_Alexandru_Lab2.Models;
using System;

using GrpcCustomer = GrpcCustomersService.Customer;
using MvcCustomer = Apetrei_Alexandru_Lab2.Models.Customer;

namespace Apetrei_Alexandru_Lab2.Controllers
{
    public class CustomersGrpcController : Controller
    {
        private readonly GrpcChannel channel;

        public CustomersGrpcController()
        {
            // Schimbă portul dacă diferă în proiectul tău gRPC
            channel = GrpcChannel.ForAddress("https://localhost:7090");
        }

        // Afișează toți clienții
        [HttpGet]
        public IActionResult Index()
        {
            var client = new CustomerService.CustomerServiceClient(channel);

            CustomerList cust = client.GetAll(new Empty());

            return View(cust);
        }

        // Formul de creare client
        public IActionResult Create()
        {
            return View();
        }

        // Creare client prin gRPC
        [HttpPost]
        public IActionResult Create(MvcCustomer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new CustomerService.CustomerServiceClient(channel);

                    // Mapare MVC -> gRPC
                    // ❌ NU seta CustomerId, serverul gRPC îl va genera automat
                    var grpcCustomer = new GrpcCustomer
                    {
                        Name = customer.Name,
                        Adress = customer.Adress,
                        Birthdate = customer.BirthDate.ToString("yyyy-MM-dd") // proto cere string
                    };

                    // Trimitere la serverul gRPC
                    client.Insert(grpcCustomer);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Prinde erorile pentru debugging
                    ModelState.AddModelError("", "Eroare la salvare: " + ex.Message);
                    return View(customer);
                }
            }

            return View(customer);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new CustomerService.CustomerServiceClient(channel);

            // Folosește alias-ul GrpcCustomer pentru obiectele gRPC
            GrpcCustomer customer = client.Get(new CustomerId() { Id = (int)id });

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer); // trimite obiectul gRPC la View
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            Empty response = client.Delete(new CustomerId()
            {
                Id = id
            });
            return RedirectToAction(nameof(Index));
        }

        // GET: CustomersGrpc/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var client = new CustomerService.CustomerServiceClient(channel);
            GrpcCustomer customer = client.Get(new CustomerId() { Id = (int)id });

            if (customer == null)
                return NotFound();

            return View(customer); // trimite obiectul gRPC la View
        }

        // POST: CustomersGrpc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, GrpcCustomer customer)
        {
            if (id != customer.CustomerId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);

                // Trimite update la serverul gRPC
                client.Update(customer);

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

    }
}
