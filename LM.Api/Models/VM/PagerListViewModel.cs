using System.Collections.Generic;
using PagedList;

namespace LM.Api.Models.VM
{
    public class PagerListViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public IPagedList Pager { get; set; }
    }
}