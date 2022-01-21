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
			var article = _shopService.GetArticleInPriceRange(id, maxExpectedPrice);
			if (article is null)
			{				
				_shopService.OrderArticle(id, maxExpectedPrice, buyerId);				
			}			
			_shopService.SellArticle(id,maxExpectedPrice,buyerId);			
		}

		public Article GetById(int id)
		{
			var article = _shopService.GetArticle(id);
			return article;
		}
	}
}
