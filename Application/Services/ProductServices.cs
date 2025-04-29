using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Application.InterfaceRepository;
using BaharShop.Domain.UploadFile;
using BaharShop.Domain;
using BaharShop.Application.Services.Dto;

namespace BaharShop.Application.Services
{
    public class ProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploadService _fileUploadService;

        public ProductServices(IProductRepository productRepository, IFileUploadService fileUploadService)
        {
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
       
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _productRepository.GetAll();
            return products;
        }
     
      

        public async Task<Product> GetProductById(long id)
        {
            return await _productRepository.GetById(id);
        }


        public async Task<IEnumerable<Product>> GetProductsWithCategories(Expression<Func<Product, bool>> where = null)
        {
            var products = await _productRepository.GetAllWithCategory(where);
            return products;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId)
        {
            return await _productRepository.GetProductsByCategoryId(categoryId);
        }


        public async Task CreateProduct(ProductDto productDto)
        {
            if (productDto.Img == null)
            {
                throw new Exception("Image is required."); 
            }

            var product = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                IsAvail = productDto.IsAvail,
                ShomeHomePage = productDto.ShomeHomePage,
                InsertTime = DateTime.Now,
                ImagePath = await _fileUploadService.UploadFileAsync(productDto.Img),
                Img = await _fileUploadService.UploadFileAsync(productDto.Img) 
            };

            await _productRepository.Add(product);
        }


        public async Task UpdateProduct(Product product)
        {
            await _productRepository.Update(product);

        }
        public async Task UpdateProduct(ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.GetById(productDto.Id);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                // بررسی مقدار فیلدها قبل از آپدیت
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.Stock = productDto.Stock;
                product.CategoryId = productDto.CategoryId;
                product.IsAvail = productDto.IsAvail;
                product.ShomeHomePage = productDto.ShomeHomePage;
                //اگر تصویر جدیدی برای محصول ارسال شده باشد 
                if (productDto.Img != null)
                {
                    var uploadedFilePath = await _fileUploadService.UploadFileAsync(productDto.Img);
                    product.Img = uploadedFilePath;
                    product.ImagePath = uploadedFilePath;
                }
                else
                {
                    product.ImagePath = productDto.ImagePath;
                }

           
                await _productRepository.Update(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product: {ex.Message}", ex);
            }
        }







        public async Task DeleteProduct(long id)
        {
            await _productRepository.Delete(id);
        }

        public async Task<PagedProductDto> GetProductPagination(int page, int pageSize, string? search, string sortOrder = "newest")
        {
            
            var products = await _productRepository.GetAll(); 

          
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search)).ToList(); 
            }

            switch (sortOrder)
            {
                case "highest":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "cheapest":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                default: // newest
                    products = products.OrderByDescending(p => p.InsertTime).ToList();
                    break;
            }

         
            int totalCount = products.Count();
            int totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

          
            products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

           
            var productDtos = products.Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsAvail = p.IsAvail,
                ShomeHomePage = p.ShomeHomePage,
                ImagePath = p.ImagePath
            }).ToList();

        
            var result = new PagedProductDto()
            {
                Items = productDtos,
                Page = page,
                TotalPage = totalPage,
            };

            return result;
        }
      

        public async Task<bool> RateProduct(long productId, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            var product = await _productRepository.GetById(productId);
            if (product == null)
            {
                return false; // محصول یافت نشد
            }

            // به روز رسانی امتیاز
            product.TotalRatings++;
            product.AverageRating = ((product.AverageRating * (product.TotalRatings - 1)) + rating) / product.TotalRatings;

            await _productRepository.Update(product);
            return true;
        }


    }
}
