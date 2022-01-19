using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface ISupplier
	{
		bool ArticleInInventory(int id);
		Article GetArticle(int id);
	}
}