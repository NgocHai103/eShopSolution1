using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eShopSolution.Data.EF
{
    class EShopSolutionDbContextFactory : IDesignTimeDbContextFactory<EShopDBContext>
    {
        public EShopDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();
            var connectString = configuration.GetConnectionString("eShopSolutionDb"); 
            var optionsBuilder = new DbContextOptionsBuilder<EShopDBContext>();
            optionsBuilder.UseSqlServer(connectString);
            return new EShopDBContext(optionsBuilder.Options);
        }
    }
}
