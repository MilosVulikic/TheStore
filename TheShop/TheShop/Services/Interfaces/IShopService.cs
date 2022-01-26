using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface IShopService
	{
		Article GetArticle(int id);
		Article GetArticleInPriceRange(int id, int maxExpectedPrice);
		Article OrderArticle(int id, int maxExpectedPrice);
		Article SellArticle(int id, int buyerId);		
	}
}