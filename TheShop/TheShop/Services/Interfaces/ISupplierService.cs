using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface ISupplierService
	{
		bool ArticleInInventory(int typeId, ISupplier supplier);
		Article GetArticle(int typeId, ISupplier supplier);
		Article GetArticleFromAnySupplier(int typeId, int maxExpectedPrice);
		Article GetArticleFromSupplier(int ArticleId, int maxExpectedPrice, ISupplier supplier);
	}
}