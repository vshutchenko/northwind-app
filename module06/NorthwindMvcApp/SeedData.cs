using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Northwind.Services.Customers;
using Northwind.Services.Employees;
using NorthwindMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NorthwindMvcApp
{
    public class SeedData
    {
        private readonly IdentityContext identityContext;
        private readonly HttpClient client;
        public SeedData(IdentityContext identityContext, Uri apiBaseUrl)
        {
            this.identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
            apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));

            this.client = new HttpClient
            {
                BaseAddress = apiBaseUrl
            };
        }

        public void SeedEmployees()
        {
            var json = this.client.GetStringAsync("api/employees").Result;

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            foreach (var e in employees)
            {
                if (this.identityContext.Users.Where(u => u.Name == $"{e.FirstName} {e.LastName}").FirstOrDefault() is null)
                {
                    this.identityContext.Users.Add(new User
                    {
                        Name = $"{e.FirstName} {e.LastName}",
                        PasswordHash = "12345",
                        RoleId = 2,
                        NorthwindDbId = e.Id.ToString(),
                    });
                }
            }

            this.identityContext.SaveChanges();
        }

        public void SeedCustomers()
        {
            var json =  this.client.GetStringAsync("api/customers").Result;

            var customers = JsonConvert.DeserializeObject<List<Customer>>(json);

            foreach (var c in customers)
            {
                if (this.identityContext.Users.Where(u => u.Name == c.CompanyName).FirstOrDefault() is null)
                {
                    this.identityContext.Users.Add(new User
                    {
                        Name = c.CompanyName,
                        PasswordHash = "12345",
                        RoleId = 3,
                        NorthwindDbId = c.Id,
                    });
                }  
            }

            this.identityContext.SaveChanges();
        }
    }
}
