using Catalog.API.Protos;
using Catalog.API.Repositories;
using Grpc.Core;

namespace Catalog.API.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _repository.GetProduct(request.Id);
            if (product == null) throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = Convert.ToDouble(product.Price), // Basitlik için double cast yapabilirsin
                Category = product.Category
            };
        }
    }
}