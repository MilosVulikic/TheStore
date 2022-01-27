using System;
using TheShop.DAL.Models;
using TheShop.Services.Interfaces;
using TheShop.DAL.Interfaces;
using log4net;
using System.Reflection;

namespace TheShop.Services
{
	public class ShopService : IShopService
	{
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		IArticleRepository _articleRepository;
		ISupplierService _supplierService;						

		public ShopService(IArticleRepository articleRepository, ISupplierService supplierService)
		{
			_articleRepository = articleRepository;						
			_supplierService = supplierService;			
		}

		
		public Article GetArticle(int articleId)
		{
			var article = _articleRepository.Get(articleId);
			if (article != null)
				_logger.Debug($"Found article with ArticleId: {articleId}");
			else
				_logger.Debug($"Not found article with ArticleId: {articleId}");
			return article;
		}

		public Article GetArticleInPriceRange(int articleId, int maxExpectedPrice)
		{
			_logger.Debug($"Getting from local stock Article with ArticleId: {articleId}, with price less than: {maxExpectedPrice}");

			var article = _articleRepository.Get(articleId);
			if (article != null && article.Price <= maxExpectedPrice)			
				return article;
			
			_logger.Debug($"Not found on local stock Article with ArticleId: {articleId}, with price less than: {maxExpectedPrice}");
			return null;
		}

		public Article OrderArticle(int articleId, int maxExpectedPrice)
		{
			_logger.Debug($"Trying to order Article with ArticleId: {articleId}.");
			Article article = null;
			article = _supplierService.GetArticleFromAnySupplier(articleId, maxExpectedPrice);

			if (article != null)			
				_articleRepository.Create(article);
				
			return article;
		}

		public Article SellArticle(int articleId, int buyerId)
		{
			_logger.Debug($"Trying to sell article with ArticleId: {articleId}");
			var article = _articleRepository.GetNonSold(articleId);
			if (article != null)
			{				
				article.IsSold = true;
				article.SoldDate = DateTime.Now;
				article.BuyerUserId = buyerId;
				
				article = _articleRepository.Update(article);
				if (article != null)
					_logger.Debug($"Sold article with ArticleId: {articleId}");
			}
			
			return article;
		}

	}

}
