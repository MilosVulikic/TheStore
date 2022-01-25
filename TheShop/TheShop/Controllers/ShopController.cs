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
			_shopService = new ShopService(new ArticleRepository(new DAL.ApplicationDbContext()), new SupplierService());
		}

		public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)	// we need to inform whether it was successful - either bool or articleDTO should be returned
		{			
			var article = _shopService.GetArticleInPriceRange(id, maxExpectedPrice);
			if (article is null)
			{				
				_shopService.OrderArticle(id, maxExpectedPrice);				
			}			
			_shopService.SellArticle(id,buyerId);			
		}

		public Article GetById(int id)
		{
			var article = _shopService.GetArticle(id);
			return article;
		}
	}
}
