using System.Collections.Generic;

namespace Core.Web.Extensions
{
    public class PageCollectionInfo<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<T> PageCollection { get; set; } = new List<T> { };  

        public int ItemsCount { get; set; }

    }
}
