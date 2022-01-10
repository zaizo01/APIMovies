using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviesFilterDTO
    {
        public int Page { get; set; } = 1;
        public int NumberOfRecordsPerPage { get; set; } = 10;
        public PaginacionDTO Paginacion
        {
            get { return new PaginacionDTO() { Page = Page, NumberOfRecordsPerPage = NumberOfRecordsPerPage }; }
        }

        public string Title { get; set; }
        public int GenderId { get; set; }
        public bool InTheaters { get; set; }
        public bool UpcomingMovies { get; set; }
        public string FieldToSort { get; set; }
        public bool SortAsc { get; set; } = true;
    }
}
