using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using skinet.API.DTOs;

namespace skinet.API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
                return _configuration["ApiUrl"] + source.PictureUrl;

            return null;
        }
    }
}