using Core.Models;

namespace Core.Specifications
{
    /// <summary>
    /// Gets the number of items (products) available after the filter be applied to populate the page.
    /// It's useful to pagination purposes
    /// </summary>
    public class ProducWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProducWithFiltersForCountSpecification(ProductSpecParams productParams) : base(
            x =>
        //Check if Search parameter is null. If it doesn't, execute what is on right side of the operation.
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
       //If is there any value in brandId or typeId (from productParams), what is on the right side of Or Operator will be our query to be sent to the base
       (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
       (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
        )
        {
        }
    }
}