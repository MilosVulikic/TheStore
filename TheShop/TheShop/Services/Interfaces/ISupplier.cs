using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface ISupplier
	{
		bool ArticleInInventory(int articleId);
		Article GetArticle(int articleId);
	}
}