using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShop.Controllers
{
	class ShopController
	{
		IShopService _shopService;

		public ShopController()
		{
			_shopService = new ShopService(new ArticleRepository());
		}

		public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
		{
			_shopService.OrderAndSellArticle(id,maxExpectedPrice,buyerId);			
		}

		public Article GetById(int id)
		{
			var article = _shopService.GetById(id);
			return article;
		}
	}
}
