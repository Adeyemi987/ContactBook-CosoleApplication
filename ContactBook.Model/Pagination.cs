  namespace ContactBook.Model
{
    public class Pagination
    {
        const int maxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        public int _PageSize { get; set; } = 10;

        public int PageSize
        {

            get { return PageSize; }
            set
            {
                _PageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
 