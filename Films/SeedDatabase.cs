using CsvHelper;
using CsvHelper.Configuration;
using Films.Contexts;
using Films.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace Films
{
    public static class SeedDatabase
    {
        
        public static void Initialize(IServiceProvider serviceProvider)
        {
            IWebHostEnvironment _env = serviceProvider.GetRequiredService<IWebHostEnvironment>();


            using (var context = new SQLiteDbContext(serviceProvider.GetRequiredService<DbContextOptions<SQLiteDbContext>>()))
            {

                if (context.MovieList.Any())
                {
                    return;
                }

                var path = Path.Combine(_env.ContentRootPath, "Data/movielist.csv");

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<MovieMap>().ToList();
                    foreach (var rec in records)
                    {
                        string[] producers = (rec.producers.Replace(" and", ",")).Split(", ");
                        foreach(var producer in producers)
                        {
                            context.MovieList.Add(
                                new Entities.MovieList
                                {
                                    Year = rec.year,
                                    Producer = producer,
                                    Studios = rec.studios,
                                    Title = rec.title,
                                     Winner = rec.winner
                                }
                              );
                        }
                    }
                    context.SaveChanges();
                }

                                
            }
        }
        
    }
    

}