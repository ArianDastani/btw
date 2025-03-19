using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Product;
using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Entites.Products;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.Product
{
    public class ProductServices : IProductServices
    {
        #region Ctof

        private IGenericRepository<DataLayer.Entites.Product.Product> _productRepository;
        private IGenericRepository<DataLayer.Entites.Product.ProductCategory> _categoryRepository;
        private IGenericRepository<DataLayer.Entites.Product.ProductSelectedCategory> _selectCategoryRepository;
        private IGenericRepository<ProductFeature> _productFeature;
        private IGenericRepository<ProductGallery> _productGallery;

        public ProductServices(IGenericRepository<DataLayer.Entites.Product.Product> productRepository, IGenericRepository<ProductCategory> categoryRepository, IGenericRepository<ProductSelectedCategory> selectCategoryRepository, IGenericRepository<ProductFeature> productFeature, IGenericRepository<ProductGallery> productGallery)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _selectCategoryRepository = selectCategoryRepository;
            _productFeature = productFeature;
            _productGallery = productGallery;
        }

        #endregion

        public async Task<List<ProductFeatureDto>> GetProductFeature(long productId)
        {
            if (productId == 0 || productId == null)
            {
                return null;
            }

            return await _productFeature.GetQuery()
                .Where(x => x.ProductId == productId && x.IsDelete == false)
                .Select(x => new ProductFeatureDto()
                {
                    Id = x.Id,
                    FeatureTitle = x.FeatureTitle,
                    FeatureValue = x.FeatureValue,
                })
                .ToListAsync();


        }

        public async Task<FilterProductDto> FilterProduct(FilterProductDto ProductDto)
        {
            var queris = _productRepository.GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .ThenInclude(x => x.ProductCategory)
                .Where(x => x.IsDelete == false)
                .OrderByDescending(x=>x.CreateDate)
                .AsQueryable();

            switch (ProductDto.FilterProductOrderBy)
            {
                case FilterProductOrderBy.CreateDate_Des:
                    queris = queris.OrderByDescending(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.CreateDate_Asc:
                    queris = queris.OrderBy(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.Price_Des:
                    queris = queris.OrderByDescending(x => x.Price);
                    break;
                case FilterProductOrderBy.Price_Asc:
                    queris = queris.OrderBy(x => x.Price);
                    break;
            }

            if (!string.IsNullOrEmpty(ProductDto.Category))
            {
                queris = queris.Where(x =>
                    x.ProductSelectedCategories.Any(f => f.ProductCategory.UrlName == ProductDto.Category));
            }

            if (!string.IsNullOrWhiteSpace(ProductDto.Title))
            {
                queris = queris.Where(x => EF.Functions.Like(x.Title, $"%{ProductDto.Title}%"));
            }

            return ProductDto.SetProduct(await queris.ToListAsync());
        }

        public async Task<List<ProductCategory>> GetMainProductCategoryByParentId(long? parentId)
        {
            if (parentId == null || parentId == 0)
            {
                return await _categoryRepository.GetQuery()
                    .Where(x => x.IsActive == true && x.IsDelete == false && x.ParentId == null)
                    .ToListAsync();
            }

            return await _categoryRepository.GetQuery()
                .Where(x => x.IsActive == true && x.IsDelete == false && x.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return await _categoryRepository.GetQuery()
                .Where(x => x.IsActive == true && x.IsDelete == false)
                .ToListAsync();
        }

        public async Task<bool> AddProduct(AddProductDto ProductDto, IFormFile image)
        {
            string imagename = "";

            if (image != null)
            {
                imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                await image.SaveFileToServer($"{Directory.GetCurrentDirectory()}/wwwroot/content/productimgae/", imagename);
            }

            DataLayer.Entites.Product.Product product = new DataLayer.Entites.Product.Product()
            {
                Price = ProductDto.Price,
                CreateDate = DateTime.Now,
                Description = ProductDto.Description,
                ShortDescription = ProductDto.ShortDescription,
                ImageName = imagename,
                Inventory = ProductDto.Inventory,
                IsActive = true,
                IsDelete = false,
                Title = ProductDto.Title,
                ViewConter = "0",
                IsSpecial = false,
            };

            await _productRepository.AddEntity(product);
            await _productRepository.SaveChanges();

            //Categories
            List<ProductSelectedCategory> productSelectedCategories = new List<ProductSelectedCategory>();

            foreach (var categoryId in ProductDto.SelectCategories)
            {
                productSelectedCategories.Add(new ProductSelectedCategory()
                {
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    ProductId = product.Id,
                    ProductCategoryId = categoryId,
                });
            }

            await _selectCategoryRepository.AddRangeEntity(productSelectedCategories);
            await _selectCategoryRepository.SaveChanges();


            //Feature
            List<ProductFeature> features = new List<ProductFeature>();

            foreach (var item in ProductDto.ProductFeatures)
            {
                features.Add(new ProductFeature()
                {
                    CreateDate = DateTime.Now,
                    FeatureTitle = item.FeatureTitle,
                    FeatureValue = item.FeatureValue,
                    Product = product,
                    IsDelete = false,

                });
            }

            await _productFeature.AddRangeEntity(features);
            await _productFeature.SaveChanges();


            return true;
        }

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId)
        {
            if (parentId == null || parentId == 0)
            {
                return await _categoryRepository.GetQuery()
                    .Where(s => s.IsDelete == false && s.IsActive && s.ParentId == null)
                    .ToListAsync();
            }

            return await _categoryRepository.GetQuery()
                .Where(s => s.IsDelete == false && s.IsActive && s.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<bool> AddProductFeature(long productId, ProductFeatureDto productFeature)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productFeature.FeatureValue) ||
                    string.IsNullOrWhiteSpace(productFeature.FeatureTitle))
                {
                    return false;
                }

                if (productId == null || productId == 0)
                {
                    return false;
                }

                var product = await _productRepository.GetEntityById(productId);
                if (product == null)
                {
                    return false;
                }

                ProductFeature newproductFeature = new ProductFeature()
                {
                    CreateDate = DateTime.Now,
                    ProductId = product.Id,
                    FeatureTitle = productFeature.FeatureTitle,
                    FeatureValue = productFeature.FeatureValue,
                    IsDelete = false,
                };

                await _productFeature.AddEntity(newproductFeature);
                await _productFeature.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveProductFeature(long Id)
        {
            if (Id == 0 || Id == null)
            {
                return false;
            }

            var feature = await _productFeature.GetEntityById(Id);
            if (feature == null)
            {
                return false;
            }

            feature.IsDelete = true;
            feature.LastUpdateDate = DateTime.Now;
            _productFeature.EditEntity(feature);
            await _productFeature.SaveChanges();
            return true;

        }

        public async Task<EditProductDto> GetProductForEdit(long productId)
        {
            if (productId == 0 || productId == null)
            {
                return null;
            }

            var product = await _productRepository.GetEntityById(productId);

            if (product == null)
            {
                return null;
            }

            return new EditProductDto()
            {
                Description = product.Description,
                productId = product.Id,
                Inventory = product.Inventory,
                Price = product.Price,
                Image = product.ImageName,
                Title = product.Title,
                ShortDescription = product.ShortDescription,
                SelectCategories = await _selectCategoryRepository.GetQuery()
                    .Where(x => x.ProductId == product.Id)
                    .Select(x => x.ProductCategoryId)
                    .ToListAsync(),
            };
        }

        public async Task<bool> EditProduct(EditProductDto productDto, IFormFile image)
        {
            if (productDto.productId == null || productDto.productId == 0)
            {
                return false;
            }

            var product = await _productRepository.GetEntityById(productDto.productId);

            if (product == null)
            {
                return false;
            }

            string imagename = "";

            if (image != null)
            {
                imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                await image.SaveFileToServer($"{Directory.GetCurrentDirectory()}/wwwroot/content/productimgae/", imagename);
                product.ImageName = imagename;

            }

            product.Inventory = productDto.Inventory;
            product.Price = productDto.Price;
            product.ShortDescription = productDto.ShortDescription;
            product.Description = productDto.Description;
            product.LastUpdateDate = DateTime.Now;
            product.Title = productDto.Title;

            _productRepository.EditEntity(product);
            await _productRepository.SaveChanges();

            _selectCategoryRepository.DeletePermanentEntities(await _selectCategoryRepository.GetQuery().Where(x => x.ProductId == product.Id).ToListAsync());

            //Categories
            List<ProductSelectedCategory> productSelectedCategories = new List<ProductSelectedCategory>();

            foreach (var categoryId in productDto.SelectCategories)
            {
                productSelectedCategories.Add(new ProductSelectedCategory()
                {
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    ProductId = product.Id,
                    ProductCategoryId = categoryId,
                });
            }

            await _selectCategoryRepository.AddRangeEntity(productSelectedCategories);
            await _selectCategoryRepository.SaveChanges();

            return true;
        }

        public async Task<bool> SetToEndOfInventory(long productId)
        {
            if (productId == 0 || productId == null) return false;
            var product = await _productRepository.GetEntityById(productId);
            if (product == null) return false;
            if (product.Inventory <= 0) return false;

            product.Inventory = 0;
            _productRepository.EditEntity(product);
            await _productRepository.SaveChanges();
            return true;
        }
        public async Task<bool> RemoveProduct(long id)
        {
            if (id == 0 || id == null)
            {
                return false;
            }

            var product = await _productRepository.GetEntityById(id);
            if (product == null) return false;

            product.IsDelete = true;
            product.LastUpdateDate = DateTime.Now;
            _productRepository.EditEntity(product);
            await _productRepository.SaveChanges();
            return true;
        }

        public async Task<bool> ActivatedProduct(long id)
        {
            if (id == 0 || id == null)
            {
                return false;
            }

            var product = await _productRepository.GetEntityById(id);
            if (product == null) return false;

            if (product.IsActive)
            {
                product.IsActive = false;
                product.LastUpdateDate = DateTime.Now;
                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();
                return true;
            }
            product.IsActive = true;
            product.LastUpdateDate = DateTime.Now;
            _productRepository.EditEntity(product);
            await _productRepository.SaveChanges();
            return true;
        }

        public async Task<List<ProductGalleriesDto>> GetAllProductGallery(long productId)
        {
            if (productId == 0 || productId == null)
            {
                return null;
            }

            var product = await _productRepository.GetEntityById(productId);
            if (product == null) return null;

            return await _productGallery.GetQuery()
                .Where(x => x.ProductId == product.Id && x.IsDelete == false)
                .Select(x => new ProductGalleriesDto()
                {
                    Id = x.Id,
                    ImageName = x.ImageName,
                    ProductId = x.ProductId,
                })
                .ToListAsync();
        }

        public async Task<AddProductImageGalleryResult> AddProductImageGallery(long productId, IFormFile image)
        {
            try
            {
                if (productId == 0 || productId == null) return AddProductImageGalleryResult.NotFound;

                var product = await _productRepository.GetQuery()
                    .FirstOrDefaultAsync(x => x.Id == productId);

                if (product == null) return AddProductImageGalleryResult.NotFound;

                var galleries = await _productGallery.GetQuery()
                    .Where(x => x.ProductId == product.Id && x.IsDelete == false)
                    .ToListAsync();


                if (galleries.Count > 6)
                {
                    return AddProductImageGalleryResult.Error;
                }

                string imagename = "";

                if (image != null)
                {
                    imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                    await image.SaveFileToServer(
                        $"{Directory.GetCurrentDirectory()}/wwwroot/content/productimgaeGallry/{product.Id}/",
                        imagename);
                }

                ProductGallery productGallery = new ProductGallery()
                {
                    CreateDate = DateTime.Now,
                    ProductId = product.Id,
                    ImageName = imagename,
                    IsDelete = false,

                };

                await _productGallery.AddEntity(productGallery);
                await _productGallery.SaveChanges();

                return AddProductImageGalleryResult.Success;
            }
            catch
            {
                return AddProductImageGalleryResult.Error;
            }
        }

        public async Task<bool> RemoveProductImageGallery(long imageId)
        {
            if (imageId == 0 || imageId == null)
            {
                return false;
            }

            var image = await _productGallery.GetEntityById(imageId);
            if (image == null) return false;

            image.LastUpdateDate = DateTime.Now;
            image.IsDelete = true;
            _productGallery.EditEntity(image);
            await _productGallery.SaveChanges();
            return true;
        }

        public async Task<ProductDetailDto> GetProductDetailById(long productId)
        {
            if (productId == 0 || productId == null)
            {
                return null;
            }

            var product = await _productRepository.GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .ThenInclude(x => x.ProductCategory)
                .Include(x => x.ProductFeatures)
                .Include(x => x.ProductGalleries)
                .SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return null;
            }

            int counter = int.Parse(product.ViewConter);
            counter++;
            product.ViewConter = counter.ToString();
            await _productRepository.SaveChanges();

            var relatedcategory = product.ProductSelectedCategories.Select(x => x.ProductCategoryId).ToList();

            return new ProductDetailDto()
            {
                Id = product.Id,
                Price = product.Price,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                ImageName = product.ImageName,
                Title = product.Title,
                Inventory = product.Inventory,
                IsActive = product.IsActive,

                ProductFeatures = product.ProductFeatures.Where(x => x.IsDelete == false).ToList(),

                ProductGalleries = product.ProductGalleries.Where(x => x.IsDelete == false).ToList(),

                ProductCategories = product.ProductSelectedCategories.Select(x => x.ProductCategory).ToList(),

                RelatedProduct = await _productRepository.GetQuery()
                    .Include(x => x.ProductSelectedCategories)
                    .Where(x => x.Id != productId && x.IsDelete == false && x.IsActive == true && x.ProductSelectedCategories.Any(s => relatedcategory.Contains(s.ProductCategoryId))).ToListAsync(),
            };
        }

        public async Task<List<DataLayer.Entites.Product.Product>> GetProductNameForSearch(string productName)
        {
            return await _productRepository.GetQuery()
                .AsQueryable()
                .Where(x => EF.Functions.Like(x.Title, $"%{productName}%") && x.IsActive == true &&
                            x.IsDelete == false)
                .ToListAsync();
        }

        public async Task<List<ProductForIndexDto>> GetProductForIndexDto(string? UrlName)
        {
            var products = await _productRepository.GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .ThenInclude(x => x.ProductCategory)
                .Where(x => x.IsDelete == false && x.IsActive == true && x.ProductSelectedCategories.Any(f => f.ProductCategory.UrlName == UrlName))
                .OrderByDescending(x => x.CreateDate)
                .Take(20)
                .Select(x => new ProductForIndexDto
                {
                    Id = x.Id,
                    Inventory = x.Inventory,
                    ImageName = x.ImageName,
                    ViewConter = x.ViewConter,
                    Title = x.Title,
                    Price = x.Price,
                })
                .ToListAsync();

            return products;

        }

        public async Task<List<ProductForIndexDto>> GetAllProductForIndexDto()
        {
            var products = await _productRepository.GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .ThenInclude(x => x.ProductCategory)
                .Where(x => x.IsDelete == false && x.IsActive == true)
                .OrderByDescending(x => x.CreateDate)
                .Take(20)
                .Select(x => new ProductForIndexDto
                {
                    Id = x.Id,
                    Inventory = x.Inventory,
                    ImageName = x.ImageName,
                    ViewConter = x.ViewConter,
                    Title = x.Title,
                    Price = x.Price,
                })
                .ToListAsync();

            return products;
        }

        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
            await _categoryRepository.DisposeAsync();
            await _selectCategoryRepository.DisposeAsync();
        }

        public async Task<bool> AddToSpecialProduct(long id)
        {
            if (id == 0 || id == null)
            {
                return false;
            }

            var product = await _productRepository.GetEntityById(id);
            if (product == null) return false;

            if (product.IsSpecial)
            {
                product.IsSpecial = false;
                product.LastUpdateDate = DateTime.Now;
                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();
                return true;
            }
            product.IsSpecial = true;
            product.LastUpdateDate = DateTime.Now;
            _productRepository.EditEntity(product);
            await _productRepository.SaveChanges();
            return true;
        }

        public async Task<DataLayer.Entites.Product.Product> GetSpecialProduct()
        {

            var product = await _productRepository.GetQuery()
                .Where(x => x.IsSpecial == true && x.IsActive == true && x.IsDelete == false)
                .FirstOrDefaultAsync();

            if (product == null) return null;

            return product;
        }
    }
}
