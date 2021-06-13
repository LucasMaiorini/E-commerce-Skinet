using System.Collections.Generic;

namespace API.Helpers
{
    //We can paginate anything <T>
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        //How many items are available after the filter be applied
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}