using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Films.Contexts;
using Films.Entities;
using Newtonsoft.Json;

namespace Films.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieListsController : ControllerBase
    {
        private readonly SQLiteDbContext _context;

        public MovieListsController(SQLiteDbContext context)
        {
            _context = context;
        }

        // GET: api/MovieLists
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetMovieList()
        {

            //main json
            var obj = new MainObj();
            GetMinMaxIntervals(obj);

            //obj.min = new List<ProducerInterval>();
            //obj.min.Add(new ProducerInterval { producer = "p1", interval = 2, followingWin = 2011, previousWin = 2015 });

            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        private void GetMinMaxIntervals(MainObj obj)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {

                //max interval obj
                obj.max = new List<ProducerInterval>();
                //busca maiores intervalos
                GetMaxIntervals(obj, command);
                //min interval obj
                obj.min = new List<ProducerInterval>();
                //busca menores intervalos
                GetMinIntervals(obj, command);
            }
        }

        private static void GetMinIntervals(MainObj obj, System.Data.Common.DbCommand command)
        {
            List<string> producers = GetProducersWithWins(command);

            GetWinsAndFindSmallestInterval(obj, command, producers);
        }

        private static void GetWinsAndFindSmallestInterval(MainObj obj, System.Data.Common.DbCommand command, List<string> producers)
        {
            int min_interval = 9999;
            foreach (var producer in producers)
            {
                command.CommandText = "select producer, year from movielist where winner = 'yes' and producer = '" + producer + "' group by producer, year order by year desc";
                List<int> years = new List<int>();
                using (var result = command.ExecuteReader())
                {

                    while (result.Read())
                    {
                        int year = Int32.Parse(result.GetValue(1).ToString());
                        years.Add(year);
                    }
                }
                int total_years = years.Count;



                var cont = 1;
                foreach (int year in years)
                {

                    for (var i = cont; i < total_years; i++)
                    {
                        int next_year = Int32.Parse(years[i].ToString());
                        int interval = year - next_year;
                        if (interval <= min_interval)
                        {
                            if (interval < min_interval)
                            {
                                obj.min.Clear();
                            }

                            obj.min.Add(new ProducerInterval { producer = producer, interval = interval, followingWin = year, previousWin = next_year });

                            min_interval = interval;
                        }
                    }
                    cont = cont + 1;

                }

            }
        }

        private static List<string> GetProducersWithWins(System.Data.Common.DbCommand command)
        {
            command.CommandText = "select producer, count(producer) from movielist where winner = 'yes' group by producer  having count(producer) > 1  order by producer";
            List<string> producers = new List<string>();
            using (var result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    string producer = result.GetValue(0).ToString();
                    int prizes = Int32.Parse(result.GetValue(1).ToString());
                    producers.Add(producer);


                }
            }

            return producers;
        }

        private void GetMaxIntervals(MainObj obj, System.Data.Common.DbCommand command)
        {
            command.CommandText = "  SELECT producer,  min(year) as minyear, max(year) as maxyear, max(year)-min(year) as diff  from movielist where winner = 'yes' group by producer having max(year) - min(year) = (select max(year) - min(year) from movielist  where winner = 'yes' group by producer order by max(year) - min(year) desc limit 1)";
            _context.Database.OpenConnection();
            using (var result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    //Console.WriteLine(result.GetValue(0) + " " + result.GetValue(3));
                    obj.max.Add(new ProducerInterval { producer = result.GetValue(0).ToString(), interval = Int32.Parse(result.GetValue(3).ToString()), previousWin = Int32.Parse(result.GetValue(1).ToString()), followingWin = Int32.Parse(result.GetValue(2).ToString()) });
                }
            }
        }



        // GET: api/MovieLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieList>> GetMovieList(int id)
        {
            var movieList = await _context.MovieList.FindAsync(id);

            if (movieList == null)
            {
                return NotFound();
            }

            return movieList;
        }

        // PUT: api/MovieLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieList(int id, MovieList movieList)
        {
            if (id != movieList.id)
            {
                return BadRequest();
            }

            _context.Entry(movieList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MovieLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MovieList>> PostMovieList(MovieList movieList)
        {
            _context.MovieList.Add(movieList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieList", new { id = movieList.id }, movieList);
        }

        // DELETE: api/MovieLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovieList>> DeleteMovieList(int id)
        {
            var movieList = await _context.MovieList.FindAsync(id);
            if (movieList == null)
            {
                return NotFound();
            }

            _context.MovieList.Remove(movieList);
            await _context.SaveChangesAsync();

            return movieList;
        }

        private bool MovieListExists(int id)
        {
            return _context.MovieList.Any(e => e.id == id);
            
        }
    }
}
