using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Films.Entities
{
    public class MovieList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string Studios { get; set; }
        public string Producer { get; set; }
        public string Winner { get; set; }

    }

     public class MovieMap
     {

        public MovieMap()
        {

        }
        public int year { get; set; }
        public string title { get; set; }
        public string studios { get; set; }
        public string producers { get; set; }
        public string winner { get; set; }
    }
    
}

    


