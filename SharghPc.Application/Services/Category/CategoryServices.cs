using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Category;
using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.Category
{
    public class CategoryServices:ICategoryServices
    {
        #region ctor

        private IGenericRepository<ProductCategory> _categoryRepository;

        public CategoryServices(IGenericRepository<ProductCategory> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion

        public async ValueTask DisposeAsync()
        {
            await _categoryRepository.DisposeAsync();
        }

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId)
        {
            if (parentId == 0 || parentId == null)
            {
                  var category= await _categoryRepository.GetQuery().Include(x=>x.Parent)
                    .Where(x => x.ParentId == null && x.IsDelete == false)
                    .ToListAsync();
                  return category;
            }

            var subCategory = await _categoryRepository.GetQuery().Include(x=>x.Parent)
                .Where(x => x.ParentId == parentId && x.IsDelete == false)
                .ToListAsync();
            return subCategory;
        }

        public async Task<bool> AddNewCategory(AddNewCategoryDto categoryDto)
        {
            try
            {
                if (categoryDto.ParentId == 0 || categoryDto.ParentId == null)
                {
                    ProductCategory productCategory = new ProductCategory()
                    {
                        ParentId = null,
                        CreateDate = DateTime.Now,
                        IsActive = true,
                        IsDelete = false,
                        UrlName = categoryDto.UrlName,
                        Title = categoryDto.Title,

                    };

                    await _categoryRepository.AddEntity(productCategory);
                    await _categoryRepository.SaveChanges();
                    return true;
                }

                ProductCategory subCategory = new ProductCategory()
                {
                    ParentId = categoryDto.ParentId,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                    IsDelete = false,
                    UrlName = categoryDto.UrlName,
                    Title = categoryDto.Title,

                };

                await _categoryRepository.AddEntity(subCategory);
                await _categoryRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> EditCategories(EditCategoriesDto categoriesDto)
        {
            if (categoriesDto.ProductCategoryId == 0 || categoriesDto.ProductCategoryId == null) 
            {
                return false;
            }

            var category = await _categoryRepository.GetEntityById(categoriesDto.ProductCategoryId);

            if (category == null)
            {
                return false;
            }

            category.UrlName = categoriesDto.UrlName;
            category.Title = categoriesDto.Title;
            category.LastUpdateDate= DateTime.Now;
            category.IsActive = true;

            _categoryRepository.EditEntity(category);
            await _categoryRepository.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteCategory(long id)
        {
            if (id == 0 || id == null) return false;

            var cate=await _categoryRepository.GetEntityById(id);

            if (cate == null) return false;
           
            cate.LastUpdateDate=DateTime.Now;
            cate.IsDelete = true;
            _categoryRepository.EditEntity(cate);
            await _categoryRepository.SaveChanges();
            return true;
        }

        public async Task<EditCategoriesDto> GetCategoriesForEdit(long id)
        {
            if (id == 0 || id == null)
            {
                return null;
            }

            var res = await _categoryRepository.GetEntityById(id);

            if (res == null)
            {
                return null;
            }

            return new EditCategoriesDto()
            {
                ProductCategoryId = res.Id,
                Title = res.Title,
                UrlName = res.UrlName,
            };
        }
    }
}
