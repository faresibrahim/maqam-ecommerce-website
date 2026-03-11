using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicShoppingCartMvcUI.Shared;

namespace MusicShoppingCartMvcUI.Controllers
{
    [Authorize(Roles=nameof(Roles.Admin))]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
        }


        public async Task<IActionResult> Index()
        {
            var products =await _productRepository.GetProducts();
            return View(products);
        }

        public async Task<IActionResult> AddProduct() //Explain the following method
        {
            var categorySelectList = (await _categoryRepository.GetCategories()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.CategoryId.ToString(),
            });
            ProductDTO productToAdd = new() { CategoryList = categorySelectList };
            return View(productToAdd);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO productToAdd)
        {
            // Get categories from database and convert them to a SelectList for the dropdown
            var categorySelectList = (await _categoryRepository.GetCategories()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.CategoryId.ToString(),
            });

            productToAdd.CategoryList = categorySelectList;

            // If validation fails, return the same view with the entered data
            if (!ModelState.IsValid)
                return View(productToAdd);

            try
            {
                // Handle image upload if an image was provided
                if (productToAdd.ImageFile != null)
                {
                    // Validate file size (max 1 MB)
                    if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file cannot exceed 1 MB");
                    }

                    // Allowed image extensions
                    string[] allowedExtensions = [".jpeg", ".jpg", ".png"];

                    // Save the image using the file service
                    string imageName = await _fileService.SaveFile(productToAdd.ImageFile, allowedExtensions);

                    // Store the file name in the DTO
                    productToAdd.Image = imageName;
                }

                // Manual mapping from ProductDTO → Product entity
                Product product = new()
                {
                    ProductId = productToAdd.ProductId,
                    ProductName = productToAdd.ProductName,
                    Description = productToAdd.Description,
                    Image = productToAdd.Image,
                    CategoryId = productToAdd.CategoryId,
                    Price = productToAdd.Price,
                    
                };

                // Save product to database
                await _productRepository.AddProduct(product);

                TempData["successMessage"] = "Product added successfully";

                return RedirectToAction(nameof(AddProduct));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToAdd);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToAdd);
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Error while saving data";
                return View(productToAdd);
            }
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                TempData["errorMessage"] = $"Product with the id: {id} was not found";
                return RedirectToAction(nameof(Index));
            }

            // Get categories and build the dropdown list
            var categorySelectList = (await _categoryRepository.GetCategories()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.CategoryId.ToString(),
                Selected = category.CategoryId == product.CategoryId
            });

            // Map Product → ProductDTO
            ProductDTO productToUpdate = new()
            {
                CategoryList = categorySelectList,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Image = product.Image
            };

            return View(productToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductDTO productToUpdate)
        {
            var categorySelectList = (await _categoryRepository.GetCategories()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.CategoryId.ToString(),
                Selected = category.CategoryId == productToUpdate.CategoryId
            });

            productToUpdate.CategoryList = categorySelectList;

            if (!ModelState.IsValid)
                return View(productToUpdate);

            try
            {
                string oldImage = "";

                if (productToUpdate.ImageFile != null)
                {
                    if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file cannot exceed 1 MB");
                    }

                    string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                    string imageName = await _fileService.SaveFile(productToUpdate.ImageFile, allowedExtensions);

                    // Store old image name so it can be deleted after successful update
                    oldImage = productToUpdate.Image;
                    productToUpdate.Image = imageName;
                }

                // Manual mapping of ProductDTO -> Product
                Product product = new()
                {
                    ProductId = productToUpdate.ProductId,
                    ProductName = productToUpdate.ProductName,
                    Description = productToUpdate.Description,
                    CategoryId = productToUpdate.CategoryId,
                    Price = productToUpdate.Price,
                    Image = productToUpdate.Image
                };

                await _productRepository.UpdateProduct(product);

                // If a new image was uploaded, delete the old image from the folder
                if (!string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }

                TempData["successMessage"] = "Product updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToUpdate);
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Error while saving data";
                return View(productToUpdate);
            }
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product == null)
                {
                    TempData["errorMessage"] = $"Product with the id: {id} was not found";
                }
                else
                {
                    await _productRepository.DeleteProduct(product);

                    if (!string.IsNullOrWhiteSpace(product.Image))
                    {
                        _fileService.DeleteFile(product.Image);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Error while deleting the data";
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
