namespace MusicShoppingCartMvcUI.Repositories
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task<Category?> GetCategoryById(int id);
        Task DeleteCategory(Category Category);
        Task<IEnumerable<Category>> GetCategories();
    }
}
