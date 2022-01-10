namespace MoviesAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Page { get; set; } = 1;
        private int numberOfRecordsPerPage = 10;
        private readonly int maximumNumberOfRecordsPerPage = 50;

        public int NumberOfRecordsPerPage { get => numberOfRecordsPerPage; 
                                            set {
                numberOfRecordsPerPage = (value > maximumNumberOfRecordsPerPage) ? maximumNumberOfRecordsPerPage : value;
                                                } }
    }
}
