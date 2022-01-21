using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface IShopService
	{
		Article GetArticle(int id);
		Article GetArticleInPriceRange(int id, int maxExpectedPrice);
		void OrderArticle(int id, int maxExpectedPrice);
		void SellArticle(int id, int buyerId);
	}
}