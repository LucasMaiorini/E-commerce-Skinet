namespace Core.Specifications
{
    /// <summary>
    /// Parameters used to configure the products pagination and filters.
    /// </summary>
    public class ProductSpecParams
    {
        //A page could only have this amount of item.
        public const int MaxPageSize = 50;

        //By default we return the page 1. It represents which page the user is.
        public int PageIndex { get; set; } = 1;

        //The client will override this value respecting the MaxPageSize.
        //It represents how many items a page could have.
        private int _pageSize = 6;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        //Is used to filter the query by brand.
        //e.g. /api/products?typeId=3&brandId=2
        public int? BrandId { get; set; }

        //Is used to filter the query by type.
        //e.g. /api/products?typeId=3&brandId=2
        public int? TypeId { get; set; }

        //Is used to set how the results will be displayed.
        public string Sort { get; set; }

        
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}