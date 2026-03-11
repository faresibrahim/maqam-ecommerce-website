namespace MusicShoppingCartMvcUI.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByProductId(int bookId);
        Task ManageStock(StockDTO stockToManage);
    }
}
