using API.Dtos;
using AutoMapper;
using Core.Models;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            this._config = config;
        }

/// <summary>
/// 
/// </summary>
/// <param name="product"></param>
/// <param name="productDto"></param>
/// <param name="destMember"></param>
/// <param name="context"></param>
/// <returns></returns>
        public string Resolve(Product product, ProductToReturnDto productDto, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                return _config["ApiUrl"] + product.PictureUrl;
            }
            return null;
        }
    }
}