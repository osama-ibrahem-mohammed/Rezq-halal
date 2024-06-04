using AutoMapper;
using Core.DTO;
using Core.Entity;
using Infrastructur.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<List<ProductDTO>> Get()
        {
            var products= await unitOfWork.ProductRepo.GetAll("ProductType,ProductBrand");
            var productDto= mapper.Map<IEnumerable<ProductDTO>>(products);
            return productDto.ToList();
        }
    }
}
