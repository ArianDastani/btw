using SharghPc.DataLayer.DTOs.Category;
using SharghPc.DataLayer.Entites.Product;

namespace SharghPc.Application.Services.Category
{
    public interface ICategoryServices : IAsyncDisposable
    {
        public Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId);
        public Task<bool> AddNewCategory(AddNewCategoryDto categoryDto);
        public Task<bool> EditCategories(EditCategoriesDto categoriesDto);
        public Task<bool> DeleteCategory(long id);
        public Task<EditCategoriesDto> GetCategoriesForEdit(long id);


    }
}
