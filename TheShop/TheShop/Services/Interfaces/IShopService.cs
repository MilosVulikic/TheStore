using TheShop.DAL.Models;

namespace TheShop.Services.Interfaces
{
	public interface IShopService
	{
		Article GetById(int id);
		void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId);
	}
}