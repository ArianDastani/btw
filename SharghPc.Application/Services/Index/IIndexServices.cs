
using Microsoft.AspNetCore.Http;
using SharghPc.DataLayer.DTOs.Index;
using SharghPc.DataLayer.Entites.Site;

namespace SharghPc.Application.Services.Index
{
    public interface IIndexServices:IAsyncDisposable
    {
        public Task<bool> AddIndexCategoryArea(AddIndexCategoryAreaDto categoryAreaDto, IFormFile image);
        public Task<bool> RemoveIndexCategoryArea(long categoryId);
        public Task<List<IndexCategoryArea>> GetIndexCategoryArea();
        public Task<EditIndexCategoryAreaDto> GetIndexCategoryArea(long categoryId);
        public Task<bool> EditCategoryAreas(EditIndexCategoryAreaDto categoryAreaDto,IFormFile image);
    }
}
