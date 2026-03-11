using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicShoppingCartMvcUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetCategories();
            return View(categories);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            try
            {
                var CategoryToAdd = new Category { CategoryName = category.CategoryName, CategoryId = category.CategoryId, Icon = category.Icon };
                await _categoryRepo.AddCategory(CategoryToAdd);
                TempData["successMessage"] = "Category added successfully";
                return RedirectToAction(nameof(AddCategory));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Category could not added!";
                return View(category);
            }

        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryRepo.GetCategoryById(id);
            if (category is null)
                throw new InvalidOperationException($"Category with id: {id} is not found");
            var categoryToUpdate = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
            return View(categoryToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryDTO categoryToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryToUpdate);
            }
            try
            {
                var category = new Category { CategoryName = categoryToUpdate.CategoryName, CategoryId = categoryToUpdate.CategoryId };
                await _categoryRepo.UpdateCategory(category);
                TempData["successMessage"] = "Category is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Category could not updated!";
                return View(categoryToUpdate);
            }

        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetCategoryById(id);
            if (category is null)
                throw new InvalidOperationException($"Category with id: {id} is not found");
            await _categoryRepo.DeleteCategory(category);
            return RedirectToAction(nameof(Index));

        }

    }
}