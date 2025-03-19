using Microsoft.AspNetCore.Http;
using SharghPc.DataLayer.DTOs.Product;
using SharghPc.DataLayer.Entites.Product;

namespace SharghPc.Application.Services.Product
{
    public interface IProductServices:IAsyncDisposable
    {
        public Task<FilterProductDto> FilterProduct(FilterProductDto ProductDto);

        public Task<List<ProductCategory>> GetMainProductCategoryByParentId(long? parentId);
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId);

        public Task<List<ProductCategory>> GetAllActiveProductCategories();
        public Task<bool> AddProduct(AddProductDto ProductDto, IFormFile image);

        public Task<List<ProductFeatureDto>> GetProductFeature(long productId);
        public Task<bool> AddProductFeature(long productId, ProductFeatureDto productFeature);
        public Task<bool> RemoveProductFeature(long Id);
        public Task<EditProductDto> GetProductForEdit(long productId);

        public Task<bool> EditProduct(EditProductDto productDto,IFormFile image);
        public Task<bool> SetToEndOfInventory(long productId);

        public Task<bool> RemoveProduct(long id);
        public Task<bool> ActivatedProduct(long id);

        public Task<List<ProductGalleriesDto>> GetAllProductGallery(long productId);

        public Task<AddProductImageGalleryResult> AddProductImageGallery(long productId,IFormFile image);
        public Task<bool> RemoveProductImageGallery(long imageId);

        public Task<ProductDetailDto> GetProductDetailById(long productId);

        public Task<List<DataLayer.Entites.Product.Product>> GetProductNameForSearch(string productName);

        public Task<List<ProductForIndexDto>> GetProductForIndexDto(string? UrlName);
        public Task<List<ProductForIndexDto>> GetAllProductForIndexDto();

        public Task<bool> AddToSpecialProduct(long id);

        public Task<DataLayer.Entites.Product.Product> GetSpecialProduct();
        
    }
}
