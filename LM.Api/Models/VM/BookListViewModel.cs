using LM.Api.Models.DB;

namespace LM.Api.Models.VM
{
    public class BookListViewModel : PagerListViewModel<Book>
    {
        public string Search { get; set; }

        public string Sort { get; set; }

        public string Column { get; set; }

        public BookListViewModel()
        {
            Column = "id";
            Sort = "asc";
        }
    }
}