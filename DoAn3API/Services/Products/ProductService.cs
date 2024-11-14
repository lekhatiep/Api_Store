using AutoMapper;
using Dapper;
using DoAn3API.Constants;
using DoAn3API.DataContext;
using DoAn3API.Dtos.ProductImages;
using DoAn3API.Dtos.Products;
using DoAn3API.Extensions;
using DoAn3API.Services.Categories;
using DoAn3API.Services.Firebase;
using DoAn3API.Services.StoreService;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using Infastructure.Repositories.Catalogs.ProductCategoryRepo;
using Infastructure.Repositories.ProductImageRepo;
using Infastructure.Repositories.ProductRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DoAn3API.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly ICategoryService _categoryService;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IFirebaseService _firebaseService;
        private readonly DapperContext _dapperContext;

        public ProductService(
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IMapper mapper,
            IStorageService storageService,
            ICategoryService categoryService,
            IProductCategoryRepository productCategoryRepository,
            IFirebaseService firebaseService,
            DapperContext dapperContext
            )
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _mapper = mapper;
            _storageService = storageService;
            _categoryService = categoryService;
            _productCategoryRepository = productCategoryRepository;
            _firebaseService = firebaseService;
            _dapperContext = dapperContext;
        }

        public async Task<int> CreateProduct(CreateProductDto productDto)
        {
            var newProduct = _mapper.Map<Product>(productDto);

            //Save image

            if (productDto.ThumbnailImage != null)
            {
                newProduct.ProductImages = new List<ProductImage>
                {
                    new ProductImage
                    {
                        Caption = "Thumnail Images",
                        CreateTime = DateTime.Now,
                        IsDefault = true,
                        SortOrder = 1,
                        FileSize = productDto.ThumbnailImage.Length,
                        //ImagePath = await this.SaveFile(productDto.ThumbnailImage)
                        ImagePath = await _firebaseService.UploadFileAsync(productDto.ThumbnailImage)
                    }

                };
            }
            else
            {
                newProduct.ProductImages = new List<ProductImage>
                {
                    new ProductImage
                    {
                        Caption = "Thumnail Images Default",
                        CreateTime = DateTime.Now,
                        IsDefault = true,
                        SortOrder = 1,
                        FileSize = 0,
                        ImagePath =  "/" + SystemConstant.ProductSettings.IMG_FOLDER_NAME + "/" + "default-image.jpg"
                    }

                };
            }

            var newProductId = await _productRepository.AddProductReturnId(newProduct);


            return newProductId;
        }

        public async Task<List<ProductDto>> GetAllProduct()
        {
            var query = _productRepository.List();

            if (!query.Any())
            {
                return null;
            }

            var listProduct = await query
                .Include(x=>x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete == false))
                .Where(x => x.IsDelete == false).ToListAsync();

            var listProductDto = _mapper.Map<List<ProductDto>>(listProduct);

            return listProductDto;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var queryProductImages = _productImageRepository.List();

            var product = await _productRepository.GetById(id);
            var productImages = await queryProductImages.Where(x=>x.ProductId == id).ToListAsync();
            var ortherProductImages  = await queryProductImages.Where(x => x.ProductId == id && x.IsDefault != true && x.IsDelete == false).ToListAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.ImagePath = productImages.Count != 0 ? productImages.Where(x => x.IsDefault == true).FirstOrDefault().ImagePath : "";
            productDto.ProductImages = _mapper.Map<List<ProductImageDto>>(ortherProductImages);

            return productDto;
        }

        public async Task<ProductDto> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var product = await _productRepository.GetById(updateProductDto.Id);
            var queryProductImages = _productImageRepository.List();

            if (product == null)
            {
                return null;
            }

            if (updateProductDto.ThumbnailImage !=null)
            {
                var thumbnailImage = await queryProductImages.Where(x => x.ProductId == product.Id && x.IsDefault == true).FirstOrDefaultAsync();

                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = updateProductDto.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await _firebaseService.UploadFileAsync(updateProductDto.ThumbnailImage);

                    await _productImageRepository.Update(thumbnailImage, thumbnailImage.Id);
                }
            }

            var updateProduct = _mapper.Map<UpdateProductDto, Product>(updateProductDto);
            var productUpdated = await _productRepository.Update(updateProduct, updateProduct.Id);

            return _mapper.Map<Product, ProductDto>(productUpdated);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }

        private async Task<string> SaveFile(IFormFile formFile)
        {
            var orginalName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(orginalName)}";
            await _storageService.SaveFileAsync(formFile.OpenReadStream(), fileName);

            return "/" + SystemConstant.ProductSettings.USET_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public PagedList<ProductDto> GetAllProductPaging(ProductPagedRequestDto  requestDto)
        {
            var queryProduct = _productRepository.List();

            if (!queryProduct.Any())
            {
                return null;
            }
            //Prepare query
            var listProduct = queryProduct
                .Include(x => x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete == false))
                .Where(x => x.IsDelete == false);

            if(requestDto.CategoryID > 0)
            {
                listProduct = _categoryService.GetAllProductByCategoryId(requestDto, requestDto.CategoryID);
            }

            if (!string.IsNullOrEmpty(requestDto.Search))
            {
                var searchTextConvert = StringExtension.RemoveDiacritics(requestDto.Search);

                listProduct = listProduct.Where(x => x.SeoTitle.ToLower().Contains(searchTextConvert.ToLower()));
            }


            #region SORTING
            //Default sort by date created
            listProduct = listProduct.OrderBy(x => x.CreateTime);

            if (!string.IsNullOrEmpty(requestDto.SortBy))
            {
                switch (requestDto.SortBy)
                {
                    case "new_desc":
                        listProduct = listProduct.OrderBy(x => x.CreateTime);
                        break;

                    case "popular_desc":
                        listProduct = listProduct.OrderByDescending(x => x.ViewCount);
                        break;

                    case "price_desc":
                        listProduct = listProduct.OrderByDescending(x => x.Price);
                        break;
                    case "price_asc":
                        listProduct = listProduct.OrderBy(x => x.Price);
                        break;

                    default:
                        break;
                }
            }

            #endregion SORTING
            //Paging
            var pageList = PagedList<Product>.ToPagedList(ref listProduct, requestDto.PageNumber, requestDto.PageSize );

            var dataResult = _mapper.Map<PagedList<ProductDto>>(pageList);

            return dataResult;

        }

        public async Task UpdateDefaultImage(UpdateProductDto updateProductDto)
        {
            var product = await _productRepository.GetById(updateProductDto.Id);

            if (product == null)
            {
                 throw new NotImplementedException();
            }

            var productImages =
                    new ProductImage
                    {
                        ProductId = product.Id,
                        Caption = "Thumnail Images Default",
                        CreateTime = DateTime.Now,
                        IsDefault = true,
                        SortOrder = 1,
                        FileSize = 0,
                        ImagePath = "/" + SystemConstant.ProductSettings.IMG_FOLDER_NAME + "/" + "default-image.jpg"
                    };

            await _productImageRepository.Insert(productImages);
            await _productImageRepository.Save();

        }

        public PagedList<ProductDto> GetProductByCategoryId(ProductPagedRequestDto requestDto, int categoryId)
        {
            var listProduct = _categoryService.GetAllProductByCategoryId(requestDto, categoryId);

            #region SORTING
            listProduct = Sorting(requestDto, listProduct);

            #endregion SORTING
            //Paging
            var pageList = PagedList<Product>.ToPagedList(ref listProduct, requestDto.PageNumber, requestDto.PageSize);

            var dataResult = _mapper.Map<PagedList<ProductDto>>(pageList);

            return dataResult;
        }

        private static IQueryable<Product> Sorting(ProductPagedRequestDto requestDto, IQueryable<Product> listProduct)
        {
            #region SORTING
            //Default sort by date created
            listProduct = listProduct.OrderBy(x => x.CreateTime);

            if (!string.IsNullOrEmpty(requestDto.SortBy))
            {
                switch (requestDto.SortBy)
                {
                    case "new_desc":
                        listProduct = listProduct.OrderBy(x => x.CreateTime);
                        break;

                    case "popular_desc":
                        listProduct = listProduct.OrderByDescending(x => x.ViewCount);
                        break;

                    case "price_desc":
                        listProduct = listProduct.OrderByDescending(x => x.Price);
                        break;
                    case "price_asc":
                        listProduct = listProduct.OrderBy(x => x.Price);
                        break;

                    default:
                        break;
                }
            }


            return listProduct;
            #endregion SORTING
        }

        public PagedList<ProductDto> GetProductByName(ProductPagedRequestDto requestDto)
        {

            var queryProduct = _productRepository.List();
            var queryProductCats = _productCategoryRepository.List();

            if (!queryProduct.Any() || String.IsNullOrEmpty(requestDto.Search))
            {
                return new PagedList<ProductDto>(new List<ProductDto>(),1,1,1);
            }

            //Prepare query
            var searchTextConvert = StringExtension.RemoveDiacritics(requestDto.Search);

             var listProduct = queryProduct
                .Include(x => x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete == false))
                .Where(x => x.IsDelete == false
                     && x.SeoTitle.ToLower().Contains(searchTextConvert.ToLower())
                );


            #region SORTING
            //Default sort by date created
            listProduct = listProduct.OrderBy(x => x.Title);   

            #endregion SORTING
            //Paging
            var pageList =  PagedList<Product>.ToPagedList(ref listProduct, requestDto.PageNumber, requestDto.PageSize);

            var dataResult = _mapper.Map<PagedList<ProductDto>>(pageList);

            return dataResult;
        }

        

        public async Task UpdateSEOTitle(UpdateProductDto updateProductDto)
        {
            var product = await _productRepository.GetById(updateProductDto.Id);

            if (product == null)
            {
                throw new NotImplementedException();
            }

            product.SeoTitle = StringExtension.RemoveDiacritics(product.Title ?? "");
            product.SeoDescription = StringExtension.RemoveDiacritics(product.Description ?? "");

            await _productRepository.Update(product, product.Id);

        }

        public async Task AddOrRemoveQuantityStock(int productID, int quantity)
        {
            using (var con = _dapperContext.CreateConnection())
            {
                var sql = $@"UPDATE Products SET Quantity = {quantity} WHERE Id = {productID}";

                await con.ExecuteAsync(sql);

            }
        }
    }
}
