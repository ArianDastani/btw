
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Index;
using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Entites.Site;
using SharghPc.DataLayer.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace SharghPc.Application.Services.Index
{
    public class IndexServices:IIndexServices
    {
        #region ctor

        IGenericRepository<IndexCategoryArea> _repository;

        public IndexServices(IGenericRepository<IndexCategoryArea> repository)
        {
            _repository = repository;
        }

        #endregion

        public async ValueTask DisposeAsync()
        {
            await _repository.DisposeAsync();
        }

        public async Task<bool> AddIndexCategoryArea(AddIndexCategoryAreaDto categoryAreaDto,IFormFile image)
        {
            try
            {
                string imagename = "";

                if (image != null)
                {
                    imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                    await image.SaveFileToServer($"{Directory.GetCurrentDirectory()}/wwwroot/content/CategoryArea/", imagename);
                }


                IndexCategoryArea indexCategory = new IndexCategoryArea()
                {
                    IsDelete = false,
                    ImageName = imagename,
                    CreateDate = DateTime.Now,
                    UrlName = categoryAreaDto.UrlName,
                    Title = categoryAreaDto.Title,
                };

                await _repository.AddEntity(indexCategory);
                await _repository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveIndexCategoryArea(long categoryId)
        {
            if (categoryId == 0 || categoryId == null)
            {
                return false;
            }

            var category = await _repository.GetEntityById(categoryId);

            if (category == null)
            {
                return false;
            }

            category.IsDelete = true;
            _repository.EditEntity(category);
            await _repository.SaveChanges();
            return true;
        }

        public async Task<List<IndexCategoryArea>> GetIndexCategoryArea()
        {
            return await _repository.GetQuery()
                .Where(x => x.IsDelete == false)
                .ToListAsync();
        }

        public async Task<EditIndexCategoryAreaDto> GetIndexCategoryArea(long categoryId)
        {
            if (categoryId == 0 || categoryId == null)
            {
                return null;
            }

            var category = await _repository.GetEntityById(categoryId);

            if (category == null)
            {
                return null;
            }

            return new EditIndexCategoryAreaDto()
            {
                Id = category.Id,
                Title = category.Title,
                UrlName = category.UrlName,
            };


        }

        public async Task<bool> EditCategoryAreas(EditIndexCategoryAreaDto categoryAreaDto, IFormFile image)
        {
            if (categoryAreaDto.Id == 0)
            {
                return false;
            }

            var categoryarea = await _repository.GetEntityById(categoryAreaDto.Id);

            if (categoryarea == null)
            {
                return false;
            }

            string imagename = "";

            if (image != null)
            {
                imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                await image.SaveFileToServer($"{Directory.GetCurrentDirectory()}/wwwroot/content/productimgae/", imagename);
                categoryarea.ImageName = imagename;
            }

            categoryarea.Title = categoryAreaDto.Title;
            categoryAreaDto.UrlName=categoryarea.UrlName;

            _repository.EditEntity(categoryarea);
            await _repository.SaveChanges();
            return true;
        }
    }
}
