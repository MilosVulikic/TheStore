using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface IShopService
	{
		Article GetArticle(int articleId);
		Article GetArticleInPriceRange(int articleId, int maxExpectedPrice);
		Article OrderArticle(int articleId, int maxExpectedPrice);
		Article SellArticle(int articleId, int buyerId);		
	}
}