using Core.Models;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        /// <summary>
        /// The method gets filters as arguments and includes it to 
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="brandId"></param>
        /// <param name="typeId"></param>
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
        : base(x =>
        //Check if Search parameter is null. If it doesn't, execute what is on right side of the operation.
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
        //If there is any value in brandId or typeId, what is on the right side of Or Operator will be our query to be sent to the base
       (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
       (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
        )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            //The default case, when there is no sort specified, is bring the list ordered by name in alphabetical order.
            AddOrderBy(x => x.Name);
            //Gets an amount of records to Skip (given an array of records) and to Take in the page.
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            //Defines what happens following the value of sort parameter.
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(product => product.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(product => product.Price);
                        break;
                    default:
                        AddOrderBy(product => product.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}