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
			var article = _shopService.GetArticle(id);
			if (article is null)	// article not on stock
			{				
				_shopService.OrderArticle(id, maxExpectedPrice, buyerId);
				// if not found among suppliers... return NULL?
			}
			// if finalley exists then
			_shopService.SellArticle(id,maxExpectedPrice,buyerId);			
		}

		public Article GetById(int id)
		{
			var article = _shopService.GetArticle(id);
			return article;
		}
	}
}
