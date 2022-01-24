using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface ISupplierService
	{
		bool ArticleInInventory(int id, ISupplier supplier);
		Article GetArticle(int id, ISupplier supplier);
		Article GetArticleFromAnySupplier(int id, int maxExpectedPrice);
		Article GetArticleFromSupplier(int id, int maxExpectedPrice, ISupplier supplier);
	}
}