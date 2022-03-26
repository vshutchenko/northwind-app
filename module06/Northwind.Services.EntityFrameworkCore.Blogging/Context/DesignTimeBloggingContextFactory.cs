using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    /// <summary>
    /// Factory to provide data base context for migrations.
    /// </summary>
    public class DesignTimeBloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        /// <summary>
        /// Provide data base context.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Context.</returns>
        public BloggingContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_NORTHWIND_BLOGGING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"{connectionString} environment variable is not set.");
            }

            Console.WriteLine($"Using {connectionString} environment variable as a connection string.");

            var builderOptions = new DbContextOptionsBuilder<BloggingContext>().UseSqlServer(connectionString).Options;
            return new BloggingContext(builderOptions);
        }
    }
}
