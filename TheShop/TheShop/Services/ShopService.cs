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
		IUnitOfWork _unitOfWork;
		ISupplierService _supplierService;						

		public ShopService(IUnitOfWork unitOfWork, ISupplierService supplierService)
		{
			_unitOfWork = unitOfWork;				
			_supplierService = supplierService;			
		}

		
		public Article GetArticle(int articleId)
		{
			var article = _unitOfWork.Articles.Get(articleId);
			if (article != null)
				_logger.Debug($"Found article with ArticleId: {articleId}");
			else
				_logger.Debug($"Not found article with ArticleId: {articleId}");
			return article;
		}

		public Article GetArticleInPriceRange(int articleId, int maxExpectedPrice)
		{
			_logger.Debug($"Getting from local stock Article with ArticleId: {articleId}, with price less than: {maxExpectedPrice}");

			var article = _unitOfWork.Articles.Get(articleId);
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
			{
				_unitOfWork.Articles.Add(article);
				 _unitOfWork.Complete();
			}

			return article;
		}

		public Article SellArticle(int id, int buyerId)
		{
			_logger.Debug($"Trying to sell article with ArticleId: {id}");
			var article = _unitOfWork.Articles.GetNonSold(id);
			if (article != null)
			{				
				article.IsSold = true;
				article.SoldDate = DateTime.Now;
				article.BuyerUserId = buyerId;
				
				_unitOfWork.Complete();
				if (article != null)
					_logger.Debug($"Sold article with ArticleId: {id}");
			}
			
			return article;
		}

	}

}
