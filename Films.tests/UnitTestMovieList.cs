using Films.Contexts;
using Films.Controllers;
using Films.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Xunit;


namespace Films.tests
{
    public class APIUnitTest
    {
        [Fact]
        public void CheckDataBase()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<SQLiteDbContext>().UseSqlite(connection).Options;

            using (var context = new SQLiteDbContext(options))
            {
                context.Database.EnsureCreated();

                context.MovieList.Add(new MovieList { id = 1, Producer = "Thiago", Studios = "Studio1", Title = "Filme", Winner = "yes", Year = 2022 });
                context.SaveChanges();



                var x = context.MovieList.Where(x => x.id == 1).ToList();
                Assert.Equal("Thiago", x[0].Producer);
                Assert.True(x.Count == 1);
            }


        }

        [Fact]
        public void CheckDataFromMovieListHttpRequest()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<SQLiteDbContext>().UseSqlite(connection).Options;

            
            using (var context = new SQLiteDbContext(options))
            {
                context.Database.EnsureCreated();

                context.MovieList.Add(new MovieList { id = 1, Producer = "John Doe", Studios = "Studio1", Title = "Filme1", Winner = "yes", Year = 2022 });
                context.MovieList.Add(new MovieList { id = 2, Producer = "John Doe", Studios = "Studio1", Title = "Filme2", Winner = "yes", Year = 2023 });
                context.MovieList.Add(new MovieList { id = 3, Producer = "Thiago D.", Studios = "Studio1", Title = "Filme3", Winner = "yes", Year = 1980 });
                context.MovieList.Add(new MovieList { id = 5, Producer = "Thiago D.", Studios = "Studio1", Title = "Filme4", Winner = "yes", Year = 1989 });
                context.MovieList.Add(new MovieList { id = 6, Producer = "Joe A.", Studios = "Studio1", Title = "Filme5", Winner = "yes", Year = 1990 });
                context.MovieList.Add(new MovieList { id = 7, Producer = "Joe A.", Studios = "Studio1", Title = "Filme6", Winner = "yes", Year = 1995 });
                context.MovieList.Add(new MovieList { id = 8, Producer = "Max M.", Studios = "Studio1", Title = "Filme7", Winner = "yes", Year = 1990 });
                context.MovieList.Add(new MovieList { id = 9, Producer = "Max M.", Studios = "Studio1", Title = "Filme8", Winner = "yes", Year = 1999 });

                context.SaveChanges();

                // arrange
                var controller = new MovieListsController(context);
                // act
                IActionResult result = controller.GetMovieList();
                var responseResult = Assert.IsType<ContentResult>(result);
                var content = responseResult.Content;
                JObject json = JObject.Parse(content);
                var min = json["min"];
                var max = json["max"];
                //assert
                Assert.True(min.Count() == 1);
                Assert.True(max.Count() == 2);
                Assert.Equal("John Doe", min[0]["producer"]);
                Assert.Equal("Max M.", max[0]["producer"]);
                Assert.Equal("Thiago D.", max[1]["producer"]);
            }

            
        }

        


    }
}
