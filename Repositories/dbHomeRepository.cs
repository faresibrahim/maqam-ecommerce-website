using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Repositories
{
    public class dbHomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public dbHomeRepository(ApplicationDbContext db)
        {
            _dbContext = db;

        }


        public async Task<IEnumerable<Category>> DisplayCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        //explain the following function on chatgpt
        public async Task<IEnumerable<Product>> DisplayProducts(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _dbContext.Products
                            join category in _dbContext.Categories
                            on product.CategoryId equals category.CategoryId
                            join stock in _dbContext.Stocks
                            on product.ProductId equals stock.ProductId
                            into product_stocks
                            from productWithStock in product_stocks.DefaultIfEmpty()
                            where string.IsNullOrEmpty(sTerm) || (product != null && product.ProductName
                            .ToLower().StartsWith(sTerm))
                            select new Product
                            {

                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                CategoryId = product.CategoryId,
                                Image = product.Image,
                                Price = product.Price,
                                CategoryName = category.CategoryName,
                              //  Quantity = product.Quantity,
                                Description = product.Description,
                                Quantity=productWithStock==null?0:productWithStock.Quantity
                            }
                            ).ToListAsync();
            //are we getting an id from category? If we are getting an id from category, then we need to filter the products by category id
            if (categoryId > 0)
            {
                products = products.Where(a=>a.CategoryId == categoryId).ToList();
            }
            return products;
        }
    }
}

